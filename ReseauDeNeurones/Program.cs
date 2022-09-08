using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libReseauDeNeurones;
using System.IO;
using System.Globalization;

namespace ReseauDeNeurones
{
    class Program
    {
        static void Main(string[] args)
        {
            string temp;
            int nbNeuronesEntrant, nbNeuronesSortant;

           
            Console.WriteLine("Bonjour, bienvenue sur le réseau de neurones");


            //choix du fichier
            bool res = false;
            int numChoix = 0;
            while (res == false)
            {
                Console.WriteLine("\nVeuillez choisir un fichier pour commencer :");
                Console.WriteLine("1 - Qualité du vin");
                Console.WriteLine("2 - Potabilité de l'eau");
                Console.WriteLine("3 - Fonction Xor");
                Console.WriteLine("4 - Fonction Or");
                Console.WriteLine("5 - Fonction and");
                Console.WriteLine("6 - Fonction Or avec des Vrai ou Faux");
                Console.WriteLine("7 - Choisir un autre fichier en tapant le chemin");
                Console.Write("Veuillez entre le numéro du fichier choisi : ");
                string saisie = Console.ReadLine();
                res = int.TryParse(saisie, out numChoix);
                if (res == false)
                {
                    Console.WriteLine("la saisie n'est pas correcte.");
                }
                else
                {
                    if (numChoix > 7 || numChoix < 1)
                    {
                        res = false;
                        Console.WriteLine("la saisie ne peut pas être supérieur à 5 ou inférieur à 1.");
                    }
                }

            }
            string documentChoisi = "../../csv/WineQT.csv";
            switch (numChoix)
            {
                case 1:
                    documentChoisi = "../../csv/WineQT.csv";
                    break;
                case 2:
                    documentChoisi = "../../csv/water_potability.csv";
                    break;
                case 3:
                    documentChoisi = "../../csv/Xor.csv";
                    break;
                case 4:
                    documentChoisi = "../../csv/Or.csv";
                    break;
                case 5:
                    documentChoisi = "../../csv/And.csv";
                    break;
                case 6:
                    documentChoisi = "../../csv/OrChaine.csv";
                    break;
                case 7:
                    Console.Write("Tapez le chemin du fichier csv voulue : ");
                    documentChoisi = Console.ReadLine();
                    break;
            }


            //lecture des colonnes
            var reader = new StreamReader(File.OpenRead(documentChoisi));
            List<string> lesColonnes = new List<string>();
            bool premiereLigne = true;
            List<double> valeurMaxNormalisation = new List<double>();
            List<List<string>> valeurChaineNormalise = new List<List<string>>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var valeurs = line.Split(',');
                if (premiereLigne == true)
                {
                    for (int j = 0; j < valeurs.Count(); j++)
                    {
                        lesColonnes.Add(valeurs[j]);
                        valeurChaineNormalise.Add(new List<string>());
                        Console.WriteLine(j + 1 + " " + valeurs[j]);
                    }
                    premiereLigne = false;
                }
                else
                {
                    //récuperation des valeurs les plus grandes pour pouvoir mettre toutes les valeurs du fichier entre 0 et 1
                    for (int i = 0; i < valeurs.Count(); i++)
                    {
                        if (valeurMaxNormalisation.Count() <= i)
                        {
                            double doubleTemp;
                            bool valeurChaine = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                            if (valeurChaine == true)
                            {
                                valeurMaxNormalisation.Add(doubleTemp);
                            }
                            else
                            {
                                valeurChaineNormalise[i].Add(valeurs[i]);
                                valeurMaxNormalisation.Add(0);

                            }

                        }
                        else
                        {
                            double doubleTemp;
                            bool valeurChaine = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                            if (valeurChaine == true)
                            {
                                if (valeurMaxNormalisation[i] < doubleTemp)
                                {
                                    valeurMaxNormalisation[i] = doubleTemp;
                                }
                            }
                            else
                            {
                                if (valeurChaineNormalise[i].Contains(valeurs[i]) == false)
                                {
                                    
                                    valeurChaineNormalise[i].Add(valeurs[i]);
                                    valeurMaxNormalisation[i] = valeurMaxNormalisation[i] + 1;
                                }
                               
                            }



                           
                        }
                    }
                }
            }


            //selection des colonnes calculées.
            bool autreColonne = true;
            List<int> lesColonnesCalcules = new List<int>();
            int numTemp;
            bool saisieColonne = true;
            while (saisieColonne == true)
            {
                Console.Write("Mettez le numéro de la colonne que vous voules faire calculer à votre réseau : ");
                temp = Console.ReadLine();
                int.TryParse(temp, out numTemp);
                if (numTemp > lesColonnes.Count())
                {
                    Console.WriteLine("Erreur !!! Vous avez selectionné une colonne qui n'existe pas");
                }
                else
                {
                    saisieColonne = false;
                    lesColonnesCalcules.Add(numTemp-1);
                }
            }

          

            //ajout d'une autre colonne à faire calculer au réseau
            while (autreColonne == true)
            {
                Console.Write("\nVoulez vous ajouter une autre colonne à calculer (o ou n) : ");
                temp = Console.ReadLine();
                if (temp == "o")
                {
                    Console.Write("\nMettez le numéro de la colonne que vous voules faire calculer à votre réseau : ");
                    temp = Console.ReadLine();
                    int.TryParse(temp, out numTemp);
                    bool erreurColonne = false;
                    foreach (int i in lesColonnesCalcules)
                    {
                        if (i == numTemp-1)
                        {
                            erreurColonne = true;
                            Console.WriteLine("Erreur !!! La colonne a déjà été selectionnée.");
                        }

                    }
                    if (numTemp > lesColonnes.Count())
                    {
                        erreurColonne = true;
                        Console.WriteLine("Erreur !!! Vous avez selectionné une colonne qui n'existe pas");
                    }
                    if (erreurColonne == false)
                    {
                        lesColonnesCalcules.Add(numTemp-1);
                    }
                }
                else
                {
                    autreColonne = false;
                }

            }

      

            //presentation des colonnes calculées
            Console.WriteLine("Voici le(s) colonne(s) que vous avez selectionné : ");
            for (int i = 0; i < lesColonnes.Count(); i++)
            {
                foreach (int j in lesColonnesCalcules)
                {
                    if (i == j)
                    {
                        Console.WriteLine(lesColonnes[i]);
                    }
                }
            }
            nbNeuronesEntrant = lesColonnes.Count() - lesColonnesCalcules.Count();
            Console.WriteLine("Il va y avoir " + nbNeuronesEntrant + " colonnes en entrée dans le réseau de neurones ");

           
            
            //création des couches
            List<List<Neurone>> lesCouches = new List<List<Neurone>>();
            nbNeuronesSortant = lesColonnesCalcules.Count();
            bool premiereCouche = true;
            int nbNeuronesMax = nbNeuronesEntrant;
            int nbNeuronesCoucheAvant = 0;

            Console.Write("Voulez-vous créer manuellement les couches intermédaires (o ou n) : ");
            temp = Console.ReadLine();
            if (temp == "o")
            {
                //création des couches intermédaires manuellement
                List<Neurone> laPremiereCouche = new List<Neurone>();
                for (int i = 0; i < nbNeuronesMax; i++)
                {
                    laPremiereCouche.Add(new Neurone(1, 1));
                }
                lesCouches.Add(laPremiereCouche);
                nbNeuronesCoucheAvant = nbNeuronesMax;
                bool saisieCouche = true;
                int nbNeuronesSaisis;
                while (saisieCouche == true)
                {
                    int numeroCouche = lesCouches.Count() + 1;
                    Console.Write("Combien de neurones pour la couche N°" + numeroCouche+" : ");
                    temp = Console.ReadLine();
                    int.TryParse(temp, out nbNeuronesSaisis);
                    if (nbNeuronesSaisis < nbNeuronesSortant)
                    {
                        Console.WriteLine("Erreur !!! on ne peut pas mettre un nombre de neurones pour une couche intermédiare inférieur au nombre de neurones de la dernière couche");
                    }
                    else
                    {
                        List<Neurone> uneCouche = new List<Neurone>();
                        for (int nbNeurone = 0; nbNeurone < nbNeuronesSaisis; nbNeurone++)
                        {
                            uneCouche.Add(new Neurone(nbNeuronesCoucheAvant));
                        }
                        nbNeuronesCoucheAvant = nbNeuronesSaisis;
                        lesCouches.Add(uneCouche);
                    }
                    Console.Write("Voulez vous ajouter une autre couche intermédiaire (o ou n) : ");
                    temp = Console.ReadLine();
                    if (temp == "n")
                    {
                        saisieCouche = false;
                    }

                }
            }
            else
            {
                //création des couches et des neurones autamtiquement

                while (nbNeuronesMax > nbNeuronesSortant || premiereCouche == true)
                {
                    if (premiereCouche == true)
                    {
                        List<Neurone> uneCouche = new List<Neurone>();
                        for (int i = 0; i < nbNeuronesMax; i++)
                        {
                            uneCouche.Add(new Neurone(1, 1));
                        }
                        lesCouches.Add(uneCouche);
                        nbNeuronesCoucheAvant = nbNeuronesMax;
                        nbNeuronesMax = nbNeuronesMax / 2;
                        premiereCouche = false;
                    }
                    else
                    {
                        List<Neurone> uneCouche = new List<Neurone>();
                        for (int i = 0; i < nbNeuronesMax; i++)
                        {
                            uneCouche.Add(new Neurone(nbNeuronesCoucheAvant));
                        }
                        lesCouches.Add(uneCouche);
                        nbNeuronesCoucheAvant = nbNeuronesMax;
                        nbNeuronesMax = nbNeuronesMax / 2;
                    }


                }
            }

         

            //creation de la derniere couche et des derniers neurones
            List<Neurone> derniereCouche = new List<Neurone>();
            for (int i = 0; i < nbNeuronesSortant; i++)
            {
                derniereCouche.Add(new Neurone(nbNeuronesCoucheAvant));
            }
            lesCouches.Add(derniereCouche);


            //affichage concernant les informations des neurones
            Console.WriteLine("nombre de couches " + lesCouches.Count().ToString());
            foreach (List<Neurone> l in lesCouches)
            {
                Console.WriteLine("couche avec " + l.Count().ToString());
            }

            //transition pour l'entrainement
            Console.Write("Appuyer sur une touche pour continuer sur l'entrainement du réseau.  ");
            Console.ReadKey();
            Console.Clear();

            //prise des paramètres pour l'entrainement 
            int nbLignesTotalFichier = 0;
            reader = new StreamReader(File.OpenRead(documentChoisi));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var valeurs = line.Split(',');
                nbLignesTotalFichier++;

            }
            Console.WriteLine("il y a " + nbLignesTotalFichier.ToString() + " lignes dans le fichier.");

            int nbLigneEntrainement = 0;

            bool saisieLigne = false;
            while (saisieLigne == false)
            {
                Console.Write("Jusqu'à quelle ligne voulez-vous aller pour entrainer le réseau ? ");
                temp = Console.ReadLine();
                saisieLigne = int.TryParse(temp, out nbLigneEntrainement);
                if (saisieLigne == false)
                {
                    Console.WriteLine("Erreur !!! Vous devez saisir un nombre.");
                }
                else
                {
                    if (nbLigneEntrainement > nbLignesTotalFichier)
                    {
                        Console.WriteLine("Erreur !!! Le nombre selectionné ne peut pas être supérieur au nombre de lignes dans le fichier.");
                        saisieLigne = false;
                    }
                    if (nbLigneEntrainement == 1)
                    {
                        Console.WriteLine("Erreur !!! Le nombre selectionné ne peut pas être égale à 1.");
                        saisieLigne = false;

                    }
                    if (nbLigneEntrainement <= 0)
                    {
                        Console.WriteLine("Erreur !!! Le nombre selectionné ne peut pas être nul ou négatif.");
                        saisieLigne = false;
                    }
                }

            }

            //debut de l'entrainement
            double pourcentageErreur = 1;
            double cptLigne = 0;
            while (pourcentageErreur > 0.1)
            {
                Console.WriteLine("début de l'entrainement jusqu'à la ligne " + nbLigneEntrainement + ".");
                double cptErreurs = 0;
                //Console.WriteLine(lesCouches[0].Count());

                cptLigne = 0;
                // on recreer le reader pour le remettre au début
                reader = new StreamReader(File.OpenRead(documentChoisi));

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var valeurs = line.Split(',');
                    cptLigne++;
                    bool celluleAcalculee;
                    List<double> donnees = new List<double>();
                    List<double> donneesAttendues = new List<double>();
                    double doubleTemp;
                    if (cptLigne <= nbLigneEntrainement && cptLigne != 1)
                    {
                        //Début de l'entrainement pour la ligne
                        Console.WriteLine("Début de l'entrainement pour la ligne " + cptLigne);
                        if (donnees.Count() != 0)
                        {
                            donnees.Clear();

                        }
                        // on parcourt les cellules d'une ligne
                        for (int i = 0; i < valeurs.Count(); i++)
                        {
                            celluleAcalculee = true;
                            foreach (int j in lesColonnesCalcules)
                            {
                                //on ne prend pas les cellules qui sont calculées
                                if (i == j)
                                {
                                    celluleAcalculee = false;
                                    bool resChaineAttendue = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                                    if (resChaineAttendue == true)
                                    {
                                        donneesAttendues.Add(doubleTemp / valeurMaxNormalisation[i]);
                                    }
                                    else
                                    {
                                        int cptValeurChaine = 0;
                                        foreach(string laChaineDeLaColonne in valeurChaineNormalise[i])
                                        {
                                            if (laChaineDeLaColonne == valeurs[i])
                                            {
                                                donneesAttendues.Add(cptValeurChaine / valeurMaxNormalisation[i]);
                                            }
                                            cptValeurChaine++;
                                        }
                                    }

                                }
                            }
                            if (celluleAcalculee == true)
                            {
                                bool resChaineCalculee = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                                if (resChaineCalculee == true)
                                {
                                    donnees.Add(doubleTemp / valeurMaxNormalisation[i]);
                                }
                                else
                                {
                                    int cptValeurChaine = 0;
                                    foreach (string laChaineDeLaColonne in valeurChaineNormalise[i])
                                    {
                                        if (laChaineDeLaColonne == valeurs[i])
                                        {
                                            donnees.Add(cptValeurChaine / valeurMaxNormalisation[i]);
                                        }
                                        cptValeurChaine++;
                                    }
                                }
                            }
                        }

                        //calcul des sorties de la première couche
                        for (int d = 0; d < donnees.Count(); d++)
                        {
                            List<double> entreePremiereCouche = new List<double>();
                            entreePremiereCouche.Add(donnees[d]);
                            lesCouches[0][d].calculSortiePremiereCouche(entreePremiereCouche);
                        }

                        //calcul des sorties pour les couches suivantes
                        for (int c = 1; c < lesCouches.Count(); c++)
                        {
                            //on recupere les sorties de la couche precedente
                            List<double> sortiesCouchePrecedente = new List<double>();
                            foreach (Neurone n in lesCouches[c - 1])
                            {
                                sortiesCouchePrecedente.Add(n.getSortie());
                            }


                            //calcul des sortie de la couche parcourue actuellement
                            foreach (Neurone n in lesCouches[c])
                            {
                                n.calculeSortie(sortiesCouchePrecedente);
                            }

                        }

                        //calcul de l'erreur quadratique (calcul de l'erreur de la derniere couche)
                        int cptNeurone = 0;
                        double sommeErreurs = 0;
                        //affichage des sorties 
                        foreach (Neurone n in lesCouches[lesCouches.Count() - 1])
                        {

                            Console.WriteLine("sortie : " + n.getSortie());
                            Console.WriteLine("sortie attendue : " + donneesAttendues[cptNeurone]);
                            sommeErreurs = sommeErreurs + (donneesAttendues[cptNeurone] - n.getSortie()) * (donneesAttendues[cptNeurone] - n.getSortie());

                            //calcul de l'erreur d'un neurone de la derniere couche
                            n.setErreur(donneesAttendues[cptNeurone] - n.getSortie());

                            cptNeurone++;


                        }
                        sommeErreurs = 0.5 * sommeErreurs;
                        Console.WriteLine("l'erreur quadratique est égale à : " + sommeErreurs);

                        if (sommeErreurs > 0.0035)
                        {
                            cptErreurs++;
                            //modification des poids de la derniere couche
                            int cptNeuroneDerniereCouche = 0;
                            foreach (Neurone n in lesCouches[lesCouches.Count() - 1])
                            {
                                int cptNeuroneAvantDerniereCouche = 0;
                                foreach (Neurone neuroneCoucheDavant in lesCouches[lesCouches.Count() - 2])
                                {
                                    n.setPoids(0.2 * (donneesAttendues[cptNeuroneDerniereCouche] - n.getSortie()) * 4 * n.getSortie() * (1 - n.getSortie()) * neuroneCoucheDavant.getSortie(), cptNeuroneAvantDerniereCouche);
                                    cptNeuroneAvantDerniereCouche++;
                                }
                                cptNeuroneDerniereCouche++;
                            }


                            //calcul de l'erreur  des neurones de toutes les couches sauf la dernirère et la premiere
                            for (int i = lesCouches.Count() - 2; i >= 1; i--)
                            {
                                cptNeurone = 0;
                                foreach (Neurone unNeurone in lesCouches[i])
                                {
                                    double somme = 0;
                                    //calcul des erreurs, on recupere les erreurs de la couche d'après
                                    foreach (Neurone unNeuroneCouchePLus in lesCouches[i + 1])
                                    {
                                        somme = somme + (unNeuroneCouchePLus.getErreur() * unNeuroneCouchePLus.getUnPoids(cptNeurone));
                                    }
                                    somme = (4 * unNeurone.getSortie() * (1 - unNeurone.getSortie())) * somme;
                                    unNeurone.setErreur(somme);

                                    //calcul des poids
                                    int cptNeuroneCoucheMoins = 0;
                                    foreach (Neurone unNeuroneCoucheMoins in lesCouches[i - 1])
                                    {

                                        unNeurone.setPoids(0.2 * unNeurone.getErreur() * unNeuroneCoucheMoins.getSortie(), cptNeuroneCoucheMoins);
                                        cptNeuroneCoucheMoins++;
                                    }
                                    cptNeurone++;
                                }
                            }
                        }

                    }
                }
                pourcentageErreur = cptErreurs / (nbLigneEntrainement - 1);
                Console.WriteLine("nombre d'erreurs : " + cptErreurs + " pour " + (nbLigneEntrainement - 1) + " soit " + pourcentageErreur);
                //Console.ReadKey();

            }

            Console.Write("Fin de l'entrainement pour le réseau appuyer pour continuer. ");
            Console.ReadKey();
            Console.Clear();


            Console.WriteLine("succès");

            if (nbLignesTotalFichier == nbLigneEntrainement)
            {
                Console.WriteLine("le fichier a été entierement parcouru pour l'entrainement. Pour la validation, le fichier va être parcourue dans son intégralité. ");
                Console.Write("Appuyer sur une touche pour commencer la validation.");
                Console.ReadKey();
                //on reparcourt le fichier
                cptLigne = 0;
                double cptErreurs = 0;
                // on recreer le reader pour le remettre au début
                reader = new StreamReader(File.OpenRead(documentChoisi));

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var valeurs = line.Split(',');
                    cptLigne++;
                    bool celluleAcalculee;
                    List<double> donnees = new List<double>();
                    List<double> donneesAttendues = new List<double>();
                    double doubleTemp;

                    if (cptLigne > 1)
                    {
                        //Début du calcul pour la ligne
                        Console.WriteLine("Début de l'entrainement pour la ligne " + cptLigne);
                        if (donnees.Count() != 0)
                        {
                            donnees.Clear();

                        }
                        // on parcourt les cellules d'une ligne
                        for (int i = 0; i < valeurs.Count(); i++)
                        {
                            celluleAcalculee = true;
                            foreach (int j in lesColonnesCalcules)
                            {
                                //on ne prend pas les cellules qui sont calculées
                                if (i == j)
                                {
                                    celluleAcalculee = false;
                                    bool resChaineAttendue = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                                    if (resChaineAttendue == true)
                                    {
                                        donneesAttendues.Add(doubleTemp / valeurMaxNormalisation[i]);
                                    }
                                    else
                                    {
                                        int cptValeurChaine = 0;
                                        foreach (string laChaineDeLaColonne in valeurChaineNormalise[i])
                                        {
                                            if (laChaineDeLaColonne == valeurs[i])
                                            {
                                                donneesAttendues.Add(cptValeurChaine / valeurMaxNormalisation[i]);
                                            }
                                            cptValeurChaine++;
                                        }
                                    }

                                }
                            }
                            if (celluleAcalculee == true)
                            {
                                bool resChaineCalculee = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                                if (resChaineCalculee == true)
                                {
                                    donnees.Add(doubleTemp / valeurMaxNormalisation[i]);
                                }
                                else
                                {
                                    int cptValeurChaine = 0;
                                    foreach (string laChaineDeLaColonne in valeurChaineNormalise[i])
                                    {
                                        if (laChaineDeLaColonne == valeurs[i])
                                        {
                                            donnees.Add(cptValeurChaine / valeurMaxNormalisation[i]);
                                        }
                                        cptValeurChaine++;
                                    }
                                }
                            }
                        }

                        //calcul des sorties de la première couche
                        for (int d = 0; d < donnees.Count(); d++)
                        {
                            List<double> entreePremiereCouche = new List<double>();
                            entreePremiereCouche.Add(donnees[d]);
                            lesCouches[0][d].calculSortiePremiereCouche(entreePremiereCouche);
                        }

                        //calcul des sorties pour les couches suivantes
                        for (int c = 1; c < lesCouches.Count(); c++)
                        {
                            //on recupere les sorties de la couche precedente
                            List<double> sortiesCouchePrecedente = new List<double>();
                            foreach (Neurone n in lesCouches[c - 1])
                            {
                                sortiesCouchePrecedente.Add(n.getSortie());
                            }


                            //calcul des sortie de la couche parcourue actuellement
                            foreach (Neurone n in lesCouches[c])
                            {
                                n.calculeSortie(sortiesCouchePrecedente);
                            }

                        }

                        //calcul de l'erreur quadratique (calcul de l'erreur de la derniere couche)
                        int cptNeurone = 0;
                        double sommeErreurs = 0;
                        //affichage des sorties 
                        foreach (Neurone n in lesCouches[lesCouches.Count() - 1])
                        {

                            Console.WriteLine("sortie : " + n.getSortie());
                            Console.WriteLine("sortie attendue : " + donneesAttendues[cptNeurone]);
                            sommeErreurs = sommeErreurs + (donneesAttendues[cptNeurone] - n.getSortie()) * (donneesAttendues[cptNeurone] - n.getSortie());

                            //calcul de l'erreur d'un neurone de la derniere couche
                            n.setErreur(donneesAttendues[cptNeurone] - n.getSortie());

                            cptNeurone++;


                        }
                        sommeErreurs = 0.5 * sommeErreurs;
                        Console.WriteLine("l'erreur quadratique est égale à : " + sommeErreurs);
                        if (sommeErreurs > 0.0035)
                        {
                            cptErreurs++;
                        }
                    }
                }
                pourcentageErreur = cptErreurs / (cptLigne - 1);
                Console.WriteLine("nombre d'erreurs : " + cptErreurs + " pour " + (cptLigne - 1) + " soit " + pourcentageErreur);
            }
            else
            {
                Console.Write("Appuyer sur une touche pour commencer la validation.");
                Console.ReadKey();
                //on continue le parcours le fichier
                cptLigne = 0;
                double cptErreurs = 0;
                // on recreer le reader pour le remettre au début
                reader = new StreamReader(File.OpenRead(documentChoisi));
                int nbLigneCalulees = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var valeurs = line.Split(',');
                    cptLigne++;
                    bool celluleAcalculee;
                    List<double> donnees = new List<double>();
                    List<double> donneesAttendues = new List<double>();
                    double doubleTemp;

                    if (cptLigne >=nbLigneEntrainement)
                    {
                        //Début du calcul pour la ligne
                        nbLigneCalulees++;
                        Console.WriteLine("Début de l'entrainement pour la ligne " + cptLigne);
                        if (donnees.Count() != 0)
                        {
                            donnees.Clear();

                        }
                        // on parcourt les cellules d'une ligne
                        for (int i = 0; i < valeurs.Count(); i++)
                        {
                            celluleAcalculee = true;
                            foreach (int j in lesColonnesCalcules)
                            {
                                //on ne prend pas les cellules qui sont calculées
                                if (i == j)
                                {
                                    celluleAcalculee = false;
                                    bool resChaineAttendue = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                                    if (resChaineAttendue == true)
                                    {
                                        donneesAttendues.Add(doubleTemp / valeurMaxNormalisation[i]);
                                    }
                                    else
                                    {
                                        int cptValeurChaine = 0;
                                        foreach (string laChaineDeLaColonne in valeurChaineNormalise[i])
                                        {
                                            if (laChaineDeLaColonne == valeurs[i])
                                            {
                                                donneesAttendues.Add(cptValeurChaine / valeurMaxNormalisation[i]);
                                            }
                                            cptValeurChaine++;
                                        }
                                    }

                                }
                            }
                            if (celluleAcalculee == true)
                            {
                                bool resChaineCalculee = double.TryParse(valeurs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out doubleTemp);
                                if (resChaineCalculee == true)
                                {
                                    donnees.Add(doubleTemp / valeurMaxNormalisation[i]);
                                }
                                else
                                {
                                    int cptValeurChaine = 0;
                                    foreach (string laChaineDeLaColonne in valeurChaineNormalise[i])
                                    {
                                        if (laChaineDeLaColonne == valeurs[i])
                                        {
                                            donnees.Add(cptValeurChaine / valeurMaxNormalisation[i]);
                                        }
                                        cptValeurChaine++;
                                    }
                                }
                            }
                        }

                        //calcul des sorties de la première couche
                        for (int d = 0; d < donnees.Count(); d++)
                        {
                            List<double> entreePremiereCouche = new List<double>();
                            entreePremiereCouche.Add(donnees[d]);
                            lesCouches[0][d].calculSortiePremiereCouche(entreePremiereCouche);
                        }

                        //calcul des sorties pour les couches suivantes
                        for (int c = 1; c < lesCouches.Count(); c++)
                        {
                            //on recupere les sorties de la couche precedente
                            List<double> sortiesCouchePrecedente = new List<double>();
                            foreach (Neurone n in lesCouches[c - 1])
                            {
                                sortiesCouchePrecedente.Add(n.getSortie());
                            }


                            //calcul des sortie de la couche parcourue actuellement
                            foreach (Neurone n in lesCouches[c])
                            {
                                n.calculeSortie(sortiesCouchePrecedente);
                            }

                        }

                        //calcul de l'erreur quadratique (calcul de l'erreur de la derniere couche)
                        int cptNeurone = 0;
                        double sommeErreurs = 0;
                        //affichage des sorties 
                        foreach (Neurone n in lesCouches[lesCouches.Count() - 1])
                        {

                            Console.WriteLine("sortie : " + n.getSortie());
                            Console.WriteLine("sortie attendue : " + donneesAttendues[cptNeurone]);
                            sommeErreurs = sommeErreurs + (donneesAttendues[cptNeurone] - n.getSortie()) * (donneesAttendues[cptNeurone] - n.getSortie());

                            //calcul de l'erreur d'un neurone de la derniere couche
                            n.setErreur(donneesAttendues[cptNeurone] - n.getSortie());

                            cptNeurone++;


                        }
                        sommeErreurs = 0.5 * sommeErreurs;
                        Console.WriteLine("l'erreur quadratique est égale à : " + sommeErreurs);
                        if (sommeErreurs > 0.0035)
                        {
                            cptErreurs++;
                        }
                    }
                }
                pourcentageErreur = cptErreurs / (nbLigneCalulees - 1);
                Console.WriteLine("nombre d'erreurs : " + cptErreurs + " pour " + (nbLigneCalulees - 1) + " soit " + pourcentageErreur);

            }
            Console.WriteLine("nombre total de lignes dans le fichier : " + nbLignesTotalFichier);
            Console.WriteLine("fichier parcouru jusqu'à la ligne  : " + nbLigneEntrainement);


            Console.ReadKey();
        }
    }
}

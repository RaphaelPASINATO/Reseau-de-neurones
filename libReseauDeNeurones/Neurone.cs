using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libReseauDeNeurones
{
    public class Neurone
    {
        double sortie;
        double erreur;
        List<double> lesPoids;
        Random aleatoire = new Random(DateTime.Now.Millisecond);

        public Neurone(int nbPoids)
        {
            lesPoids = new List<double>();
            for(int i = 0; i <nbPoids;i++)
            {
                double nbRng = aleatoire.Next(-20, 21);
                nbRng = nbRng / 100;
                lesPoids.Add(nbRng);
            }
        }

        public Neurone(int nbPoids, int poids)
        {
            lesPoids = new List<double>();
            for (int i = 0; i < nbPoids; i++)
            {
                lesPoids.Add(1);
            }
        }


        public void calculeSortie (List<double> lesEntrees)
        {
            double somme = 0;
            for (int i =0; i < lesEntrees.Count();i++)
            {
                somme = somme + lesEntrees[i] * lesPoids[i];
            }
            sortie = 1 / (1 + Math.Exp(-4 * somme));
        }


        public void calculSortiePremiereCouche(List<double> lesEntrees)
        {
            double somme = 0;
            for (int i = 0; i < lesEntrees.Count(); i++)
            {
                somme = somme + lesEntrees[i] * lesPoids[i];

            }
            sortie = somme;
            //Console.WriteLine("sortie : " + sortie);

        }

        public double getSortie()
        {
            return sortie;
        }

        public void setErreur(double nouvelleErreur)
        {
            erreur = nouvelleErreur;
        }

        public double getUnPoids(int idPoids)
        {
            return lesPoids[idPoids];
        }

        public void setPoids(double nouveauPoids, int id)
        {
            lesPoids[id] = lesPoids[id] + nouveauPoids;
        }

        public double getErreur()
        {
            return erreur;
        }
    }
    
}

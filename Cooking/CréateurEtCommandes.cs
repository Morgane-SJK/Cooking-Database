using System;
using System.Collections.Generic;
using System.Text;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    class CréateurEtCommandes
    {
        private string nomCréateur;
        private int nbTotalCommande;


        public string NomCréateur { get => nomCréateur; set => nomCréateur = value; }
        public int NbTotalCommande { get => nbTotalCommande; set => nbTotalCommande = value; }


        public CréateurEtCommandes(string NomCréateur, int NbTotalCommande)
        {
            this.nbTotalCommande = NbTotalCommande;
            this.nomCréateur = NomCréateur;
        }
    }
}

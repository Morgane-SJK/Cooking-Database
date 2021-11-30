using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class ListeIngredients
    {
        private string idRecette;
        private string idProduit;
        private int quantite;
        private string nomProduit;
        private string unite;

        public string IdRecette { get => idRecette; set => idRecette = value; }
        public string IdProduit { get => idProduit; set => idProduit = value; }
        public int Quantite { get => quantite; set => quantite = value; }
        public string NomProduit { get => nomProduit; set => nomProduit = value; }
        public string Unite { get => unite; set => unite = value; }

        public ListeIngredients(string IdRecette, string IdProduit, int Quantite, string NomProduit, string Unite)
        {
            this.idRecette = IdRecette;
            this.idProduit = IdProduit;
            this.quantite = Quantite;
            this.nomProduit = NomProduit;
            this.unite = Unite;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class Produit 
    {
        private string idProduit;
        private string nomProduit;
        private string categorie;
        private string unite;
        private int stockActuel;
        private int stockMini;
        private int stockMaxi;
        private string idFournisseur;

        public string IdProduit { get => idProduit; set => idProduit = value; }
        public string NomProduit { get => nomProduit; set => nomProduit = value; }
        public string Categorie { get => categorie; set => categorie = value; }
        public string Unite { get => unite; set => unite = value; }
        public int StockActuel { get => stockActuel; set => stockActuel = value; }
        public int StockMini { get => stockMini; set => stockMini = value; }
        public int StockMaxi { get => stockMaxi; set => stockMaxi = value; }
        public string IdFournisseur { get => idFournisseur; set => idFournisseur = value; }

        public Produit(string IdProduit, string NomProduit, string Categorie, string Unite, int StockActuel, int StockMini, int StockMaxi, string IdFournisseur)
        {
            this.idProduit = IdProduit;
            this.nomProduit = NomProduit;
            this.categorie = Categorie;
            this.unite = Unite;
            this.stockActuel = StockActuel;
            this.stockMini = StockMini;
            this.stockMaxi = StockMaxi;
            this.idFournisseur = IdFournisseur;
        }
    }
}

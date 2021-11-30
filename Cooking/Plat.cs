using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class Plat 
    {
        private string idPlat;
        private string idRecette;
        private int quantitePlat;
        private string idCdr;
        private string nomRecettePlat;
        private int prix;

        public string IdPlat { get => idPlat; set => idPlat = value; }
        public string IdRecette { get => idRecette; set => idRecette = value; }
        public int QuantitePlat { get => quantitePlat; set => quantitePlat = value; }
        public string IdCdr { get => idCdr; set => idCdr = value; }
        public string NomRecettePlat { get => nomRecettePlat; set => nomRecettePlat = value; }
        public int Prix { get => prix; set => prix = value; }

        public Plat (string IdPlat, string IdRecette, int QuantitePlat, string IdCdr, string NomRecettePlat, int Prix)
        {
            this.idPlat = IdPlat;
            this.idRecette = IdRecette;
            this.quantitePlat = QuantitePlat;
            this.idCdr = IdCdr;
            this.nomRecettePlat = NomRecettePlat;
            this.prix = Prix;
        }
    }
}

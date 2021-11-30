using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class Recette 
    {
        private string idRecette;
        private string nomRecette;
        private string type;
        private string description;
        private int prixDeVente;
        private string idCdr;
        private int compteur;
        private int remunerationCdr;

        public string IdRecette { get => idRecette; set => idRecette = value; }
        public string NomRecette { get => nomRecette; set => nomRecette = value; }
        public string Type { get => type; set => type = value; }
        public string Description { get => description; set => description = value; }
        public int PrixDeVente { get => prixDeVente; set => prixDeVente = value; }
        public string IdCdr { get => idCdr; set => idCdr = value; }
        public int Compteur { get => compteur; set => compteur = value; }
        public int RemunerationCdr { get => remunerationCdr; set => remunerationCdr = value; }

        public Recette(string IdRecette, string NomRecette, string Type, string Description, int PrixDeVente, string IdCdr, int Compteur, int RemunerationCdr)
        {
            this.idRecette = IdRecette;
            this.nomRecette = NomRecette;
            this.type = Type;
            this.description = Description;
            this.prixDeVente = PrixDeVente;
            this.idCdr = IdCdr;
            this.compteur = Compteur;
            this.remunerationCdr = RemunerationCdr;
        }
    }
}

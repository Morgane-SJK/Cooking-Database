using System;
using System.Collections.Generic;
using System.Text;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class Createur 
    {
        private string idCdr;
        private int benefice;
        private string idClient;

        public string IdCdr { get => idCdr; set => idCdr = value; }
        public int Benefice { get => benefice; set => benefice = value; }
        public string IdClient { get => idClient; set => idClient = value; }

        public Createur(string IdCdr,int Benefice, string IdClient, int NbTotalCommande)
        {
            this.idCdr = IdCdr;
            this.benefice = Benefice;
            this.idClient = IdClient;

        }
    }
}

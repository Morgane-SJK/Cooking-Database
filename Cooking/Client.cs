using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class Client
    {
        private string idClient;
        private string nomClient;
        private string numTelephoneC;

        public string IdClient { get => idClient; set => idClient = value; }
        public string NomClient { get => nomClient; set => nomClient = value; }
        public string NumTelephoneC { get => numTelephoneC; set => numTelephoneC = value; }

        public Client(string IdClient, string NomClient, string NumTelephoneC)
        {
            this.idClient = IdClient;
            this.nomClient = NomClient;
            this.numTelephoneC = NumTelephoneC;

        }
    }
}

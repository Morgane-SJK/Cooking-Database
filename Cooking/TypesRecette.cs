using System;
using System.Collections.Generic;
using System.Text;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public class TypesRecette
    {
        private string typeRecette;


        public string TypeRecette { get => typeRecette; set => TypeRecette = value; }

        public TypesRecette(string TypeRecette)
        {
            this.typeRecette = TypeRecette;
        }
    }
}

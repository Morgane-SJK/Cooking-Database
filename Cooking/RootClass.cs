using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    public static class RootClass
    {
        public static MySqlConnection OuvertureConnexion()
        {
            string connexionString = "SERVER = localhost; PORT = 3306; DATABASE = cooking; UID = root; PASSWORD = root;";
            MySqlConnection connection = new MySqlConnection(connexionString);
            connection.Open();
            return connection;
        }

        public static MySqlDataReader ExecutionRequeteSELECT(MySqlConnection connection, string commande)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            //connection.Close();
            return reader;
        }

        public static void ExecutionRequeteINSERT_UPDATE_DELETE(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static List<Produit> ExecutionRequetePRODUIT(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = ExecutionRequeteSELECT(connection, commande);
            List<Produit> ListeProduits = new List<Produit>();


            while (reader.Read())
            {
                string[] TableauProduits = new string[8];
                //string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString;
                    valueAsString = reader.GetValue(i).ToString();
                    //currentRowAsString += valueAsString + ", ";
                    TableauProduits[i] = valueAsString;
                }
                Produit produit = new Produit(TableauProduits[0], TableauProduits[1], TableauProduits[2], TableauProduits[3], int.Parse(TableauProduits[4]), int.Parse(TableauProduits[5]), int.Parse(TableauProduits[6]), TableauProduits[7]);
                ListeProduits.Add(produit);
            }
            connection.Close();
            return ListeProduits;
        }

        public static List<ListeIngredients> ExecutionRequeteLISTEINGREDIENT(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = ExecutionRequeteSELECT(connection, commande);
            List<ListeIngredients> ListeIngredients = new List<ListeIngredients>();


            while (reader.Read())
            {
                string[] TableauIngredients = new string[3];
                //string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString;
                    valueAsString = reader.GetValue(i).ToString();
                    //currentRowAsString += valueAsString + ", ";
                    TableauIngredients[i] = valueAsString;
                }
                ListeIngredients ingredient = new ListeIngredients(TableauIngredients[0], TableauIngredients[1], int.Parse(TableauIngredients[2]),"","");
                ListeIngredients.Add(ingredient);
            }
            connection.Close();
            return ListeIngredients;
        }

        public static List<Recette> ExecutionRequeteRECETTE(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = RootClass.ExecutionRequeteSELECT(connection, commande);
            List<Recette> ListeRecettes = new List<Recette>();

            while (reader.Read())
            {
                string[] TableauRecette = new string[8];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    TableauRecette[i] = valueAsString;
                }
                Recette recette = new Recette(TableauRecette[0], TableauRecette[1], TableauRecette[2], TableauRecette[3], int.Parse(TableauRecette[4]), TableauRecette[5], int.Parse(TableauRecette[6]), int.Parse(TableauRecette[7]));
                ListeRecettes.Add(recette);
            }
            reader.Close();
            connection.Close();

            return ListeRecettes;
        }

        public static string GestionApostrophes(string EntréeTextBox)
        {
            string TexteEpurée = "";
            char apostrophe = (char)39;
            char backSlash = (char)92;
            for (int i = 0; i < EntréeTextBox.Length; i++)
            {
                if (EntréeTextBox[i] == apostrophe)
                {
                    TexteEpurée += backSlash.ToString() + EntréeTextBox[i];
                }
                else
                {
                    TexteEpurée += EntréeTextBox[i];
                }

            }

            return TexteEpurée;
        }

        public static string GénérationAutomatiqueNouvelID(string commande, string premièreLettre)
        {
            string nouvelID = "";
            int index = premièreLettre.Length;
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = ExecutionRequeteSELECT(connection, commande);
            reader.Read();

            try
            {
                string idPrecedent = reader.GetString(0);
                for (int i = index; i < idPrecedent.Length; i++)
                {
                    nouvelID = nouvelID + idPrecedent[i];
                }
            }
            catch
            {
                nouvelID = "0";
            }
            reader.Close();

            nouvelID = premièreLettre + (Convert.ToInt32(nouvelID) + 1).ToString();
            connection.Close();
            return nouvelID;
        }

        public static string ExecutionRequeteString(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = ExecutionRequeteSELECT(connection, commande);
            reader.Read();
            string stringARetourner = reader.GetString(0);
            reader.Close();
            connection.Close();
            return stringARetourner;
        }

        public static List<string> ExecutionRequeteListString(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = RootClass.ExecutionRequeteSELECT(connection, commande);
            List<string> ListeClient = new List<string>();

            while (reader.Read())
            {
                string[] TableauClient = new string[1];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    TableauClient[i] = valueAsString;
                }
                ListeClient.Add(TableauClient[0]);
            }
            reader.Close();
            connection.Close();

            return ListeClient;
        }

        public static Recette ExecutionRequeteRECETTESimple(string commande)
        {
            MySqlConnection connection = OuvertureConnexion();
            MySqlDataReader reader = RootClass.ExecutionRequeteSELECT(connection, commande);
            reader.Read();
            string[] TableauRecette = new string[8];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string valueAsString = reader.GetValue(i).ToString();
                TableauRecette[i] = valueAsString;
            }
            Recette recette = new Recette(TableauRecette[0], TableauRecette[1], TableauRecette[2], TableauRecette[3], int.Parse(TableauRecette[4]), TableauRecette[5], int.Parse(TableauRecette[6]), int.Parse(TableauRecette[7]));
            
            reader.Close();
            connection.Close();

            return recette;
        }




    }
}

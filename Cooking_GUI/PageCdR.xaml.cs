using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    /// <summary>
    /// Logique d'interaction pour PageCdR.xaml
    /// </summary>
    public partial class PageCdR : Page
    {
        public PageCdR()
        {
            InitializeComponent();
            List<Recette> RecettesPersos = ExecutionRequeteRECETTEJoin("SELECT nomRecette, prixDeVente, compteur, remunerationCdr FROM recette,createur WHERE recette.idCdr= createur.idCdr and createur.idClient='" + IdUtilisateurActif.Id + "';");
            gridRecettePerso.ItemsSource = RecettesPersos;
            string nom = RootClass.ExecutionRequeteString("SELECT nomClient FROM client WHERE idClient = '" + IdUtilisateurActif.Id + "';");
            TextBlock2.Text = nom;
            int benefice = int.Parse(RootClass.ExecutionRequeteString("SELECT benefice FROM createur WHERE idClient = '" + IdUtilisateurActif.Id + "';"));
            PrixTotal.Text = benefice.ToString();
        }

        private void clickPageClient(object sender, RoutedEventArgs e)
        {
            PageClient fenetreClient = new PageClient();
            fenetreClient.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreClient);
        }

        private void clickDeconnexion(object sender, RoutedEventArgs e)
        {
            PageConnexion fenetreAccueil = new PageConnexion();
            fenetreAccueil.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreAccueil);
        }

        private void clickNouvelleRecette(object sender, RoutedEventArgs e)
        {
            PageCreationRecette fenetreCreationRecette = new PageCreationRecette();
            fenetreCreationRecette.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreCreationRecette);
        }

        public List<Recette> ExecutionRequeteRECETTEJoin(string commande)
        {
            MySqlConnection connection = RootClass.OuvertureConnexion();
            MySqlDataReader reader = RootClass.ExecutionRequeteSELECT(connection, commande);
            List<Recette> ListeRecettesPersos = new List<Recette>();
            while (reader.Read())
            {
                string[] TableauRecettesPersos = new string[4];
                //string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    TableauRecettesPersos[i] = valueAsString;
                }
                Recette recette = new Recette("", TableauRecettesPersos[0], "", "", int.Parse(TableauRecettesPersos[1]), "", int.Parse(TableauRecettesPersos[2]), int.Parse(TableauRecettesPersos[3]));
                ListeRecettesPersos.Add(recette);
            }
            reader.Close();
            connection.Close();
            return ListeRecettesPersos;         
        }
    }
}

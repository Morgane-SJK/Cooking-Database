using System;
using System.Collections.Generic;
using System.Printing;
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
    /// Logique d'interaction pour PageCreationCompte.xaml
    /// </summary>
    public partial class PageCreationCompte : Page
    {
        public PageCreationCompte()
        {
            InitializeComponent();
        }

        private void clickValider(object sender, RoutedEventArgs e)
        {
            string nom = TextBox1.Text;
            string numero = TextBox2.Text;
            string nouvelID = RootClass.GénérationAutomatiqueNouvelID("SELECT idClient FROM client where length(idClient) IN (SELECT max(length(idClient)) from client) and idClient!='G1' ORDER by idClient desc limit 1;", "C");
            RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`client` (`idClient`,`nomClient`,`numTelephoneC`) VALUES ('" + nouvelID + "','" + nom + "','" + numero + "');");
            IdUtilisateurActif.Id = nouvelID;
            MessageBox.Show("Votre identifiant est  : "+nouvelID);
        }

        private void clickClient(object sender, RoutedEventArgs e)
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
    }

}

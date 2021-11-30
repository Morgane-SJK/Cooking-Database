using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    /// Logique d'interaction pour PageConnexion.xaml
    /// </summary>
    public partial class PageConnexion : Page
    {
        public PageConnexion()
        {
            InitializeComponent();
        }

        private void clickCreationCompte(object sender, RoutedEventArgs e)
        {
            PageCreationCompte fenetreCreationCompte = new PageCreationCompte();
            fenetreCreationCompte.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreCreationCompte);
        }

        private void clickVerification(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = RootClass.OuvertureConnexion();
            MySqlDataReader reader = RootClass.ExecutionRequeteSELECT(connection, "SELECT idClient, nomClient FROM client");

            string idClient="";
            string nomClient="";
            bool test = false;
            bool test2 = false;
            while (reader.Read())
            {
                string idClientTest = reader.GetString(0);
                string nomClientTest = reader.GetString(1);
                //si l'utilisateur a rentré des identifiants valide
                if (TextBox1.Text == nomClientTest && TextBox2.Text == idClientTest)
                {
                    test = true;
                    idClient = idClientTest;

                    //si l'utilisateur est un gestionnaire
                    if (idClient[0] == 'G')
                    {
                        test2 = true;
                    }
                    nomClient = nomClientTest;
                    break;
                }

            }
            reader.Close();
            connection.Close();

            if (test == true && test2 == false)
            {
                IdUtilisateurActif.Id = idClient;
                PageClient fenetreClient = new PageClient();
                fenetreClient.ShowsNavigationUI = false;
                this.NavigationService.Navigate(fenetreClient);
                
            }
            else if (test == true && test2 == true)
            {
                IdUtilisateurActif.Id = idClient;
                PageGestionnaire fenetreGestionnaire = new PageGestionnaire();
                fenetreGestionnaire.ShowsNavigationUI = false;
                this.NavigationService.Navigate(fenetreGestionnaire);
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect");
            }
        }

        private void clickModeDémo(object sender, RoutedEventArgs e)
        {
            PageDemo fenetreDémo = new PageDemo();
            fenetreDémo.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreDémo);
        }
    }
}

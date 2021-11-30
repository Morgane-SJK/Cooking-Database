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

namespace TD_F_SENEJKO_TCHIKLADZE
{
    /// <summary>
    /// Logique d'interaction pour PageDemo.xaml
    /// </summary>
    public partial class PageDemo : Page
    {
        public PageDemo()
        {
            List<string> idCreateur = RootClass.ExecutionRequeteListString("SELECT idCdr FROM recette GROUP BY idCdr order by sum(compteur);");
            List<string> SommeCompteur = RootClass.ExecutionRequeteListString("SELECT sum(compteur) FROM recette GROUP BY idCdr order by sum(compteur);");
            List<CréateurEtCommandes> listeCréateuretCommande = new List<CréateurEtCommandes>();
            for (int i = 0; i< SommeCompteur.Count; i++)
            {   
                if (int.Parse(SommeCompteur[i])!= 0) 
                {
                    string idClientCreateur = RootClass.ExecutionRequeteString("SELECT idClient FROM createur WHERE idCdr='" + idCreateur[i] + "';");
                    string nomClientcreateur = RootClass.ExecutionRequeteString("SELECT nomClient FROM client WHERE idClient = '" + idClientCreateur + "';");
                    CréateurEtCommandes UnCréateur = new CréateurEtCommandes(nomClientcreateur, int.Parse(SommeCompteur[i]));
                    listeCréateuretCommande.Add(UnCréateur);
                }

            }
            List<Produit> listeProduit = RootClass.ExecutionRequetePRODUIT("SELECT * FROM produit WHERE stockActuel <= 2*stockMini;");

            InitializeComponent();
            TextBlock1.Text = RootClass.ExecutionRequeteString("SELECT count(*) FROM client");
            TextBlock2.Text = RootClass.ExecutionRequeteString("SELECT count(*) FROM createur");
            TextBlock3.Text = RootClass.ExecutionRequeteString("SELECT count(*) FROM recette");
            gridCréateurCommande.ItemsSource = listeCréateuretCommande;
            gridProduit.ItemsSource = listeProduit;
        }

        private void clickAfficherRecettes(object sender, RoutedEventArgs e)
        {
            int index = gridProduit.SelectedIndex;
            Produit produito = (Produit)gridProduit.Items[index];
            List<string> idRecettes = RootClass.ExecutionRequeteListString("SELECT idRecette FROM listeIngredients WHERE idProduit = '" + produito.IdProduit + "';");
            List<Recette> NomRecettes = new List<Recette>();
            for (int i = 0; i< idRecettes.Count; i++)
            {
                NomRecettes.Add(RootClass.ExecutionRequeteRECETTESimple("SELECT * FROM recette WHERE idRecette = '"+idRecettes[i]+"';"));
            }
            
            gridProduitRecettes.ItemsSource = NomRecettes;
        }

        private void clickDeconnexion(object sender, RoutedEventArgs e)
        {
            PageConnexion fenetreAccueil = new PageConnexion();
            fenetreAccueil.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreAccueil);
        }
    }
}

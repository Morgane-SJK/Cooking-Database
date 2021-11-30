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
    /// Logique d'interaction pour PageCreationCdR.xaml
    /// </summary>
    public partial class PageCreationCdR : Page
    {
        public PageCreationCdR()
        { 
            List<Produit> listeProduit = RootClass.ExecutionRequetePRODUIT("SELECT * FROM produit;");
            List<TypesRecette> listeTypeRecette = new List<TypesRecette>();


            //informations de la combobox contenant les types de recette
            TypesRecette entrée = new TypesRecette("Entrée");
            TypesRecette platPrincipal = new TypesRecette("Plat Principal");
            TypesRecette fromage = new TypesRecette("Fromage");
            TypesRecette dessert = new TypesRecette("Dessert");
            TypesRecette accompagnement = new TypesRecette("Accompagnement");
            TypesRecette apéritif = new TypesRecette("Apéritif");
            TypesRecette autre = new TypesRecette("Autre");

            listeTypeRecette.Add(entrée);
            listeTypeRecette.Add(platPrincipal);
            listeTypeRecette.Add(fromage);
            listeTypeRecette.Add(dessert);
            listeTypeRecette.Add(accompagnement);
            listeTypeRecette.Add(apéritif);
            listeTypeRecette.Add(autre);

            InitializeComponent();
            ComboBox1.ItemsSource = listeProduit;
            ComboBox2.ItemsSource = listeTypeRecette;
        }

        private void clickAjouterIngredient(object sender, RoutedEventArgs e)
        {
            int index = ComboBox1.SelectedIndex;
            if (index >= 0)
            {
                List<Produit> listeProduits = RootClass.ExecutionRequetePRODUIT("SELECT * FROM produit;");
                int Quantité = 0;
                try { Quantité = int.Parse(TextBox4.Text); }
                catch
                {
                    MessageBox.Show("Veuillez indiquer une quantité valide.");
                }
                ListeIngredients nouvelIngredient = new ListeIngredients("", listeProduits[index].IdProduit, Quantité, listeProduits[index].NomProduit, listeProduits[index].Unite);
                bool ingredientDejaPresent = false;
                for (int i = 0; i < gridListeIngredient.Items.Count; i++)
                {
                    ListeIngredients ingrid = (ListeIngredients)gridListeIngredient.Items[i];
                    //si l'ingrédient est d"ja présent on augmente seulement sa quantité
                    if (ingrid.NomProduit == listeProduits[index].NomProduit)
                    {
                        ingredientDejaPresent = true;
                        ingrid.Quantite += Quantité;
                        gridListeIngredient.Items[i] = ingrid;
                        break;
                    }
                }
                if (!ingredientDejaPresent)
                {
                    gridListeIngredient.Items.Add(nouvelIngredient);
                }
                gridListeIngredient.Items.Refresh();
            }
        }

        private void clickValiderRecette(object sender, RoutedEventArgs e)
        {
            string nom = RootClass.GestionApostrophes(TextBox1.Text);
            string type = ComboBox2.Text;
            string description = RootClass.GestionApostrophes(TextBox3.Text);
            string prix = TextBox5.Text;
            bool test = true;
            try
            {

                int prixint = Convert.ToInt32(prix);

            }
            catch
            {
                test = false;
            }

            //si toutes données ont le bon format
            if (nom != null && type != null && description != null && test)
            {
                string nouvelIDR = RootClass.GénérationAutomatiqueNouvelID("SELECT idRecette FROM recette where length(idRecette) IN (SELECT max(length(idRecette)) from recette) ORDER by idRecette desc limit 1;", "R");
                string nouvelIDCDR = RootClass.GénérationAutomatiqueNouvelID("SELECT idCdr FROM createur where length(idCdr) IN (SELECT max(length(idCdr)) from createur) ORDER by idCdr desc limit 1;", "CdR");
                RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`createur` (`idCdr`,`idClient`) VALUES ('" + nouvelIDCDR + "','" + IdUtilisateurActif.Id + "');");
                RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`recette` (`idRecette`,`nomRecette`,`type`,`description`,`prixDeVente`,`idCdr`) VALUES ('" + nouvelIDR + "','" + nom + "','" + type + "','" + description + "','" + prix + "','" + nouvelIDCDR + "');");
                for (int i = 0; i < gridListeIngredient.Items.Count; i++)
                {
                    ListeIngredients ingredient = (ListeIngredients)gridListeIngredient.Items[i];

                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`listeIngredients` (`idRecette`,`idProduit`,`quantite`) VALUES ('" + nouvelIDR + "','" + ingredient.IdProduit + "','" + ingredient.Quantite + "');");
                }
                MessageBox.Show("Votre recette a bien été ajoutée à notre base de données ! Bienvenue dans l'équipe des Créateurs de Recettes ! ");
                PageCdR fenetreCdr = new PageCdR();
                fenetreCdr.ShowsNavigationUI = false;
                this.NavigationService.Navigate(fenetreCdr);
            }
            else if (test == false)
            {
                MessageBox.Show("Veuillez remplir les prix et quantité avec des nombres entiers.");
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
            }
        }

        private void UpdateUnite(object sender, SelectionChangedEventArgs e)
        {
            int index = ComboBox1.SelectedIndex;
            List<Produit> listeProduits = RootClass.ExecutionRequetePRODUIT("SELECT * FROM produit;");
            TextBlock1.Text = listeProduits[index].Unite;
        }

        private void clickSupprimerIngredient(object sender, RoutedEventArgs e)
        {
            int index = gridListeIngredient.SelectedIndex;
            if (index >= 0)
            {
                gridListeIngredient.Items.RemoveAt(index);
            }
        }

        private void clickAnnuler(object sender, RoutedEventArgs e)
        {
            PageClient fenetreClient = new PageClient();
            fenetreClient.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreClient);
        }

    }
}

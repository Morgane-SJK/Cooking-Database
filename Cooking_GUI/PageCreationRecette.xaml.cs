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
    /// Logique d'interaction pour PageCreationRecette.xaml
    /// </summary>
    public partial class PageCreationRecette : Page
    {
        public PageCreationRecette()
        {
            List<TypesRecette> listeTypeRecette = new List<TypesRecette>();

            //on rentre les éléments de la combobox contenant les types de recettes
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
            List<Produit> listeProduit = RootClass.ExecutionRequetePRODUIT("SELECT * FROM produit;");
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
            string type = RootClass.GestionApostrophes(ComboBox2.Text);
            string description = RootClass.GestionApostrophes(TextBox3.Text);
            string prix = RootClass.GestionApostrophes(TextBox5.Text);

            bool test = true;
            try
            {
                int prixint = Convert.ToInt32(prix);

            }
            catch
            {
                test = false;
            }


            if (nom != null && type != null && description != null && test)
            {

                string nouvelIDR = RootClass.GénérationAutomatiqueNouvelID("SELECT idRecette FROM recette where length(idRecette) IN (SELECT max(length(idRecette)) from recette) ORDER by idRecette desc limit 1;", "R");

                string idCdr = RootClass.ExecutionRequeteString("SELECT idCdr FROM createur WHERE createur.idClient = '" + IdUtilisateurActif.Id + "'; ");

                RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`recette` (`idRecette`,`nomRecette`,`type`,`description`,`prixDeVente`,`idCdr`) VALUES ('" + nouvelIDR + "','" + nom + "','" + type + "','" + description + "','" + prix + "','" + idCdr + "');");

                for (int i = 0; i < gridListeIngredient.Items.Count; i++)
                {
                    ListeIngredients ingredient = (ListeIngredients)gridListeIngredient.Items[i];
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`listeIngredients` (`idRecette`,`idProduit`,`quantite`) VALUES ('" + nouvelIDR + "','" + ingredient.IdProduit + "','" + ingredient.Quantite + "');");
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("UPDATE produit SET stockMini = (stockMini/2)+(3*"+ingredient.Quantite+") WHERE idProduit='"+ingredient.IdProduit+"';");
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("UPDATE produit SET stockMaxi = stockMaxi+(2*" + ingredient.Quantite + ") WHERE idProduit='" + ingredient.IdProduit + "';");
                }

                MessageBox.Show("Votre recette a bien été ajoutée à notre base de données ! ");
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

        private void clickEspaceCreateur(object sender, RoutedEventArgs e)
        {
            PageCdR fenetreCreateur = new PageCdR();
            fenetreCreateur.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreCreateur);
        }
    }
}

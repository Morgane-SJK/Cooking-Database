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
using System.Xml;
using MySql.Data.MySqlClient;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    /// <summary>
    /// Logique d'interaction pour PageClient.xaml
    /// </summary>
    public partial class PageClient : Page
    { 
        public PageClient()
        {
            InitializeComponent();
            gridRecette.ItemsSource = RootClass.ExecutionRequeteRECETTE("SELECT * FROM recette;");
            string nom = RootClass.ExecutionRequeteString("SELECT nomClient FROM client WHERE idClient = '"+ IdUtilisateurActif.Id +"';");
            TextBlock2.Text = nom;
        }
        
        private void clickPageCdR(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = RootClass.OuvertureConnexion();
            MySqlDataReader reader = RootClass.ExecutionRequeteSELECT(connection, "SELECT idClient FROM createur;");
            bool test = false;
            while (reader.Read())
            {
                string idClientTest = reader.GetString(0);
                //on verifie si le client n'est pas déja un créateur 
                if (idClientTest == IdUtilisateurActif.Id)
                {
                    test = true;
                    break;
                }
            }
            reader.Close();
            connection.Close();

            //On redirige l'utilisateur sur des pages différentes si jamais il est déja créateur ou non
            if (test)
            {
                PageCdR fenetreCdR = new PageCdR();
                fenetreCdR.ShowsNavigationUI = false;
                this.NavigationService.Navigate(fenetreCdR);
            }
            else
            {
                PageCreationCdR fenetreCreationCdR = new PageCreationCdR();
                fenetreCreationCdR.ShowsNavigationUI = false;
                this.NavigationService.Navigate(fenetreCreationCdR);
            }
        }

        private void clickDeconnexion(object sender, RoutedEventArgs e)
        {
            PageConnexion fenetreAccueil = new PageConnexion();
            fenetreAccueil.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreAccueil);
        }

        private void clickAjouterAuPanier(object sender, RoutedEventArgs e)
        {
            int index = gridRecette.SelectedIndex; 
            
            //s'il y a un index déja selectionné
            if (index >= 0)
            {
                int quantite = 0;
                bool quantiteAcceptable = true;
                try
                {
                    quantite = int.Parse(TextBox1.Text);
                }
                catch
                {
                    MessageBox.Show("Veuillez entrer une quantité valide pour votre plat.");
                }
                Recette recetto = (Recette)gridRecette.Items[index];
                int nombreDeProduits = int.Parse(RootClass.ExecutionRequeteString("  SELECT count(listeIngredients.idProduit) FROM recette, listeIngredients WHERE recette.idRecette=listeIngredients.idRecette and recette.idRecette='"+recetto.IdRecette+"';"));
                List<ListeIngredients> ListeIngredients = RootClass.ExecutionRequeteLISTEINGREDIENT("SELECT * FROM listeIngredients WHERE idRecette='"+recetto.IdRecette+"';");
                for (int i=0; i<nombreDeProduits; i++)
                {
                    int quantiteDejaPresente = 0;
                    for (int j = 0; j < gridCommande.Items.Count; j++)
                    {
                        Plat platibis = (Plat)gridCommande.Items[j];

                        //si le plat est déja présent on prend en compte la quantité ajoutée pour le calcul des stocks max utilisables
                        if (platibis.NomRecettePlat==recetto.NomRecette)
                        {
                            quantiteDejaPresente = platibis.QuantitePlat;
                        }
                    }
                    //Calcul de la quantité maximale nécessaire a la réalisation de ces recettes
                    int quantiteProduitNecessaire =ListeIngredients[i].Quantite*(quantite+quantiteDejaPresente);
                    int stockActuelProduit = int.Parse(RootClass.ExecutionRequeteString("SELECT stockActuel FROM produit WHERE idProduit='"+ ListeIngredients[i].IdProduit+"';"));
                    
                    //si on cherche a commander plus de quantités de ce produit qu'il n'y a de stock
                    if (quantiteProduitNecessaire> stockActuelProduit)
                    {
                        quantiteAcceptable = false;
                    }
                }
                if (quantiteAcceptable)
                {
                    bool recetteDejaPresente = false;
                    for (int i = 0; i < gridCommande.Items.Count; i++)
                    {
                        Plat plati = (Plat)gridCommande.Items[i];
                        //si le plat est déja présent on augmente juste sa quantité
                        if (plati.NomRecettePlat == recetto.NomRecette)
                        {
                            recetteDejaPresente = true;
                            plati.QuantitePlat += quantite;
                            plati.Prix += recetto.PrixDeVente * quantite;
                            gridCommande.Items[i] = plati;
                            break;
                        }
                    }
                    //si le plat n'est pas déja présent on le rajoute
                    if (!recetteDejaPresente)
                    {
                        Plat plato = new Plat("", recetto.IdRecette, quantite, recetto.IdCdr, recetto.NomRecette, recetto.PrixDeVente * quantite);
                        gridCommande.Items.Add(plato);
                    }
                    gridCommande.Items.Refresh();
                    int Somme = 0;
                    for (int i = 0; i < gridCommande.Items.Count; i++)
                    {
                        Plat platane = (Plat)gridCommande.Items[i];
                        Somme += platane.Prix;
                    }
                    PrixTotal.Text = Somme.ToString();
                }
                else
                {
                    MessageBox.Show("Nos stocks actuels ne permettent pas la réalisation de cette quantité. Veuillez entrer une quantité inférieure.");
                }

            }
        }

        private void clickPayer(object sender, RoutedEventArgs e)
        {
            bool achatPossible = true;
            bool createur = false;
            try
            {
                //on regarde si les créateur ont assez de cook pour pouvoir payer leurs commandes
                int benefice = int.Parse(RootClass.ExecutionRequeteString("SELECT benefice FROM createur WHERE idClient='"+ IdUtilisateurActif.Id+"';"));
                if (benefice < int.Parse(PrixTotal.Text))
                {
                    achatPossible = false;
                }
                createur = true;
            }
            catch
            {

            }
            if (achatPossible)
            {
                string nouvelIDCO = RootClass.GénérationAutomatiqueNouvelID("SELECT idCommande FROM commande where length(idCommande) IN (SELECT max(length(idCommande)) from commande) ORDER by idCommande desc limit 1;", "CO");

                for (int i = 0; i < gridCommande.Items.Count; i++)
                {
                    string nouvelIDP = RootClass.GénérationAutomatiqueNouvelID("SELECT idPlat FROM plat where length(idPlat) IN (SELECT max(length(idPlat)) from plat) ORDER by idPlat desc limit 1;", "PL");
                    Plat plato = (Plat)gridCommande.Items[i];
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`plat` (`idPlat`,`idRecette`,`quantitePlat`,`idCdr`) VALUES ('" + nouvelIDP + "','" + plato.IdRecette + "','" + plato.QuantitePlat + "','" + plato.IdCdr + "');");
                    Recette elRecet = new Recette("", "", "", "", 0, "", 0, 0);
                    for (int j = 0; j < gridRecette.Items.Count; j++)
                    {
                        elRecet = (Recette)gridRecette.Items[j];
                        if (elRecet.IdRecette == plato.IdRecette)
                        {
                            break;
                        }
                    }
                    int nouveauCompteur = elRecet.Compteur + plato.QuantitePlat;
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("UPDATE recette SET compteur = '" + nouveauCompteur + "'WHERE idRecette='" + plato.IdRecette + "';");
                    
                    //Update prix, remuneration et benefice
                    if (nouveauCompteur >= 10 && nouveauCompteur <= 50 && elRecet.Compteur < 10)
                    {
                        int nouveauPrix = elRecet.PrixDeVente + 2;
                        RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("  UPDATE recette SET prixDeVente = '" + nouveauPrix + "' WHERE idRecette='" + elRecet.IdRecette + "';");
                    }
                    else if (nouveauCompteur >= 50 && elRecet.Compteur < 50)
                    {
                        int nouveauPrix = elRecet.PrixDeVente + 5;
                        RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("  UPDATE recette SET prixDeVente = '" + nouveauPrix + "' WHERE idRecette='" + elRecet.IdRecette + "';");
                        if (elRecet.RemunerationCdr == 2)
                        {
                            elRecet.RemunerationCdr = 4;
                            RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("  UPDATE recette SET remunerationCdr = '4' WHERE idRecette='" + elRecet.IdRecette + "';");
                        }
                    }
                    int nouveauBenefice = int.Parse(RootClass.ExecutionRequeteString("SELECT benefice FROM createur WHERE idCdr = '" + elRecet.IdCdr + "'; ")) + (elRecet.RemunerationCdr * plato.QuantitePlat);
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("  UPDATE createur SET benefice = '" + nouveauBenefice + "' WHERE idCdr='" + elRecet.IdCdr + "';");

                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("INSERT INTO `cooking`.`commande` (`idCommande`,`idClient`,`idPlat`,`date`) VALUES ('" + nouvelIDCO + "','" + IdUtilisateurActif.Id + "','" + nouvelIDP + "','" + ConversionDateAmericaine(DateTime.Now) + "');");

                    //Mise à jour des stocks
                    int nombreDeProduits = int.Parse(RootClass.ExecutionRequeteString("  SELECT count(listeIngredients.idProduit) FROM recette, listeIngredients WHERE recette.idRecette=listeIngredients.idRecette and recette.idRecette='" + elRecet.IdRecette + "';"));
                    List<ListeIngredients> ListeIngredients = RootClass.ExecutionRequeteLISTEINGREDIENT("SELECT * FROM listeIngredients WHERE idRecette='" + elRecet.IdRecette + "';");
                    for (int j = 0; j < nombreDeProduits; j++)
                    {
                        int quantiteProduitNecessaire = ListeIngredients[j].Quantite * plato.QuantitePlat;
                        RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("UPDATE produit SET stockActuel=(stockActuel-" + quantiteProduitNecessaire + ") WHERE idProduit='" + ListeIngredients[j].IdProduit + "';");
                    }


                }
                if (createur)
                {
                    int prix = int.Parse(PrixTotal.Text);
                    RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("UPDATE createur SET benefice=(benefice-"+prix+") WHERE idClient='"+IdUtilisateurActif.Id+"';");
                }
                MessageBox.Show("Votre commande a bien été enregistrée ! Vous la recevrez d'ici peu. Merci de votre fidélité ! Cooking vous souhaite une bonne dégustation et une excellente journée ! ");
                gridCommande.Items.Clear();
                PrixTotal.Text = "0";
            }
            else
            {
                MessageBox.Show("Vous n'avez pas assez de Cook pour payer cette commande !");
            }
             
        }


        static string ConversionDateAmericaine(DateTime dateEuropéenne)
        {
            string dateAModifier = dateEuropéenne.ToString();
            string dateTransformée = "";
            string jour = dateAModifier[0].ToString() + dateAModifier[1].ToString();
            string mois = dateAModifier[3].ToString() + dateAModifier[4].ToString();
            string annee = dateAModifier[6].ToString() + dateAModifier[7].ToString() + dateAModifier[8].ToString() + dateAModifier[9].ToString();
            dateTransformée = annee + "/" + mois + "/" + jour + " ";
            for (int i = 11; i < 19; i++)
            {
                dateTransformée = dateTransformée + dateAModifier[i];
            }

            return dateTransformée;
        }
    }
}

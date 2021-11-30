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
using System.Xml;

namespace TD_F_SENEJKO_TCHIKLADZE
{
    /// <summary>
    /// Logique d'interaction pour PageGestionnaire.xaml
    /// </summary>
    public partial class PageGestionnaire : Page
    {
        public PageGestionnaire()
        {
            List<Recette> listeRecetteCreateur = RootClass.ExecutionRequeteRECETTE("SELECT * FROM recette;");
            InitializeComponent();
            for (int  i = 0; i< listeRecetteCreateur.Count; i++)
            {
                gridRecettesCreateurs.Items.Add(listeRecetteCreateur[i]);
            }
            TextBlock2.Text = RootClass.ExecutionRequeteString("SELECT nomClient FROM client WHERE idClient = '" + IdUtilisateurActif.Id + "';");

            string idCreateurDOR = RootClass.ExecutionRequeteString("SELECT idCdr FROM recette GROUP BY idCdr order by sum(compteur) DESC limit 1;");
            string totalCommandes = RootClass.ExecutionRequeteString("SELECT sum(compteur) FROM recette GROUP BY idCdr order by sum(compteur) DESC limit 1;");
            string idClientCreateurDOR = RootClass.ExecutionRequeteString("SELECT idClient FROM createur WHERE idCdr='" + idCreateurDOR + "';");
            TextBlock5.Text= RootClass.ExecutionRequeteString("SELECT nomClient FROM client WHERE idClient = '" + idClientCreateurDOR + "';");
            TextBlock6.Text =totalCommandes;

            List<Recette> listeMeilleuresRecettes = RootClass.ExecutionRequeteRECETTE("SELECT * FROM recette order by compteur desc limit 5;");
            TextBlock7.Text = listeMeilleuresRecettes[0].NomRecette;
            TextBlock8.Text = listeMeilleuresRecettes[1].NomRecette;
            TextBlock9.Text = listeMeilleuresRecettes[2].NomRecette;
            TextBlock10.Text = listeMeilleuresRecettes[3].NomRecette;
            TextBlock11.Text = listeMeilleuresRecettes[4].NomRecette;
        }

        private void clickSupprRecette(object sender, RoutedEventArgs e)
        {
            int index = gridRecettesCreateurs.SelectedIndex;
            if (index >= 0)
            {
                Recette recetteASuppr = (Recette)gridRecettesCreateurs.Items[index];
                RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("DELETE FROM  `cooking`.`recette` WHERE idRecette = '" + recetteASuppr.IdRecette + "';");
                gridRecettesCreateurs.Items.RemoveAt(index);
                gridRecettesCreateurs.Items.Refresh();
            }
        }

        private void clickSupprCreateur(object sender, RoutedEventArgs e)
        {
            int index = gridRecettesCreateurs.SelectedIndex;
            if (index >= 0)
            {
                Recette createurASuppr = (Recette)gridRecettesCreateurs.Items[index];
                RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("DELETE FROM  `cooking`.`createur` WHERE idCdR = '" + createurASuppr.IdCdr + "';");
                gridRecettesCreateurs.Items.RemoveAt(index);
                gridRecettesCreateurs.Items.Refresh();
            }
        }

        private void clickDeconnexion(object sender, RoutedEventArgs e)
        {
            PageConnexion fenetreAccueil = new PageConnexion();
            fenetreAccueil.ShowsNavigationUI = false;
            this.NavigationService.Navigate(fenetreAccueil);
        }

        private void clickMaj(object sender, RoutedEventArgs e)
        {
            RootClass.ExecutionRequeteINSERT_UPDATE_DELETE("UPDATE produit p SET p.stockMini = p.stockMini / 2, p.stockMaxi = p.stockMaxi / 2 WHERE p.idProduit NOT IN(SELECT li.idProduit FROM listeIngredients li, plat pl, commande c WHERE li.idRecette = pl.idRecette and pl.idPlat = c.idPlat and datediff(now(), c.date) < 31);");
        }

        private void clickEditionXML(object sender, RoutedEventArgs e)
        {
            XmlDocument docXml = new XmlDocument(); 
            XmlElement Seed = docXml.CreateElement("CommandesAPasserAuxFournisseurs");         
            docXml.AppendChild(Seed); 
            XmlDeclaration xmldecl = docXml.CreateXmlDeclaration("1.0", "UTF-8", "no");      
            docXml.InsertBefore(xmldecl, Seed);

            List<Produit> listeProduitACommander = RootClass.ExecutionRequetePRODUIT("SELECT * FROM produit WHERE stockActuel<stockMini;");
            List<string> listeidFournisseur = RootClass.ExecutionRequeteListString("SELECT f.idFournisseur FROM fournisseur f, produit p WHERE p.idFournisseur=f.idFournisseur and p.stockActuel<p.stockMini GROUP BY idFournisseur; ");
            for (int i = 0; i< listeidFournisseur.Count; i++)
            {
                XmlElement Fournisseur = docXml.CreateElement("Fournisseur"); 
                Seed.AppendChild(Fournisseur); 

                XmlAttribute nomFournisseur = docXml.CreateAttribute("nomFournisseur");   
                nomFournisseur.Value = RootClass.ExecutionRequeteString("SELECT nomFournisseur FROM fournisseur WHERE idFournisseur='"+listeidFournisseur[i]+"';");                               
                Fournisseur.SetAttributeNode(nomFournisseur);

                for (int j = 0; j < listeProduitACommander.Count; j++)
                {
                    if (listeProduitACommander[j].IdFournisseur == listeidFournisseur[i])
                    {

                        //on recommence ces opérations chaque noeuds et leurs attributs
                        XmlElement Produit = docXml.CreateElement("Produit");
                        Fournisseur.AppendChild(Produit);

                        XmlAttribute nomProduit = docXml.CreateAttribute("nomProduit");
                        nomProduit.Value = RootClass.ExecutionRequeteString("SELECT nomProduit FROM produit WHERE idProduit='"+listeProduitACommander[j].IdProduit+"';");
                        Produit.SetAttributeNode(nomProduit);


                        XmlAttribute Quantité = docXml.CreateAttribute("Quantité");
                        int stockActuel = int.Parse(RootClass.ExecutionRequeteString("SELECT stockActuel FROM produit WHERE idProduit='" + listeProduitACommander[j].IdProduit + "';"));
                        int stockMax=int.Parse(RootClass.ExecutionRequeteString("SELECT stockMaxi FROM produit WHERE idProduit='" + listeProduitACommander[j].IdProduit + "';"));
                        int quantiteACommander = stockMax-stockActuel;
                        Quantité.Value = quantiteACommander.ToString();
                        Produit.SetAttributeNode(Quantité);
                    }

                }

            }
            

            docXml.Save("Commandes-Fournisseurs.xml"); 
        }
    }
}

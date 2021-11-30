drop database if exists cooking;
create database cooking;
use cooking;

#Création des tables
CREATE TABLE `cooking`.`client` (
  `idClient` VARCHAR(10) NOT NULL,
  `nomClient` VARCHAR(50) NOT NULL,
  `numTelephoneC` VARCHAR(20),
  PRIMARY KEY (`idClient`) );
--  
CREATE TABLE `cooking`.`createur` (
  `idCdr` VARCHAR(10) NOT NULL,
  `benefice` INT default 0,
  `idClient` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`idCdr`),
  FOREIGN KEY (`idClient`) REFERENCES `cooking`.`client` (`idClient`)
  ON DELETE NO ACTION ON UPDATE CASCADE);
--  
CREATE TABLE `cooking`.`fournisseur`(
  `idFournisseur` VARCHAR(10) NOT NULL,
  `nomFournisseur` VARCHAR(50) NOT NULL,
  `numTelephoneF` VARCHAR(20),
  PRIMARY KEY (`idFournisseur`) );
--
CREATE TABLE `cooking`.`produit`(
  `idProduit` VARCHAR(10) NOT NULL,
  `nomProduit` VARCHAR(50) NOT NULL,
  `categorie` VARCHAR(50) NOT NULL,
  `unite` VARCHAR(20),
  `stockActuel` INT NOT NULL,
  `stockMini` INT,
  `stockMaxi` INT,
  `idFournisseur` VARCHAR(10),
  PRIMARY KEY (`idProduit`),
  FOREIGN KEY (`idFournisseur`) REFERENCES `cooking`.`fournisseur` (`idFournisseur`)
  ON DELETE SET NULL ON UPDATE CASCADE);
  --
CREATE TABLE `cooking`.`recette`(
  `idRecette` VARCHAR(10) NOT NULL,
  `nomRecette` VARCHAR(50) NOT NULL,
  `type` VARCHAR(50) NOT NULL,
  `description` VARCHAR(250),
  `prixDeVente` INT NOT NULL,
  `idCdr` VARCHAR(10),
  `compteur` INT DEFAULT 0,
  `remunerationCdr` INT DEFAULT 2,
  PRIMARY KEY (`idRecette`),
  FOREIGN KEY (`idCdr`) REFERENCES `cooking`.`createur` (`idCdr`)
  ON DELETE CASCADE ON UPDATE CASCADE);
  --
  CREATE TABLE `cooking`.`listeIngredients`(
  `idRecette` VARCHAR(10) NOT NULL,
  `idProduit` VARCHAR(10) NOT NULL,
  `quantite` INT NOT NULL,
  PRIMARY KEY (`idRecette`,`idProduit`),
  FOREIGN KEY (`idRecette`) REFERENCES `cooking`.`recette` (`idRecette`)
  ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (`idProduit`) REFERENCES `cooking`.`produit` (`idProduit`)
  ON DELETE CASCADE ON UPDATE CASCADE);
    --
  CREATE TABLE `cooking`.`plat`(
  `idPlat` VARCHAR(10) NOT NULL,
  `idRecette` VARCHAR(10),
  `quantitePlat` INT NOT NULL,
  `idCdr` VARCHAR(10),
  PRIMARY KEY (`idPlat`),
  FOREIGN KEY (`idRecette`) REFERENCES `cooking`.`recette` (`idRecette`)
  ON DELETE SET NULL ON UPDATE CASCADE,
  FOREIGN KEY (`idCdr`) REFERENCES `cooking`.`createur` (`idCdr`)
  ON DELETE SET NULL ON UPDATE CASCADE);
      --
  CREATE TABLE `cooking`.`commande`(
  `idCommande` VARCHAR(10),
  `idClient` VARCHAR(10),
  `idPlat` VARCHAR(10) NOT NULL,
  `date` DATETIME NOT NULL,
  PRIMARY KEY (`idCommande`,`idPlat`),
  FOREIGN KEY (`idClient`) REFERENCES `cooking`.`client` (`idClient`)
  ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY (`idPlat`) REFERENCES `cooking`.`plat` (`idPlat`)
  ON DELETE NO ACTION ON UPDATE NO ACTION);
  
  
  #Description des tables
    DESC client;
    DESC createur;
	DESC fournisseur;
    DESC produit;
    DESC recette;
    DESC listeIngredients;
    DESC plat;
    DESC commande;
    

  #Insertion de quelques données  
  INSERT INTO `cooking`.`client` VALUES ('G1','El Director','0651221281');
  INSERT INTO `cooking`.`client` VALUES ('C1','De la Fayette','0678451245');
  INSERT INTO `cooking`.`client` VALUES ('C2','Dupont','0687542154');
  INSERT INTO `cooking`.`client` VALUES ('C3','Tchikladze', '0686367590');
  INSERT INTO `cooking`.`client` VALUES ('C4','Senejko', '0798426028');

  INSERT INTO `cooking`.`createur` VALUES ('CdR1','500','C3');
  INSERT INTO `cooking`.`createur` VALUES ('CdR2','500','C4');
  
  INSERT INTO `cooking`.`fournisseur` VALUES ('F1','La compagnie des Indes Orientales','0192461348');
  INSERT INTO `cooking`.`fournisseur` VALUES ('F2','Bio C Bon','0156981308');
  INSERT INTO `cooking`.`fournisseur` VALUES ('F3', "L'épicerie du coin", '0154673210');
  INSERT INTO `cooking`.`fournisseur` VALUE ('F4','Le VERITABLE Fermier','0127210908');
  INSERT INTO `cooking`.`fournisseur` VALUE ('F5','L\'attrape nigauds','0978456481');
  
  INSERT INTO `cooking`.`produit` VALUES ('PR1','Ornithorynque','Viande','Grammes','5000','1000','10000','F1');
  INSERT INTO `cooking`.`produit` VALUES ('PR2', 'Courgette', 'Légume', 'Unité', '300','50','1000','F2');
  INSERT INTO `cooking`.`produit` VALUES ('PR3', 'Pâte feuilletée', 'Produit transformé', 'Unité', '50', '20', '200', 'F3');
  INSERT INTO `cooking`.`produit` VALUES ('PR4', 'Lait', 'Produit laitier', 'cL', '2000', '3000', '10000', 'F3');
  INSERT INTO `cooking`.`produit` VALUES ('PR5', 'Crème', 'Produit laitier', 'cL', '2000', '3000', '10000', 'F3');
  INSERT INTO `cooking`.`produit` VALUES ('PR6', 'Lardons', 'Viande', 'Grammes', '5000', '1000', '20000', 'F3');
  INSERT INTO `cooking`.`produit` VALUES ('PR7', 'Gruyère râpé', 'Produit laitier', 'Grammes', '5000', '2000', '20000', 'F3');  
  INSERT INTO `cooking`.`produit` VALUES ('PR8', 'Chocolat noir', 'Matière Première', 'Grammes', '10000', '1000', '50000', 'F3');
  INSERT INTO `cooking`.`produit` VALUES ('PR9', 'Beurre', 'Produit laitier', 'Grammes', '30000', '5000', '100000', 'F4');
  INSERT INTO `cooking`.`produit` VALUES ('PR10', 'Sucre en poudre', 'Matière Première', 'Grammes', '20000', '10000', '100000', 'F3');
  INSERT INTO `cooking`.`produit` VALUES ('PR11', 'Farine', 'Matière Première', 'Grammes', '20000', '10000', '100000', 'F4');
  INSERT INTO `cooking`.`produit` VALUES ('PR12', 'Oeufs', 'Matière Première', 'Unité', '1000', '50', '5000', 'F4');
  INSERT INTO `cooking`.`produit` VALUES ('PR13','Amandes en poudre','Matière Première','Grammes','2000','1500','10000','F1');
  INSERT INTO `cooking`.`produit` VALUES ('PR14','Ras el hanout','Epice','Grammes','150','100','1000','F5');
  
  INSERT INTO `cooking`.`recette` VALUES ("R1","Ragout Ornithorynque","Plat principal","Quelques légumes entourés par des tranches fines d'Ornithorynque.",'37','CdR1','2','2');
  INSERT INTO `cooking`.`recette` VALUES ("R2","Quiche","Plat principal","La meilleure quiche fait maison.",'10','CdR1','4','2');
  INSERT INTO `cooking`.`recette` VALUES ("R3","Gâteau au chocolat","Dessert","Le meilleur gâteau au chocolat !",'15','CdR2','2','2');
  INSERT INTO `cooking`.`recette` VALUES ("R4","Quiche à l'ornithorynque","Plat principal","Le mix entre mes 2 meilleures recettes ! Vous ne serez pas déçus c'est promis !",'30','CdR1','1','2');
  INSERT INTO `cooking`.`recette` VALUES ("R5","Galette des Rois","Dessert","La fameuse recette de votre grand-mère revisitée pour une dégustation savoureuse",'17','CdR2','0','2');
  INSERT INTO `cooking`.`recette` VALUES ("R6","Crêpe au chocolat","Dessert","La recette parfaite!",'5','CdR2','0','2');

  INSERT INTO `cooking`.`listeIngredients` VALUES ('R1','PR1','200');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R1','PR2','2');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R2','PR3','1');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R2','PR4','20');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R2','PR5','20');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R2','PR6','200');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R2','PR7','50');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R2','PR12','3');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R3','PR8','200');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R3','PR9','125');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R3','PR10','100');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R3','PR11','50');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R3','PR12','3');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R4','PR3','1');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R4','PR4','20');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R4','PR5','20');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R4','PR1','150');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R4','PR7','50');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R4','PR12','3');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R5','PR3','2');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R5','PR13','140');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R5','PR10','100');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R5','PR12','3');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R5','PR9','75');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R6','PR11','300');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R6','PR4','100');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R6','PR9','60');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R6','PR12','4');
  INSERT INTO `cooking`.`listeIngredients` VALUES ('R6','PR8','30');

  INSERT INTO `cooking`.`plat` VALUES ('PL1','R1','2','CdR1');
  INSERT INTO `cooking`.`plat` VALUES ('PL2','R2','4','CdR1');
  INSERT INTO `cooking`.`commande` VALUES ('CO1','C3','PL1','2020/04/19 00:00:00');
  INSERT INTO `cooking`.`commande` VALUES ('CO1','C3','PL2','2020/04/19 00:00:00');
  INSERT INTO `cooking`.`plat` VALUES ('PL3','R3','2','CdR2');
  INSERT INTO `cooking`.`plat` VALUES ('PL4','R4','1','CdR1');
  INSERT INTO `cooking`.`commande` VALUES ('CO2','C4','PL3','2020/05/03 10:00:00');
  INSERT INTO `cooking`.`commande` VALUES ('CO2','C4','PL4','2020/05/03 10:00:00');


#Affichage des éléments des tables
  SELECT * FROM client;
  SELECT * FROM createur;
  SELECT * FROM fournisseur;
  SELECT * FROM produit;
  SELECT * FROM recette;
  SELECT * FROM listeIngredients;
  SELECT * FROM plat;
  SELECT * FROM commande;
  
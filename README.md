# Cahier des charges ✨ Speedex 🚀

Ce cahier des charges contient les fonctionnalités à développer découpées en
plusieurs User Stories (US).

L'objectif de cet exercice est de pratiquer le développement de fonctionnalités
accompagné de tests unitaires/intégrations. Libre à vous d'ajouter des tests
d'intégration. Ils seront bien entendu appréciés lors de la correction ;)

L'ordre des US n'est pas figé, vous pouvez les traiter dans l'ordre que vous
souhaitez en fonction de vos préférences.

## Nommage des commits 📝

Chaque commit doit correspondre à une User Story développée.
Lorsque vous faites des commits, rajoutez à la fin l'id de l'US correspondante.

Exemple de nom de commit :

```text
feat: votre message de commit #US-01
fix: votre message de commit #US-02
refactor: votre message de commit #US-03
feat: votre message de commit #US-04 
```

Cela permettra de tracer facilement les fonctionnalités développées.

NB : Si vous avez des questions ou des points de blocage, n'hésitez pas à lever
la main. L'utilisation de Google ou de LLM n'est pas interdite.

## Rendu du développement 📦

### Option 1 : Si vous avez un compte GitHub

#### Fork du projet

Une fois les fonctionnalités développées, poussez vos commits sur votre fork.
Créez une Pull Request (PR) depuis votre fork vers la branche `test/td-note` du
repository d'origine avec votre nom/prénom en titre de la PR.

### Option 2 : Si vous n’avez pas de compte GitHub

#### Téléchargement et configuration du projet

Une fois les fonctionnalités développées, créez un zip de votre code source
incluant le dossier **.git**.

Nommez votre fichier zip avec votre nom/prénom (ex. : Nom_Prenom_TD.zip) et
envoyez-le par email :

- _hugo.tachoires@u-bordeaux.fr_

---

## US-01 - Le nom du destinataire de la commande doit être en majuscule ✉️

En tant que **responsable logistique**, je veux que le nom du destinataire d’une
commande soit toujours formaté en majuscule afin de garantir la cohérence et la
lisibilité des informations sur les étiquettes de livraison.

## US-02 - Ajouter un filtre pour les produits en fonction de la catégorie 🛍️

En tant que **client**, je veux pouvoir filtrer les produits par catégorie sur
le
catalogue en ligne pour trouver rapidement les articles qui m’intéressent.

## US-03 - Ajouter un filtre pour récupérer les commandes d'un client en fonction de son adresse email 📧

En tant que **responsable service client**, je veux pouvoir rechercher toutes
les
commandes associées à un client à partir de son adresse email pour répondre plus
rapidement aux demandes de suivi.

## US-04 - Vérifier que le poids total des produits d’une commande est inférieur à 30 kg ⚖️

En tant que **responsable entrepôt**, je veux m'assurer que chaque colis ne
dépasse
pas 30 kg pour respecter les règles de transport des partenaires logistiques.

## US-05 - Vérifier que le volume total des produits est inférieur à 1m³ 📦

En tant que **responsable entrepôt**, je veux garantir que le volume des
produits
d’un colis reste inférieur à 1 m³ pour optimiser le stockage et la livraison.

## US-06 - Vérifier que les produits donnés sont bien dans la commande ✅

En tant que **responsable entrepôt**, je veux vérifier que seuls les produits
appartenant à une commande peuvent être ajoutés à un colis pour éviter les
erreurs d’expédition.

## US-07 - Vérifier que les produits donnés respectent la quantité demandée 🔢

En tant que **responsable entrepôt**, je veux m’assurer que les quantités
ajoutées
au colis respectent les quantités commandées pour éviter les erreurs d’envoi.

## US-08 - Calculer le prix total d'une commande sans tenir compte des devises 💰

En tant que **comptable**, je veux connaître le prix total d’une commande en
fonction des prix unitaires des produits pour faciliter la facturation.

## US-10 - Calculer le prix total d'une commande en tenant compte des devises 🌍

En tant que **comptable**, je veux calculer le prix total d’une commande en
convertissant les prix des produits dans une devise unique (euros) pour
simplifier les reportings financiers.

## US-11 - Compléter la route de statistiques pour récupérer le top 3 des clients qui ont le plus dépensé 🥇🥈🥉

En tant que **responsable marketing**, je veux connaître les 3 clients les plus
dépensiers pour leur proposer des offres exclusives.
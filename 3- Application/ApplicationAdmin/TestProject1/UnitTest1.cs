using ApplicationAdmin.Model_View;
using ApplicationAdmin.Model;
using ApplicationAdmin.DAL;

namespace TestProject1
{
    public class Tests
    {
        AppVM Vm;
        Dal Dal;

        [SetUp]
        public void Setup()
        {
            Dal = new Dal();
            Vm = new AppVM();
            Vm.EvenementSelectionne = Dal.EvenementFactory.GetAll()[0];
        }

        [Test]
        public void TestCreerEvenement()
        {
            string artiste = "ArtisteTest";
            string NomSpectacle = "NomSpectacleTest";
            string Path = "";
            string Path2 = "";
            string Date = "2020-02-02";
            string Date2 = "2020-02-02";
            string Heure = "18h";
            string prix = "18000";

            foreach (CoutRange c in Vm.CoutRange)
            {
                c.Cout = 27;
            }

            Vm.CréerEvenement(artiste, NomSpectacle, Path, Path2, Date, Date2, Heure, prix);

            List<Evenement> evenements = Dal.EvenementFactory.GetAll();
            bool valid = false;

            foreach (Evenement e in evenements)
            {
                if (e.Artiste == artiste && e.NomSpectacle == NomSpectacle && e.Image == Path && e.Date.ToString() == Date && e.DateLimite.ToString() == Date2 && e.Heure == Heure && e.CoutEvenement.ToString() == prix)
                {
                    Vm.EvenementSelectionne = e;
                    Vm.SuppEvenement();
                    valid = true;
                }
            }

            Assert.AreEqual(valid, true);
        }


        [Test]
        public void TestDeleteEvenement()
        {
            string artiste = "ArtisteTest";
            string NomSpectacle = "NomSpectacleTest";
            string Path = "";
            string Path2 = "";
            string Date = "2020-02-02";
            string Date2 = "2020-02-02";
            string Heure = "18h";
            string prix = "18000";

            foreach (CoutRange c in Vm.CoutRange)
            {
                c.Cout = 27;
            }

            Vm.CréerEvenement(artiste, NomSpectacle, Path, Path2, Date, Date2, Heure, prix);

            List<Evenement> evenements = Dal.EvenementFactory.GetAll();
            bool valid = false;

            foreach (Evenement e in evenements)
            {
                if (e.Artiste == artiste && e.NomSpectacle == NomSpectacle && e.Image == Path && e.Date.ToString() == Date && e.DateLimite.ToString() == Date2 && e.Heure == Heure && e.CoutEvenement.ToString() == prix)
                {
                    Vm.EvenementSelectionne = e;
                    Vm.SuppEvenement();
                    valid = true;
                    List<Evenement> evenementscheck = Dal.EvenementFactory.GetAll();
                    foreach (Evenement ev in evenementscheck)
                    {
                        if (ev.Artiste == artiste && ev.NomSpectacle == NomSpectacle && ev.Image == Path && ev.Date.ToString() == Date && ev.DateLimite.ToString() == Date2 && ev.Heure == Heure && ev.CoutEvenement.ToString() == prix)
                        {
                            valid = false;
                        }
                    }
                }
            }

            Assert.AreEqual(valid, true);
        }

        [Test]
        public void TestCreerUser()
        {

            var initialResponse = Vm.checkInscription("ExempleTest4", "Password1", "Password1", "email4@gmail.com");

            if (initialResponse == "Valide")
            {
                // create account
                Vm.CreerUser("ExempleTest4", "Password1", "email4@gmail.com", "Admin");

                // test
                List<Admin> admins = Dal.AdminFactory.GetAll();
                bool valid = false;

                foreach (Admin a in admins)
                {
                    if (a.Name == "ExempleTest4")
                    {
                        valid = true;
                    }
                }

                // Clear DataBase
                Dal.AdminFactory.DeleteAccount("ExempleTest4");

                // Assert
                Assert.AreEqual(valid, true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateEvenements()
        {
            string artiste = "ArtisteTest";
            string NomSpectacle = "NomSpectacleTest";
            string Path = "";
            string Path2 = "";
            string Date = "2020-02-02";
            string Date2 = "2020-02-02";
            string Heure = "18h";
            string prix = "18000";

            foreach (CoutRange c in Vm.CoutRange)
            {
                c.Cout = 27;
            }

            Vm.CréerEvenement(artiste, NomSpectacle, Path, Path2, Date, Date2, Heure, prix);

            List<Evenement> evenements = Dal.EvenementFactory.GetAll();
            bool valid = false;

            foreach (Evenement e in evenements)
            {
                if (e.Artiste == artiste && e.NomSpectacle == NomSpectacle && e.Image == Path && e.Date.ToString() == Date && e.DateLimite.ToString() == Date2 && e.Heure == Heure && e.CoutEvenement.ToString() == prix)
                {
                    Vm.EvenementSelectionne = e;
                    Vm.EvenementSelectionne.NomSpectacle = "NomChangeTest";
                    Vm.UpdateEvenements("2020-02-02", "2020-02-02");
                }
            }

            List<Evenement> evenements2 = Dal.EvenementFactory.GetAll();
            foreach (Evenement e in evenements2)
            {
                if (e.NomSpectacle == "NomChangeTest")
                {
                    valid = true;
                }
            }

            Assert.AreEqual(valid, true);



            Vm.SuppEvenement();
        }

        [Test]
        public void TestChangeName()
        {
            var initialResponse = Vm.checkInscription("ExempleTest5", "Password1", "Password1", "email5@gmail.com");

            if (initialResponse == "Valide")
            {
                // create account
                Vm.CreerUser("ExempleTest5", "Password1", "email5@gmail.com", "Admin");

                // test
                List<Admin> admins = Dal.AdminFactory.GetAll();

                foreach (Admin a in admins)
                {
                    if (a.Name == "ExempleTest5")
                    {
                        Vm.AdminConnecte = a;
                    }
                }
            }
            else
            {
                Assert.Fail();
            }

            Vm.ChangeName("NouveauNomTest");

            // Assert
            bool verif = false;

            List<Admin> admins2 = Dal.AdminFactory.GetAll();

            foreach (Admin a in admins2)
            {
                if (a.Name == "NouveauNomTest")
                {
                    verif=true;
                }
            }

            // Clear DataBase
            Dal.AdminFactory.DeleteAccount("NouveauNomTest");
        }
    }
}
using ApplicationClient.Model_View;
using ApplicationClient.Model;
using ApplicationClient.DAL;
using Org.BouncyCastle.Crypto.Engines;

namespace TestProject2
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
        public void TestPlaceExist()
        {
            bool valid = true;
            if(!Vm.PlaceExist("A", 1))
            {
                valid = false;
            }
            if (Vm.PlaceExist("Z", 1))
            {
                valid = false;
            }
            if (Vm.PlaceExist("B", 80))
            {
                valid = false;
            }
            Assert.AreEqual(valid, true);
        }

        

        [Test]
        public void TestInscriptionPassword()
        {
            //Wrong password
            string rep = Vm.Inscription("ExempleTest1", "Password1", "WrongPassword", "email@gmail.com");
            Assert.AreEqual(rep, "Les mots de passes doivent être similaires");
        }

        [Test]
        public void TestInscriptionUsername()
        {
            // test
            var initialResponse = Vm.Inscription("ExempleTest2", "Password1", "Password1", "email@gmail.com");

            if (initialResponse == "Valide")
            {
                // create account
                Vm.CreerCompte("ExempleTest2", "Password1", "email@gmail.com");

                // test
                var registrationResponse = Vm.Inscription("ExempleTest2", "Password1", "Password1", "anotheremail@gmail.com");

                // Clear DataBase
                Dal.ClientFactory.DeleteAccount("ExempleTest2");

                // Assert
                Assert.AreEqual("Nom existant", registrationResponse);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestInscriptionEmail()
        {
            // test
            var initialResponse = Vm.Inscription("ExempleTest3", "Password1", "Password1", "sameemail@gmail.com");

            if (initialResponse == "Valide")
            {
                // create account
                Vm.CreerCompte("ExempleTest3", "Password1", "sameemail@gmail.com");

                // test
                var registrationResponse = Vm.Inscription("ExempleTest3", "Password1", "Password1", "sameemail@gmail.com");

                // Clear DataBase
                Dal.ClientFactory.DeleteAccount("ExempleTest3");

                // Assert
                Assert.AreEqual("Email existant", registrationResponse);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreerCompte()
        {
            var initialResponse = Vm.Inscription("ExempleTest4", "Password1", "Password1", "email4@gmail.com");

            if (initialResponse == "Valide")
            {
                // create account
                Vm.CreerCompte("ExempleTest4", "Password1", "email4@gmail.com");

                // test
                List<Client> clients = Dal.ClientFactory.GetAll();
                bool valid = false;

                foreach(Client c in clients)
                {
                    if (c.Name == "ExempleTest4")
                    {
                        valid = true;
                    }
                }

                // Clear DataBase
                Dal.ClientFactory.DeleteAccount("ExempleTest4");

                // Assert
                Assert.AreEqual(valid, true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGetMaxRange()
        {
            int a = AppVM.GetMaxRange("A");
            int c = AppVM.GetMaxRange("C");
            int e = AppVM.GetMaxRange("E");
            int g = AppVM.GetMaxRange("G");
            int i = AppVM.GetMaxRange("I");

            if(a==18&& c == 30 && e == 28 && g == 33 && i == 24)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestChangerMotDePasse()
        {
            var initialResponse = Vm.Inscription("ExempleTest5", "Password1", "Password1", "email5@gmail.com");

            if (initialResponse == "Valide")
            {
                // create account
                Vm.CreerCompte("ExempleTest5", "Password1", "email5@gmail.com");

                // test
                Client c = Vm.Connection("ExempleTest5", "Password1");
                bool valid = false;
                if(c != null)
                {
                    Vm.ClientConnecte = c;
                    bool temp = Vm.ChangerMotDePasse("Password1", "Password2", "Password2");
                    Vm.RefreshUserConnecter();
                    if(Vm.ClientConnecte.Password == Hasher.HashPassword("Password2"))
                    {
                        valid = true;
                    }
                }
                else
                {
                    Assert.Fail();
                }

                // Clear DataBase
                Dal.ClientFactory.DeleteAccount("ExempleTest5");

                // Assert
                Assert.AreEqual(valid, true);
            }
            else
            {
                Assert.Fail();
            }
        }
        
    }
}
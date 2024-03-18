using ApplicationAdmin.Model;
using ApplicationAdmin.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;
using ApplicationAdmin.View;
using System.Linq;
using Mysqlx.Crud;
using System.Diagnostics;
using System.Windows.Input;
using System.Security.RightsManagement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net.Http;
using System.Text;
using System.Drawing;
using System.Text.Json;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace ApplicationAdmin.Model_View
{
    public class AppVM : PropertyChanger
    {
        #region private variables

        Admin? adminConnecte;
        Dal dal = new Dal();
        IEnumerable<Evenement> evenements;
        Evenement evenementSelectionne;
        CoutRange[] coutRange;
        Evenement evenementRapportSelection;
        List<Client> clients;
        Client clientRapportSelection;
        double remplie;
        int nbbillet;
        double profit;
        double revenu;
        double cout;
        string typerapport;
        bool imageValid;
        string imageName;
        string dateAjout;
        string dateLimiteAjout;
        string coutAjout;
        string coutUpdate;



        #endregion

        #region public variables

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };
        public IEnumerable<Evenement> Evenements { get { return evenements; } set { evenements = value; } }
        public Admin? AdminConnecte
        {
            get { return adminConnecte; }
            set
            {
                adminConnecte = value;
                OnPropertyChanged(nameof(AdminConnecte));
            }
        }
        public Evenement EvenementSelectionne { get { return evenementSelectionne; } set { evenementSelectionne = value; } }
        public CoutRange[] CoutRange { get { return coutRange; } set { coutRange = value; } }
        public Evenement EvenementRapportSelection { get { return evenementRapportSelection; } set { evenementRapportSelection = value; } }
        public List<Client> Clients { get { return clients; } set { clients = value; } }
        public Client ClientRapportSelection { get { return clientRapportSelection; } set { clientRapportSelection = value; } }
        public double Remplie { get { return remplie; } set { remplie = value; } }
        public int nbBillet { get { return nbbillet; } set { nbbillet = value; } }
        public double Revenu { get { return revenu; } set { revenu = value; } }
        public double Profit { get { return profit; } set { profit = value; } }
        public double Cout { get { return cout; } set { cout = value; } }
        public string TypeRapport { get { return typerapport; } set { typerapport = value; } }
        public bool ImageValid { get { return imageValid; } set { imageValid = value; } }
        public string ImageName { get { return imageName; } set { imageName = value; } }
        public string DateAjout  { get { return dateAjout; } set { dateAjout = value; } }
        public string DateLimiteAjout { get { return dateLimiteAjout; } set { dateLimiteAjout = value; } }
        public string CoutAjout { get { return coutAjout; } set { coutAjout = value; } }
        public string CoutUpdate { get { return coutUpdate; } set { coutUpdate = value; } }


        #endregion

        #region Command properties
        public ICommand ProduireRapport { get; }
        public ICommand EnregistrerRapport { get; }
        public ICommand CreerEvenement { get; }
        public ICommand UpdateEvenement { get; }
        public ICommand Modifier { get; }

        

        #endregion Command properties

        #region Methodes

        public Admin Connection(string username, string password)
        {
            return dal.AdminFactory.CheckConnection(username, password);
        }

        public void MettreajourSpectacle()
        {
            Evenements = dal.EvenementFactory.GetAll();
            
        }

        public void RefreshUserConnecter()
        {
            Admin c = dal.AdminFactory.Get(AdminConnecte.Id);
            AdminConnecte = c;
        }

        public void RefreshCoutRange()
        {
            if(EvenementSelectionne != null)
            {
                for (int i = 0; i < 25; i++)
                {
                    bool test = double.TryParse(EvenementSelectionne.Cout.Split(",")[i].Replace(".",","), out double cout);
                    if (test)
                        CoutRange[i].Cout = cout;
                    else
                        CoutRange[i].Cout = 0;
                }
            }
            else
            {
                for (int i = 0; i < 25; i++)
                {
                    CoutRange[i].Cout = 0;
                }
            }
            
        }

        public void ResetCoutRange()
        {
            for (int i = 0; i < 25; i++)
            {
                CoutRange[i].Cout = 0;
            }
        }

        public void ChangeName(string nouveauNom)
        {
            dal.AdminFactory.ChangeName(AdminConnecte, nouveauNom);
        }

        public void CréerEvenement(string Artiste, string NomSpectacle, string ImageSpectaclePath, string ImageSpectacle, string Datestring, string Datestring2, string Heure, string CoutEvenement)
        {
            string cout = this.CoutRange[0].Cout.ToString().Replace(",", ".");

            for(int i = 0;i<25;i++)
            {
                cout = cout + "," + this.CoutRange[i].Cout.ToString().Replace(",",".");
            }
            
            DateOnly.TryParse(Datestring, out DateOnly Date);
            DateOnly.TryParse(Datestring2, out DateOnly Date2);

            dal.EvenementFactory.Add(Artiste, NomSpectacle, ImageSpectacle, Date,Date2, Heure, cout, CoutEvenement);
            if(ImageSpectaclePath!="")
            {
                AddImageToServer(ImageSpectaclePath);
            }
            
        }

        public static double GetPrixBillet(Evenement evenement, string Range)
        {
            double CoutPlace = 0;
            for (int i = 0; i < 25; i++)
            {
                if (evenement.salle[i][0].Range == Range)
                {
                    CoutPlace = evenement.salle[i][0].Prix;
                }
            }
            return CoutPlace;
        }

        public static int ConvertRangeToInd(string Range)
        {
            Dictionary<string, int> rangeToIndex = new Dictionary<string, int>
            {
                {"A", 1},
                {"B", 2},
                {"C", 3},
                {"D", 4},
                {"E", 5},
                {"F", 6},
                {"G", 7},
                {"H", 8},
                {"I", 9},
                {"J", 10},
                {"K", 11},
                {"L", 12},
                {"M", 13},
                {"N", 14},
                {"O", 15},
                {"P", 16},
                {"Q", 17},
                {"AM", 18},
                {"BM", 19},
                {"CM", 20},
                {"DM", 21},
                {"AA", 22},
                {"BB", 23},
                {"CC", 24},
                {"DD", 25},
            };

            if (rangeToIndex.TryGetValue(Range, out int id))
            {
                return id;
            }

            return 0;
        }

        public static string ConvertIndToRange(int index)
        {
            Dictionary<int, string> indexToRange = new Dictionary<int, string>
            {
                {1, "A"},
                {2, "B"},
                {3, "C"},
                {4, "D"},
                {5, "E"},
                {6, "F"},
                {7, "G"},
                {8, "H"},
                {9, "I"},
                {10, "J"},
                {11, "K"},
                {12, "L"},
                {13, "M"},
                {14, "N"},
                {15, "O"},
                {16, "P"},
                {17, "Q"},
                {18, "AM"},
                {19, "BM"},
                {20, "CM"},
                {21, "DM"},
                {22, "AA"},
                {23, "BB"},
                {24, "CC"},
                {25, "DD"},
            };

            if (indexToRange.TryGetValue(index, out string range))
            {
                return range;
            }

            return "";
        }

        public static int GetMaxRange(string Range)
        {

            int max = 0;

            if (Range == "A")
            {
                max = 18;
            }
            if (Range == "B")
            {
                max = 25;
            }
            if (Range == "C")
            {
                max = 30;
            }
            if (Range == "D")
            {
                max = 31;
            }
            if (Range == "E")
            {
                max = 28;
            }
            if (Range == "F")
            {
                max = 20;
            }
            if (Range == "G")
            {
                max = 33;
            }
            if (Range == "H")
            {
                max = 32;
            }
            if (Range == "I")
            {
                max = 24;
            }
            if (Range == "J")
            {
                max = 16;
            }
            if (Range == "K")
            {
                max = 41;
            }
            if (Range == "L")
            {
                max = 44;
            }
            if (Range == "M")
            {
                max = 47;
            }
            if (Range == "N")
            {
                max = 50;
            }
            if (Range == "O")
            {
                max = 48;
            }
            if (Range == "P")
            {
                max = 48;
            }
            if (Range == "Q")
            {
                max = 51;
            }
            if (Range == "AM")
            {
                max = 4;
            }
            if (Range == "BM")
            {
                max = 4;
            }
            if (Range == "CM")
            {
                max = 4;
            }
            if (Range == "DM")
            {
                max = 4;
            }
            if (Range == "AA")
            {
                max = 54;
            }
            if (Range == "BB")
            {
                max = 49;
            }
            if (Range == "CC")
            {
                max = 48;
            }
            if (Range == "DD")
            {
                max = 49;
            }
            return max;

        }

        public void SetUpEvenementRapport()
        {
            Model.Rapport r = dal.RapportFactory.GetRapportEvenement(EvenementRapportSelection.Id);
            Remplie = r.Remplie;
            nbBillet = r.nbBillet;
            Revenu = r.Revenu;
            Profit = r.Profit;
            Cout = r.Cout;
        }

        public void SetUpClientRapport()
        {
            Model.Rapport r = dal.RapportFactory.GetRapportClient(ClientRapportSelection.Id);
            Remplie = r.Remplie;
            nbBillet = r.nbBillet;
            Revenu = r.Revenu;
        }

        public void SetUpDateRapport(DateOnly min, DateOnly max)
        {
            Model.Rapport r = dal.RapportFactory.GetRapportDate(min,max);
            Remplie = r.Remplie;
            nbBillet = r.nbBillet;
            Revenu = r.Revenu;
            Profit = r.Profit;
            Cout = r.Cout;
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public void CreatePDF(string Path, string type)
        {
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            string pasclient = "";
            string titre = "";

            if(type=="CLIENT")
            {
                titre = "Rapport de client";
            }
            if (type == "EVENEMENT")
            {
                titre = "Rapport d'évenement";
            }
            if (type == "DATE")
            {
                titre = "Rapport de date";
            }

            if (type!="CLIENT")
            {
                pasclient =
                "<p><strong>Profit :</strong> " + Profit.ToString("00.00") + "$ </p>\r\n                " +
                "<p><strong>Cout :</strong> " + Cout.ToString("00.00") + "$ </p>\r\n                " +
                "<p><strong>Pourcentage de salle remplie :</strong> " + Math.Round(Remplie*100d,2) + "% </p>\r\n            ";
            }

            //Convert URL to PDF document. 
            PdfDocument document = htmlConverter.Convert("<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Theater Ticket</title>\r\n    <style>\r\n        body {\r\n            font-family: 'Arial', sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            display: flex;\r\n            align-items: center;\r\n            justify-content: center;\r\n            height: 100%;\r\n        }\r\n\r\n        .ticket {\r\n            background-color: #fff;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n            border-radius: 10px;\r\n            overflow: hidden;\r\n            max-width: 800px;\r\n            min-width: 800px;\r\n            margin: 20px;\r\n        }\r\n\r\n        .ticket-header {\r\n            background-color: #333;\r\n            color: #fff;\r\n            padding: 15px;\r\n            text-align: center;\r\n            font-size: 1.2em;\r\n        }\r\n\r\n        .ticket-details {\r\n            padding: 20px;\r\n        }\r\n\r\n        .event-info p {\r\n            margin: 8px 0;\r\n        }\r\n\r\n        .qr-code {\r\n            max-width: 100%;\r\n            height: auto;\r\n            display: block;\r\n            margin: 20px auto;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"ticket\">\r\n        <div class=\"ticket-header\">\r\n            " +
                "<h2>"+titre+"</h2>\r\n        </div>\r\n        <div class=\"ticket-details\">\r\n            <div class=\"event-info\">\r\n                " +

            "<p><strong>Nombre de Billets vendus :</strong> " + nbBillet + "</p>\r\n                " +
            "<p><strong>Revenu :</strong> " + Revenu.ToString("00.00") + "$ </p>\r\n                " +

            pasclient +

            "</div>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>", "");

            string stringdate = RemoveWhitespace(DateTime.Now.ToString()).Replace(':', '-');

            string temppath = "Rtemp/rapport" + stringdate + ".pdf";

            // Save the PDF document to a file
            document.Save(temppath);

            // Close the PDF document
            document.Close(true);
            
            try
            {
                if (!File.Exists(temppath))
                {
                    // This statement ensures that the file is created,
                    // but the handle is not kept.
                    using (FileStream fs = File.Create(temppath)) { }
                }

                // Ensure that the target does not exist.
                if (File.Exists(Path))
                    File.Delete(Path);

                // Move the file.
                File.Move(temppath, Path+ "/rapport" + stringdate + ".pdf");
                
                

                // See if the original exists now.
                if (File.Exists(temppath))
                {
                    ErrorWindow er = new ErrorWindow("The original file still exists, which is unexpected.");
                    er.Show();
                }
                
            }
            catch (Exception e)
            {
                ErrorWindow erw = new ErrorWindow(e.Message);
                erw.Show();
            }

        }

        public void SetupClientList()
        {
            Clients = dal.ClientFactory.GetAll();
        }

        bool CanExecuteCreerEvenement(object parameter)
        {
            
            bool verif3 = double.TryParse(CoutAjout, out double c);


            if (ImageValid && verif3)
            {
                return true;
            }

            return false;
        }

        bool CanExecuteProduireRapport(object parameter)
        {
            if(TypeRapport=="DATE")
            {
                return true;
            }
            if (TypeRapport == "CLIENT")
            {
                if (ClientRapportSelection == null)
                    return false;
                else
                    return true;
            }
            if (TypeRapport == "EVENEMENT")
            {
                if (EvenementRapportSelection == null)
                    return false;
                else
                    return true;
            }
            return false;
        }

        bool CanExecuteEnregistrerRapport(object parameter)
        {
            if (TypeRapport == "DATE")
            {
                return true;
            }
            if (TypeRapport == "CLIENT")
            {
                if (ClientRapportSelection == null)
                    return false;
                else
                    return true;
            }
            if (TypeRapport == "EVENEMENT")
            {
                if (EvenementRapportSelection == null)
                    return false;
                else
                    return true;
            }
            return false;
        }

        void ExecuteProduireRapport(object parameter)
        {

        }

        void ExecuteEnregistrerRapport(object parameter)
        {

        }

        void ExecuteCreerEvenement(object parameter)
        {
        }
        
        void ExecuteModifier(object parameter)
        {

        }

        bool CanExecuteModifier(object parameter)
        {
            if (ImageValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ExecuteUpdateEvenement(object parameter)
        {

        }

        bool CanExecuteUpdateEvenement(object parameter)
        {
            if(EvenementSelectionne!= null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string checkInscription(string username, string password1, string password2, string email)
        {
            string retour = "";
            if (password1 != password2)
            {
                retour = "Les mots de passes doivent être similaires";
            }
            else if (!IsValidEmail(email))
            {
                retour = "Le e-mail doit être valide";
            }
            else
            {
                retour = dal.ClientFactory.CheckInscription(username, email);
            }
            return retour;
        }

        private bool IsValidEmail(string email)
        {
            // Regular expression pattern for a basic email validation
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to test the email against the pattern
            return regex.IsMatch(email);
        }

        public void CreerUser(string nom, string mdp, string mail, string role)
        {
            dal.AdminFactory.CreerCompte(nom, mdp, mail, role);
        }

        public async void AddImageToServer(string imageFilePath)
        {
            HttpClient httpClient = AppVM.sharedClient;
            string uploadUrl = "https://imagesesp.w4-alexandre.vtinyhosting.site";


            using (HttpClient client = new HttpClient())
            using (MultipartFormDataContent formData = new MultipartFormDataContent())
            using (StreamContent fileContent = new StreamContent(File.OpenRead(imageFilePath)))
            {
                // Ajoute l'image au contenu multipart
                formData.Add(fileContent, "image", System.IO.Path.GetFileName(imageFilePath));

                try
                {
                    // Envoie la requête POST
                    HttpResponseMessage response = await client.PostAsync(uploadUrl, formData);

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Windows.MessageBox.Show("Une erreur est survenue lors du téléchargement de l'image. Code d'erreur : " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Une erreur est survenue : " + ex.Message);
                }
            }
        }

        public void UpdateEvenements(string Date1,string Date2)
        {
            string cout = this.CoutRange[0].Cout.ToString().Replace(",", ".");

            for (int i = 0; i < 25; i++)
            {
                cout = cout + "," + this.CoutRange[i].Cout.ToString().Replace(",", ".");
            }

            DateOnly.TryParse(Date1, out DateOnly Datestring);
            DateOnly.TryParse(Date2, out DateOnly Datestring2);

            dal.EvenementFactory.Update(EvenementSelectionne.Id, EvenementSelectionne.Artiste, EvenementSelectionne.NomSpectacle, EvenementSelectionne.Image, Datestring, Datestring2, EvenementSelectionne.Heure, cout, EvenementSelectionne.CoutEvenement.ToString());
        }

        public void SuppEvenement()
        {
            dal.EvenementFactory.Supprimer(EvenementSelectionne.Id);
        }

        

        #endregion

        #region Constructeur

        public AppVM()
        {
            CoutRange = new CoutRange[25];
            CoutRange[0] = new CoutRange("A",0);
            CoutRange[1] = new CoutRange("B", 0);
            CoutRange[2] = new CoutRange("C", 0);
            CoutRange[3] = new CoutRange("D", 0);
            CoutRange[4] = new CoutRange("E", 0);
            CoutRange[5] = new CoutRange("F", 0);
            CoutRange[6] = new CoutRange("G", 0);
            CoutRange[7] = new CoutRange("H", 0);
            CoutRange[8] = new CoutRange("I", 0);
            CoutRange[9] = new CoutRange("J", 0);
            CoutRange[10] = new CoutRange("K", 0);
            CoutRange[11] = new CoutRange("L", 0);
            CoutRange[12] = new CoutRange("M", 0);
            CoutRange[13] = new CoutRange("N", 0);
            CoutRange[14] = new CoutRange("O", 0);
            CoutRange[15] = new CoutRange("P", 0);
            CoutRange[16] = new CoutRange("Q", 0);
            CoutRange[17] = new CoutRange("AM", 0);
            CoutRange[18] = new CoutRange("BM", 0);
            CoutRange[19] = new CoutRange("CM", 0);
            CoutRange[20] = new CoutRange("DM", 0);
            CoutRange[21] = new CoutRange("AA", 0);
            CoutRange[22] = new CoutRange("BB", 0);
            CoutRange[23] = new CoutRange("CC", 0);
            CoutRange[24] = new CoutRange("DD", 0);

            ProduireRapport = new CommandRelay(ExecuteProduireRapport, CanExecuteProduireRapport);
            EnregistrerRapport = new CommandRelay(ExecuteEnregistrerRapport, CanExecuteEnregistrerRapport);
            CreerEvenement = new CommandRelay(ExecuteCreerEvenement, CanExecuteCreerEvenement);
            UpdateEvenement = new CommandRelay(ExecuteUpdateEvenement, CanExecuteUpdateEvenement);
            Modifier = new CommandRelay(ExecuteModifier, CanExecuteModifier);
        }

        #endregion

        #region PropretyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

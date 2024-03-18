using ApplicationClient.DAL;
using ApplicationClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using MailKit.Net.Smtp;
using ApplicationClient.View;
using MimeKit;
using QRCoder;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.HtmlConverter;


namespace ApplicationClient.Model_View
{
    public class AppVM : PropertyChanger
    {
        #region private variables

        Dal dal = new Dal();
        Model.Client? clientConnecte;
        List<Evenement> evenements;
        Evenement evenementSelectionne;
        string selectedPlaces;
        double coutPlace;
        string rangeSelectionne;
        string bancSelectionne;
        double coutTotal;
        List<Billet> panier;
        Billet billetSelectionner;
        double prixTotalPanier;
        List<Billet> billetsHistorique;
        Billet historiqueSelection;
        private System.Net.Mail.Attachment attachment;

        #endregion

        #region public variables

        public List<Evenement> Evenements { get { return evenements; } set { evenements = value; ChangedValue("Evenements"); } }
        public Model.Client? ClientConnecte { get { return clientConnecte; } set { clientConnecte = value; } }
        public Evenement? EvenementSelectionne { get { return evenementSelectionne; } 
            set { evenementSelectionne = value; } }
        public string SelectedPlaces { get { return selectedPlaces; } set { selectedPlaces = value; ChangedValue("SelectedPlaces"); } }
        public double CoutPlace { get { return coutPlace; } set { coutPlace = value; ChangedValue("CoutPlace"); } }
        public string RangeSelectionne { get { return rangeSelectionne; } set { rangeSelectionne = value; RefreshPrixBanc(); ChangedValue("RangeSelectionne"); } }
        public string BancSelectionne { get { return bancSelectionne; } set { bancSelectionne = value; RefreshPrixBanc(); ChangedValue("BancSelectionne"); } }
        public double CoutTotal { get { return coutTotal; } set { coutTotal = value; ChangedValue("CoutTotal"); } }
        public double PrixTotalPanier { get { return prixTotalPanier; } set { prixTotalPanier = value; ChangedValue("PrixTotalPanier"); } }
        public List<Billet> Panier { get { return panier; } set { panier = value; ChangedValue("Panier"); } }
        public Billet BilletSelectionner { get { return billetSelectionner; } set { billetSelectionner = value; ChangedValue("BilletSelectionner"); } }
        public List<Billet> BilletsHistorique { get { return billetsHistorique; } set { billetsHistorique = value; ChangedValue("BilletsHistorique"); } }
        public Billet HistoriqueSelection { get { return historiqueSelection; } set { historiqueSelection = value; ChangedValue("HistoriqueSelection"); } }

        #endregion

        #region Command properties
        public ICommand Acheter { get; }
        public ICommand AjouterSiege { get; }
        public ICommand RetirerBilletPanier { get; }
        public ICommand PayerPanier { get; }
        public ICommand VoirEvent { get; }

        


        #endregion Command properties

        #region Methodes

        public Model.Client Connection(string username,string password)
        {
            return dal.ClientFactory.CheckConnection(username, password);
        }

        public string Inscription(string username, string password1, string password2, string email)
        {
            string retour = "";
            if(password1 != password2)
            {
                retour = "Les mots de passes doivent être similaires";
            }
            else if(!IsValidEmail(email))
            {
                retour = "Le e-mail doit être valide";
            }
            else
            {
                retour = dal.ClientFactory.CheckInscription(username,email);
            }
            return retour;
        }

        public void CreerCompte(string username, string password, string email)
        {
            dal.ClientFactory.CreerCompte(username, password, email);
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

        public void MettreajourSpectacle()
        {
            Evenements = dal.EvenementFactory.GetAll();
        }

        public void MettreajourSpectacle(string text)
        {
            Evenements = Evenements = dal.EvenementFactory.GetAllfilter(text);
        }

        public void ChangeName(string nouveauNom)
        {
            dal.ClientFactory.ChangeName(ClientConnecte, nouveauNom);
        }

        public void RefreshUserConnecter()
        {
            Model.Client c = dal.ClientFactory.Get(ClientConnecte.Id);
            ClientConnecte = c;
        }

        public void RefreshPrixBanc()
        {
            CoutPlace = GetPrixBillet(EvenementSelectionne, RangeSelectionne);
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

        public void RefreshSelection(string banc, string range)
        {
            RangeSelectionne = range;
            BancSelectionne = banc;
        }

        async void ExecuteAcheter(object parameter)
        {
            Model.Panier p = dal.PanierFactory.GetPanierClient(ClientConnecte.Id);
            if(p == null)
            {
                p = dal.PanierFactory.CreerPanier(ClientConnecte.Id);
            }

            string[] places = SelectedPlaces.Split(",");
            foreach(string place in places)
            {
                string Range = place.Split("-")[0];
                string Banc = place.Split("-")[1];
                await CreerBillet(Banc, Range, EvenementSelectionne.Id, ClientConnecte.Id);
                dal.BilletFactory.AjouterBilletPanier(dal.BilletFactory.Getbyplace(Banc, Range, EvenementSelectionne.Id).Id, p.Id);
                RefreshPanier();
            }

            SelectedPlaces = "";
            RangeSelectionne = "";
            BancSelectionne = "";
        }

        async Task<bool> CreerBillet(string Banc, string Range, int Event, int Client)
        {
            return dal.BilletFactory.CreerBillet(Banc, Range, Event, Client);
        }

        bool CanExecuteAcheter(object parameter)
        {
            if(SelectedPlaces != "" && SelectedPlaces != null)
            {
                return true;
            }
            return false;
        }

        void ExecuteAjouterSiege(object parameter)
        {
            int.TryParse(BancSelectionne, out int bancint);
            //dal.BilletFactory.BilletOccupe(e, range, place);
            for (int i = 0; i < 25; i++)
            {
                if (EvenementSelectionne.salle[i][0].Range == RangeSelectionne)
                {
                    CoutTotal += EvenementSelectionne.salle[i][0].Prix;

                    if (SelectedPlaces == null || SelectedPlaces == "")
                    {
                        SelectedPlaces = RangeSelectionne + "-" + BancSelectionne;
                    }
                    else
                    {
                        SelectedPlaces = SelectedPlaces + "," + RangeSelectionne + "-" + BancSelectionne ;
                    }
                    
                    BancSelectionne = "";
                    RangeSelectionne = "";
                    CoutPlace = 0;

                }
            }
        }

        bool CanExecuteAjouterSiege(object parameter)
        {
            if (PlaceExist() && PlaceLibre(EvenementSelectionne,RangeSelectionne,BancSelectionne))
                return true;
            return false;
        }

        bool PlaceExist()
        {
            bool valid1 = false;
            int bancint;
            bool valid2 = int.TryParse(BancSelectionne, out bancint);

            if(valid2)
            {
                for (int i = 0; i < 25; i++)
                {
                    foreach (Place e in EvenementSelectionne.salle[i])
                    {
                        if (e.Range == RangeSelectionne && e.Numero == bancint)
                        {
                            valid1 = true;
                        }
                    }

                }
            }

            if(valid1&&valid2)
                return true;
            return false;
        }

        public bool PlaceExist(string range, int place)
        {
            bool valid1 = false;
 
            for (int i = 0; i < 25; i++)
            {
                foreach (Place e in EvenementSelectionne.salle[i])
                {
                    if (e.Range == range && e.Numero == place)
                    {
                        valid1 = true;
                    }
                }
            }

            return valid1;
        }

        public bool PlaceLibre(Evenement e, string range, string place)
        {
            return !dal.BilletFactory.BilletExist(e,range,place);
        }

        public List<List<bool>> GetDispoSalle()
        {
            List<List<bool>> dispoSalle = new List<List<bool>>();
            List<Billet> billets = dal.BilletFactory.ListBilletsEvenement(EvenementSelectionne);

            for (int i = 0; i < 25; i++)
            {
                dispoSalle.Add(new List<bool>());
                
                foreach (Place p in EvenementSelectionne.salle[i])
                {
                    bool valid1 = false;
                    foreach (Billet b in billets)
                    {
                        if (p.Range == b.Range && p.Numero.ToString() == b.Banc)
                        {
                            valid1 = true;
                            break;
                        }
                    }
                    dispoSalle[i].Add(valid1);
                }
            }

            return dispoSalle;
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

        void ExecuteRetirerBilletPanier(object parameter)
        {
            dal.PanierFactory.RetirerBillet(BilletSelectionner.Id);
            BilletSelectionner = null;
            RefreshPanier();
        }

        void ExecutePayerPanier(object parameter)
        {
            EnvoyerMail(Panier);
            dal.PanierFactory.PayerPanier(ClientConnecte.Id);
            dal.PanierFactory.CreerPanier(ClientConnecte.Id);
            RefreshPanier();
            RefreshBilletHistorique();
            
        }

        public void RefreshPrixTotalPanier()
        {
            double total = 0;
            foreach(Billet b in Panier)
            {
                total += b.Prix;
            }

            PrixTotalPanier= total;
        }

        bool CanExecuteRetirerBilletPanier(object parameter)
        {
            if(BilletSelectionner == null)
            {
                return false;
            }
            return true;
        }

        public void RefreshPanier()
        {
            Panier = dal.PanierFactory.GetAllBillets(ClientConnecte.Id);
            RefreshPrixTotalPanier();
        }

        bool CanExecutePayerPanier(object parameter)
        {
            if(Panier.Count() == 0)
            {
                return false;
            }
            return true;
        }

        bool CanExecuteVoirEvent(object parameter)
        {
            if (EvenementSelectionne==null)
            {
                return false;
            }
            return true;
        }

        void ExecuteVoirEvent(object parameter)
        {
            
        }

        public bool ChangerMotDePasse(string ancien,string nouveau1,string nouveau2)
        {
            if(nouveau1==nouveau2)
            {
                if (ClientConnecte.Password == Hasher.HashPassword(ancien))
                {
                    dal.ClientFactory.ChangeMDP(ClientConnecte,nouveau1);
                    RefreshUserConnecter();
                    return true;
                }
            }
            return false;
        }

        public void RefreshBilletHistorique()
        {
            BilletsHistorique = dal.PanierFactory.GetHistorique(ClientConnecte.Id);
        }

        public void RefreshPrixVoir()
        {
            CoutTotal = 0;
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
        
        private void CreatePDF(Billet billet)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(billet.Id.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrCodeImage = qRCode.GetGraphic(5);
            Bitmap resized = new Bitmap(qrCodeImage, new System.Drawing.Size(qrCodeImage.Width / 4, qrCodeImage.Height / 4));

            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            //Convert URL to PDF document. 
            PdfDocument document = htmlConverter.Convert("<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Theater Ticket</title>\r\n    <style>\r\n        body {\r\n            font-family: 'Arial', sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            display: flex;\r\n            align-items: center;\r\n            justify-content: center;\r\n            height: 100vh;\r\n        }\r\n\r\n        .ticket {\r\n            background-color: #fff;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n            border-radius: 10px;\r\n            overflow: hidden;\r\n            max-width: 1000px;\r\n     min-width: 1000px;\r\n           margin: 20px;\r\n        }\r\n\r\n        .ticket-header {\r\n            background-color: #333;\r\n            color: #fff;\r\n            padding: 15px;\r\n            text-align: center;\r\n            font-size: 1.2em;\r\n        }\r\n\r\n        .ticket-details {\r\n            padding: 20px;\r\n        }\r\n\r\n        .event-info p {\r\n            margin: 8px 0;\r\n        }\r\n\r\n        .qr-code {\r\n            max-width: 100%;\r\n            height: auto;\r\n            display: block;\r\n            margin: 20px auto;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"ticket\">\r\n        <div class=\"ticket-header\">\r\n            <h2>Théâtre CChic Ticket</h2>\r\n        </div>\r\n        <div class=\"ticket-details\">\r\n            <div class=\"event-info\">\r\n            " +
                "<p><strong>Evenement:</strong>"+ billet.Evenement.NomSpectacle +"</p>\r\n" +
                "<p><strong>Artiste:</strong> "+ billet.Evenement.Artiste +"</p>\r\n"+
                "<p><strong>Artiste:</strong> " + billet.Range + billet.Banc +"</p>\r\n" +
                "<p><strong>Date:</strong> " + billet.Evenement.Date + " " + billet.Evenement.Heure + "</p>\r\n" +
                "<p><strong>Coût:</strong> $"+ billet.Prix +"</p>\r\n </div>\r\n" +
                "</div>\r\n    </div>\r\n</body>\r\n</html>", "");

            PdfGraphics graphics = document.Pages[0].Graphics;
            PdfBitmap imagepdf = new PdfBitmap(resized);
            
            graphics.DrawImage(imagepdf,250, 425);

            // Save the PDF document to a file
            document.Save("Billet" + billet.Id + ".pdf");

            // Close the PDF document
            document.Close(true);
        }

        async void EnvoyerMail(List<Billet> panier)
        {
            try
            {
                using var email = new MimeMessage();
                email.From.Add(new MailboxAddress(name:"Theatre Cchic", address: "esp@w4-alexandre.vtinyhosting.site"));
                email.To.Add(new MailboxAddress(name: "Client", address: ClientConnecte.Email));
                email.Subject = "Confirmation d'achat";
                string tablecontent = "";
                
                double coutTotal = 0;
                List<int> billetsIds = new List<int>();
                
                foreach (Billet b in panier)
                {
                    CreatePDF(b);
                    
                    billetsIds.Add(b.Id);
                    tablecontent += "     <tr>\r\n            <td>" + b.Evenement.NomSpectacle + "</td>\r\n            <td>" + b.Evenement.Artiste + "</td>\r\n            <td>" + b.Range + b.Banc + "</td>\r\n            <td>" + b.Evenement.Date + "</td>\r\n            <td>$"+ b.Prix.ToString("00.00") +"</td>\r\n          </tr>\r\n  ";
                    coutTotal += b.Prix;
                }
                var builder = new BodyBuilder()
                {
                    HtmlBody = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <style>\r\n      body {\r\n        font-family: Arial, sans-serif;\r\n      }\r\n      .container {\r\n        width: 80%;\r\n        margin: auto;\r\n        padding: 20px;\r\n        border: 1px solid #ddd;\r\n        border-radius: 5px;\r\n      }\r\n      .header {\r\n        text-align: center;\r\n        margin-bottom: 20px;\r\n      }\r\n      .footer {\r\n        text-align: center;\r\n        margin-bottom: 20px;\r\n      }\r\n      .table {\r\n        width: 100%;\r\n        border-collapse: collapse;\r\n      }\r\n      th,\r\n      td {\r\n        padding: 10px;\r\n        border: 1px solid #ddd;\r\n        text-align: left;\r\n      }\r\n      th {\r\n        background-color: #f2f2f2;\r\n      }\r\n      .ticket {\r\n            background-color: #fff;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n            border-radius: 10px;\r\n            overflow: hidden;\r\n            max-width: 400px;\r\n            margin: 20px;\r\n        }\r\n\r\n        .ticket-header {\r\n            background-color: #333;\r\n            color: #fff;\r\n            padding: 15px;\r\n            text-align: center;\r\n            font-size: 1.2em;\r\n        }\r\n\r\n        .ticket-details {\r\n            padding: 20px;\r\n        }\r\n\r\n        .event-info p {\r\n            margin: 8px 0;\r\n        }\r\n\r\n        .qr-code {\r\n            max-width: 100%;\r\n            height: auto;\r\n            display: block;\r\n            margin: 20px auto;\r\n        }\r\n    </style>\r\n  </head>\r\n  <body>\r\n    <div class=\"container\">\r\n      <div class=\"header\">\r\n        <h1>Merci pour votre achat!</h1>\r\n      </div>\r\n      <table class=\"table\">\r\n        <thead>\r\n          <tr>\r\n            <th>Spectacle</th>\r\n            <th>Artiste</th>\r\n            <th>Place</th>\r\n            <th>Date</th>\r\n            <th>Prix</th>\r\n          </tr>\r\n        </thead>\r\n        <tbody>\r\n     " +
                    
                    tablecontent +

                    "        </tbody>\r\n        <tfoot>\r\n          <tr>\r\n            <td colspan=\"5\">Total: $" + coutTotal.ToString("00.00") + "</td>\r\n          </tr>\r\n      " +
                    
                    "  </tfoot>\r\n      </table>\r\n      </div>\r\n      <div class=\"footer\">\r\n        <p>\r\n          Si vous avez des questions, contactez-nous au\r\n          <a href=\"mailto:esp@w4-alexandre.vtinyhosting.site\">esp@w4-alexandre.vtinyhosting.site</a>.\r\n        </p>\r\n        <p>Merci pour votre confiance!</p>\r\n      </div>\r\n    </div>\r\n  </body>\r\n</html>"
                };
                
                
                for(int i =0;i<panier.Count;i++)
                {
                    builder.Attachments.Add("Billet" + billetsIds[i] + ".pdf");
                }
                
                email.Body = builder.ToMessageBody();


                using var smtp = new SmtpClient();
                smtp.Connect(host: "mail.w4-alexandre.vtinyhosting.site", port: 465, useSsl: true);

                smtp.Authenticate(userName:"esp@w4-alexandre.vtinyhosting.site",password: "Popomoka_8934");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                ErrorWindow ErWin = new ErrorWindow(ex.Message);
                ErWin.Show();
            }
        }

        #endregion

        #region Constructeur

        public AppVM()
        {
            CoutTotal = 0;
            Acheter = new CommandRelay(ExecuteAcheter, CanExecuteAcheter);
            AjouterSiege = new CommandRelay(ExecuteAjouterSiege, CanExecuteAjouterSiege);
            RetirerBilletPanier = new CommandRelay(ExecuteRetirerBilletPanier, CanExecuteRetirerBilletPanier);
            PayerPanier = new CommandRelay(ExecutePayerPanier, CanExecutePayerPanier);
            VoirEvent = new CommandRelay(ExecuteVoirEvent, CanExecuteVoirEvent);
        }

        #endregion
    }
}

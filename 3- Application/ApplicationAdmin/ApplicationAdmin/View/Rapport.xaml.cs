using ApplicationAdmin.Model_View;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using WinForms = System.Windows.Forms;
using System.Windows.Data;

namespace ApplicationAdmin.View
{
    /// <summary>
    /// Logique d'interaction pour Rapport.xaml
    /// </summary>
    public partial class Rapport : Page
    {
        AppVM vm;
        string Type;
        public Rapport(AppVM Vm,string type)
        {
            vm = Vm;
            vm.SetupClientList();
            InitializeComponent();
            if (type == "CLIENT")
            {
                Client.Visibility = Visibility.Visible;
                Date.Visibility = Visibility.Hidden;
                Evenement.Visibility = Visibility.Hidden;

            }
            if (type == "DATE")
            {
                Date.Visibility = Visibility.Visible;
                Client.Visibility = Visibility.Hidden;
                Evenement.Visibility = Visibility.Hidden;

            }
            if (type == "EVENEMENT")
            {
                Evenement.Visibility = Visibility.Visible;
                Client.Visibility = Visibility.Hidden;
                Date.Visibility = Visibility.Hidden;

            }
            Type = type;
            
            this.DataContext = vm;
            
        }


        

        private void Confirmer(object sender, RoutedEventArgs e)
        {
            
            if (Type == "CLIENT")
            {
                vm.SetUpClientRapport();
                RapportWindow rw = new RapportWindow(vm, "Rapport de client");
                rw.Show();
            }
            if (Type == "DATE")
            {
                DateOnly.TryParse(Debut.Text, out DateOnly DateDebut);
                DateOnly.TryParse(Fin.Text, out DateOnly DateFin);
                vm.SetUpDateRapport(DateDebut, DateFin);
                RapportWindow rw = new RapportWindow(vm, "Rapport de date");
                rw.Show();

            }
            if (Type == "EVENEMENT")
            {
                vm.SetUpEvenementRapport();
                RapportWindow rw = new RapportWindow(vm,"Rapport d'évenement");
                rw.Show();

            }
        }

        private void Enregistrer(object sender, RoutedEventArgs e)
        {
            if (Type == "CLIENT")
            {
                vm.SetUpClientRapport();
            }
            if (Type == "DATE")
            {
                DateOnly.TryParse(Debut.Text, out DateOnly DateDebut);
                DateOnly.TryParse(Fin.Text, out DateOnly DateFin);
                vm.SetUpDateRapport(DateDebut, DateFin);
            }
            if (Type == "EVENEMENT")
            {
                vm.SetUpEvenementRapport();
            }


            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
           
            dialog.InitialDirectory = "C:\\Users\\alexo\\OneDrive\\Desktop\\ESP\\3- Application\\ApplicationAdmin\\ApplicationAdmin\\bin\\Debug\\net6.0-windows\\Rapports";
            dialog.ShowDialog();

            if(dialog.SelectedPath != null && dialog.SelectedPath != "")
            {
                vm.CreatePDF(dialog.SelectedPath, Type);
            }
            
        }
    }
}

using ApplicationAdmin.Model;
using ApplicationAdmin.Model_View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationAdmin.View
{
    /// <summary>
    /// Logique d'interaction pour AjouterEvenement.xaml
    /// </summary>
    public partial class AjouterEvenement : Page
    {
        AppVM vm;
        Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();
        public AjouterEvenement(AppVM Vm)
        {
            vm = Vm;
            vm.ResetCoutRange();
            vm.ImageValid = false;
            InitializeComponent();

            this.DataContext = vm;
        }

        private void Créer(object sender, RoutedEventArgs e)
        {
            vm.CréerEvenement(Artiste.Text, Nom.Text, file.FileName,file.SafeFileName, Datepicker.Text, Datepickerlimite.Text, Heure.Text, CoutEvenement.Text);
            this.NavigationService.Navigate(new Accueil(vm));
        }
       
        private void Places(object sender, RoutedEventArgs e)
        {

        }

        private void AccountClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Account(vm));
        }

        private void PanierClick(object sender, RoutedEventArgs e)
        {

        }

        private void Buttonfile_Click(object sender, RoutedEventArgs e)
        {
            
            bool? succes = file.ShowDialog();
            if (succes==true)
            {
                if(file.FileName.Contains(".jpg") || file.FileName.Contains(".png"))
                {
                    vm.ImageValid = true;
                    filename.Content = file.SafeFileName;
                    filename.Foreground = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    vm.ImageValid = false;
                    filename.Content = file.SafeFileName;
                    filename.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                vm.ImageValid = false;
            }
        }

        
    }
}

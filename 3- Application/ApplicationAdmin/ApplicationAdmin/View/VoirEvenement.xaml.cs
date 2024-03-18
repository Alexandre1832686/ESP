using ApplicationAdmin.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationAdmin.View
{
    /// <summary>
    /// Logique d'interaction pour VoirEvenement.xaml
    /// </summary>
    public partial class VoirEvenement : Page
    {
        AppVM vm;
        Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();
        public VoirEvenement(AppVM Vm)
        {
            vm = Vm;
            vm.MettreajourSpectacle();
            vm.RefreshCoutRange();
            InitializeComponent();
            this.DataContext = vm;
            vm.ImageValid = true;
        }

        private void AccountClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Account(vm));
        }

        private void Modifier(object sender, RoutedEventArgs e)
        {
            vm.UpdateEvenements(Datepicker.Text,Datepickerlimite.Text);
            this.NavigationService.Navigate(new Accueil(vm));
        }

        private void Buttonfile_Click(object sender, RoutedEventArgs e)
        {
            bool? succes = file.ShowDialog();
            if (succes == true)
            {
                if (file.FileName.Contains(".jpg") || file.FileName.Contains(".png"))
                {
                    
                    filename.Text = file.SafeFileName;
                    filename.Foreground = System.Windows.Media.Brushes.Black;
                    vm.EvenementSelectionne.Image = file.SafeFileName;
                    vm.ImageValid = true;
                    vm.AddImageToServer(file.FileName);
                }
                else
                {
                    vm.ImageValid = false;

                    filename.Text = file.SafeFileName;
                    filename.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                vm.ImageValid = false;
            }
            
        }

        private void Supprimer(object sender, RoutedEventArgs e)
        {
            vm.SuppEvenement();
            this.NavigationService.Navigate(new Accueil(vm));
        }
    }
}

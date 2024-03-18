using ApplicationAdmin.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Page
    {
        AppVM vm;
        public Accueil(AppVM Vm)
        {
            vm = Vm;
            

            vm.MettreajourSpectacle();
            InitializeComponent();
            if (vm.AdminConnecte.Role == "SuperAdmin")
            {
                CreerUserButton.Visibility = Visibility.Visible;
            }
            else
            {
                CreerUserButton.Visibility = Visibility.Hidden;
            }
            this.DataContext = vm;
        }

        private void Spectacle(object sender, RoutedEventArgs e)
        {

        }

        private void AccountClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Account(vm));
        }

        private void Voir(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new VoirEvenement(vm));
        }

        private void Rapport(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SelectionRapport(vm));
        }

        private void Evenement(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AjouterEvenement(vm));
        }

        private void CreerUser(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreerUser(vm));
        }
    }
}

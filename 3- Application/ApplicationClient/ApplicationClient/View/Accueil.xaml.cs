using ApplicationClient.Model_View;
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
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace ApplicationClient.View
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
            this.DataContext = vm;
        }
        
        private void Spectacle(object sender, RoutedEventArgs e)
        {

        }

        private void AccountClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Account(vm));
        }

        private void PanierClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Panier(vm));
        }

        private void Voir(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new VoirEvenement(vm));
        }


        private void RechercheButtonClicked(object sender, RoutedEventArgs e)
        {
            vm.MettreajourSpectacle(Recherche.Text);
            this.DataContext = vm;
        }
    }
}

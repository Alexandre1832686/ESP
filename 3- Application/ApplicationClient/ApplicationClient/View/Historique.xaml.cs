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

namespace ApplicationClient.View
{
    /// <summary>
    /// Logique d'interaction pour Historique.xaml
    /// </summary>
    public partial class Historique : Page
    {
        AppVM vm;
        public Historique(AppVM vm)
        {
            this.vm = vm;
            vm.RefreshBilletHistorique();
            this.DataContext = vm;
            InitializeComponent();
        }

        private void AccountClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Account(vm));
        }

        private void PanierClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Panier(vm));
        }
    }
}

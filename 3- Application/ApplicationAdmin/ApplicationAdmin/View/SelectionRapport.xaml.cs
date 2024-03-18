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
    /// Logique d'interaction pour SelectionRapport.xaml
    /// </summary>
    public partial class SelectionRapport : Page
    {
        AppVM vm;
        public SelectionRapport(AppVM Vm)
        {
            vm = Vm;
            vm.MettreajourSpectacle();
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Evenement(object sender, RoutedEventArgs e)
        {
            vm.TypeRapport = "EVENEMENT";
            this.NavigationService.Navigate(new Rapport(vm,"EVENEMENT"));
        }

        private void Date(object sender, RoutedEventArgs e)
        {
            vm.TypeRapport = "DATE";
            this.NavigationService.Navigate(new Rapport(vm, "DATE"));
        }

        private void Client(object sender, RoutedEventArgs e)
        {
            vm.TypeRapport = "CLIENT";
            this.NavigationService.Navigate(new Rapport(vm, "CLIENT"));
        }
    }
}

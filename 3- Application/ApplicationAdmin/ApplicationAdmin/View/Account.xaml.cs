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
    /// Logique d'interaction pour Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        AppVM vm;
        public Account(AppVM Vm)
        {
            InitializeComponent();
            vm = Vm;
            this.DataContext= vm;
            vm.RefreshUserConnecter();
        }

        private void HistoriqueAchat(object sender, RoutedEventArgs e)
        {

        }

        private void ChangerMotDePasse(object sender, RoutedEventArgs e)
        {

        }

        private void ChangerMotDeNom(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ChangerNom(vm));
        }

        private void Deconection(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Connection(vm));
            
        }
    }
}

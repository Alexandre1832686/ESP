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
using ApplicationClient.Model;
using ApplicationClient.Model_View;

namespace ApplicationClient.View
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : Page
    {
        AppVM vm;
        public Connection(AppVM vm)
        {
            InitializeComponent();
            usernameTextBox.Text = "";
            passwordTextBox.Password = "";
            
            this.vm = vm;
            connectionIncorrect.Foreground = Brushes.LightGray;
        }

        public Connection()
        {
            InitializeComponent();
            this.vm = new AppVM();
        }

        private void GoTo_Inscription(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Inscription(vm));
        }

        private void ConnectionClick(object sender, RoutedEventArgs e)
        {
            Client c = vm.Connection(usernameTextBox.Text, passwordTextBox.Password);
            if ( c == null)
            {
                connectionIncorrect.Foreground = Brushes.Red;
            }
            else
            {
                vm.ClientConnecte = c;
                this.NavigationService.Navigate(new Accueil(vm));
                NavigationService.RemoveBackEntry();
            }
        }

        private void MotDePasseOublieClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

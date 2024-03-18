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
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Page
    {
        AppVM vm;
        public Inscription(AppVM Vm)
        {
            InitializeComponent();
            vm = Vm;
        }

        private void InscriptionClick(object sender, RoutedEventArgs e)
        {
            string reponse = vm.Inscription(Nom.Text, password1.Text, password2.Text, Mail.Text);
            if(reponse=="Valide")
            {
                vm.CreerCompte(Nom.Text, password1.Text, Mail.Text);
                this.NavigationService.Navigate(new Connection(vm));
            }
            else
            {
                errorMessage.Text = reponse;
            }
        }

        private void GoTo_Connection(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Connection(vm));
        }
    }
}

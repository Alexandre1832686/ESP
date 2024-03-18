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
    /// Logique d'interaction pour CreerUser.xaml
    /// </summary>
    public partial class CreerUser : Page
    {
        AppVM vm;
        public CreerUser(AppVM Vm)
        {
            vm = Vm;
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Confirmer(object sender, RoutedEventArgs e)
        {
            string role = "";
            if(Role.SelectedIndex== 0)
            {
                role = "Admin";
            }
            else
            {
                role = "SuperAdmin";
            }
            vm.CreerUser(Nom.Text, MotDePasse.Text, Mail.Text,role);
            this.NavigationService.Navigate(new Accueil(vm));
        }
    }
}

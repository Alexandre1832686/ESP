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
    /// Logique d'interaction pour changerMotDePasse.xaml
    /// </summary>
    public partial class changerMotDePasse : Page
    {
        AppVM vm;
        public changerMotDePasse(AppVM Vm)
        {
            vm = Vm;
            vm.MettreajourSpectacle();
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Confirmer(object sender, RoutedEventArgs e)
        {
            bool verif = vm.ChangerMotDePasse(Ancienmdp.Password, Nouveaumdp1.Password, Nouveaumdp2.Password);
            if(verif)
            {
                this.NavigationService.Navigate(new Account(vm));
                erreur.Foreground = Brushes.LightGray;
            }
            else
            {
                erreur.Foreground = Brushes.Red;
            }
        }
    }
}

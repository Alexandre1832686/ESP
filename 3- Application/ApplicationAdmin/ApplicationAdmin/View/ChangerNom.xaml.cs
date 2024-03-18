using ApplicationAdmin.Model_View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationAdmin.View
{
    /// <summary>
    /// Logique d'interaction pour ChangerNom.xaml
    /// </summary>
    public partial class ChangerNom : Page
    {
      
        AppVM vm;
        public ChangerNom(AppVM Vm)
        {
            InitializeComponent();
            vm = Vm;
            this.DataContext = vm;
        }

        private void Confirmer(object sender, RoutedEventArgs e)
        {
            vm.ChangeName(nouveauNom.Text);
            this.NavigationService.Navigate(new Account(vm));
        }
    }
}

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
using System.Windows.Shapes;

namespace ApplicationAdmin.View
{
    /// <summary>
    /// Logique d'interaction pour RapportWindow.xaml
    /// </summary>
    public partial class RapportWindow : Window
    {
        AppVM Vm;
        public RapportWindow(AppVM vm, string title)
        {
            Vm = vm;
            InitializeComponent();
            Title.Text = title;

            if(title== "Rapport de client")
            {
                Remplie2.Visibility= Visibility.Hidden;
                Cout2.Visibility = Visibility.Hidden;
                Profit2.Visibility = Visibility.Hidden;
                Remplie.Visibility = Visibility.Hidden;
                Cout.Visibility = Visibility.Hidden;
                Profit.Visibility = Visibility.Hidden;
            }

            this.DataContext = Vm;
        }
    }
}

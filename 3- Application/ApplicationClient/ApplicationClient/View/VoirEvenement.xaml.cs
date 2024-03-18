using ApplicationClient.Model;
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
    /// Logique d'interaction pour VoirEvenement.xaml
    /// </summary>
    public partial class VoirEvenement : Page
    {
        AppVM vm;
        public VoirEvenement(AppVM Vm)
        {
            vm = Vm;
            vm.RefreshPrixVoir();

            
            InitializeComponent();
            PrepareScene();
            this.DataContext = vm;
        }

        private void PanierClick(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Panier(vm));
           range.Text = "";
           banc.Text = "";
        }

        private void AccountClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Account(vm));
            range.Text = "";
            banc.Text = "";
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            vm.RefreshSelection(banc.Text, range.Text);
        }

        private void Acheter(object sender, RoutedEventArgs e)
        {
            range.Text = "";
            banc.Text = "";

            NavigationService.Navigate(new Panier(vm));
        }

        void PrepareScene()
        {
            List<List<bool>> dispos = vm.GetDispoSalle();
            
            for (int i = 0;i< 25; i++)
            {
                for (int j = 0; j < AppVM.GetMaxRange(AppVM.ConvertIndToRange(i+1)); j++)
                {
                    Grid place = new Grid();

                    TextBlock text = new TextBlock();
                    text.Text = (j+1).ToString();
                    text.FontSize = 8;

                    if (!dispos[i][j])
                    {
                        text.Background = Brushes.Green;
                    }
                    else
                    {
                        text.Background = Brushes.Red;
                    }
                        
                    place.Children.Add(text);

                    Scene.Children.Add(place);
                    Grid.SetRow(place, i);
                    Grid.SetColumn(place, j);
                    
                }
            }
        }
    }
}

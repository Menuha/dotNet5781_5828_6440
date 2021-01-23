using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using BLAPI;

namespace PL
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btStations_Click(object sender, RoutedEventArgs e)
        {
            StationsWindow secondWindow = new StationsWindow(bl);
            secondWindow.Show();

        }

        private void btLines_Click(object sender, RoutedEventArgs e)
        {
            LinesWindow secondWindow = new LinesWindow(bl);
            secondWindow.Show();
        }

    }
}

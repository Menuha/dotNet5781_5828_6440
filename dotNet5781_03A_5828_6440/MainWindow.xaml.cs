using dotNet5781_02_5828_6440;
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

namespace dotNet5781_03A_5828_6440
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusLine currentDisplayBusLine;
        private Buses busLines = new Buses();
        private static Random r = new Random();
        public MainWindow()
        {
            for (int i = 0; i < 10; i++)
            {
                int busCode = r.Next(1000);
                BusStation firstStation = new BusStation(r.Next(1000000));
                BusStation lastStation = new BusStation(r.Next(1000000));
                char ch = (char)r.Next(1, 8);
                Area myArea = (Area)Enum.Parse(typeof(Area), (string)(Object)ch);
                busLines.Add(new BusLine(busCode, firstStation, lastStation, myArea));
            }
            InitializeComponent();
            cbBusLines.ItemsSource = busLines.BusesList;
            cbBusLines.DisplayMemberPath = " BusLineNum "; 
            cbBusLines.SelectedIndex = 0;
        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).BusCode);
        }
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = busLines.BusesList[index];

            UpGrid.DataContext = currentDisplayBusLine;

            lbBusLineStations.DataContext = currentDisplayBusLine.StationsList;
        }

    }
}

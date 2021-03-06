using BLAPI;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for SimulateOneStationWindow.xaml
    /// </summary>
    public partial class SimulateOneStationWindow : Window
    {
        IBL bl;
        BO.Station sta;
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public SimulateOneStationWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            cbStationID.DisplayMemberPath = "Name";
            cbStationID.SelectedItem = "Code";

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            cbStationID.DataContext = bl.GetAllStations();
            cbStationID.SelectedIndex = 0; //index of the object to be selected

            dgLineTiming.IsReadOnly = true;
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            lblTimer.Content = d.Hour + " : " + d.Minute + " : " + d.Second;
            RefreshLinesTimingOfStationGrid();
        }
        void RefreshLinesTimingOfStationGrid()
        {
            DateTime d = DateTime.Now;
            dgLineTiming.DataContext = bl.GetAllLinesTimingOfStation(sta.Code, new TimeSpan(d.Hour, d.Minute, d.Second));
        }

        private void cbStationID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sta = (cbStationID.SelectedItem as BO.Station);
            

            if (sta != null)
            {
                //list of lines timing of selected station
                RefreshLinesTimingOfStationGrid();
            }
        }

       
    }
}

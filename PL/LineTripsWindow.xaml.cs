using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for LinesTripsWindow.xaml
    /// </summary>
    public partial class LineTripsWindow : Window
    {
        IBL bl;
        BO.Line curLine;
        DispatcherTimer Timer = new DispatcherTimer();
        public LineTripsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            cbLineID.SelectedValuePath = "ID";//selection return only specific Property of object
            RefreshAllLinesComboBox();

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            lblTimer.Content = d.Hour + " : " + d.Minute + " : " + d.Second;
        }
        void RefreshAllLinesComboBox()
        {
            cbLineID.DataContext = bl.GetAllLines();
            cbLineID.SelectedIndex = 0; //index of the object to be selected
        }

        //list of departures of selected line
        void RefreshAllLineTripsGrid()
        {
            dgLineTrips.DataContext = bl.GetAllLineTrips(curLine.ID);
        }


        private void cbLineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curLine = (cbLineID.SelectedItem as BO.Line);

            if (curLine != null)
                RefreshAllLineTripsGrid();
        }

        private void btAddTrip_Click(object sender, RoutedEventArgs e)
        {
            AddLineTrip win = new AddLineTrip(bl);
            win.Closing += WinAddLineTrip_Closing;
            win.ShowDialog();
        }

        private void WinAddLineTrip_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllLineTripsGrid();
            RefreshAllLinesComboBox();
        }
    }
}

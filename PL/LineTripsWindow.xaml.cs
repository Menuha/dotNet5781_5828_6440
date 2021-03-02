using BLAPI;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for LinesTripsWindow.xaml
    /// </summary>
    public partial class LineTripsWindow : Window
    {
        IBL bl;
        BO.Line curLineTrips;
        DispatcherTimer Timer = new DispatcherTimer();
        public LineTripsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            cbLineID.SelectedValuePath = "LineID";//selection return only specific Property of object
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


        private void cbLineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curLineTrips = (cbLineID.SelectedItem as BO.Line);
            //gridOneLine.DataContext = curLine;

            if (curLineTrips != null)
            {

                ////list of stations of selected line
                //RefreshAllStationsOfLineGrid();
                ////list of all stations (that selected line is not registered to it)
                //RefreshAllOtherStationsGrid();
            }
        }
    }
}

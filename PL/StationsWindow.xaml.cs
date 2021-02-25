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
using System.Windows.Shapes;

using BLAPI;

namespace PL
{

    /// <summary>
    /// Interaction logic for StationsWindow.xaml
    /// </summary>
    public partial class StationsWindow : Window
    {
        IBL bl;
        BO.Station sta;
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            cbStationID.DisplayMemberPath = "Name";
            cbStationID.SelectedItem = "Code";
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            RefreshAllStationComboBox();

            dgLinesOfStation.IsReadOnly = true;
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            lblTimer.Content = d.Hour + " : " + d.Minute + " : " + d.Second;
        }
        void RefreshAllStationComboBox()
        {
            cbStationID.DataContext = bl.GetAllStations();
            cbStationID.SelectedIndex = 0; //index of the object to be selected
        }
        
        void RefreshAllLinesOfStationGrid()
        {
            dgLinesOfStation.DataContext = bl.GetAllLinesOfStation(sta.Code);
        }

        //void RefreshAllOtherLinesGrid()
        //{
        //    List<BO.Line> listOfOtherLines = bl.GetAllLines().Where(l1 => bl.GetAllLinesOfStation(sta.Code).All(l2 => l2.ID != l1.ID)).ToList();
        //    dgOtherLines.DataContext = listOfOtherLines;
        //}

        private void cbStationID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sta = (cbStationID.SelectedItem as BO.Station);
            gridOneStation.DataContext = sta;

            if (sta != null)
            {
                //list of lines of selected station
                RefreshAllLinesOfStationGrid();
                //
            }
        }

        private void btUpStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((codeTextBox.Text.Length) > 5 || codeTextBox.Text.Length < 5)
                {
                    MessageBoxResult rol = MessageBox.Show("Press station code with 5 nunbers only!");
                    if (rol == MessageBoxResult.OK)
                        return;
                }
                if (sta.Latitude < 30 || sta.Latitude > 34)
                {
                    MessageBoxResult res2 = MessageBox.Show("Press latitude in Israel only!");
                    if (res2 == MessageBoxResult.OK)
                        return;
                }

                if (sta.Longitude < 30 || sta.Longitude > 34)
                {
                    MessageBoxResult res3 = MessageBox.Show("Press longitude in Israel only!");
                    if (res3 == MessageBoxResult.OK)
                        return;
                }
                else
                if (sta != null)
                    bl.UpdateStation(sta);
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btDelStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Delete selected station?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;

            try
            {
                if (sta != null)
                {
                    bl.DeleteStation(sta.Code);

                    RefreshAllStationComboBox();
                  //
                    RefreshAllLinesOfStationGrid();
                }
            }
            catch (BO.BadLineIDStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btAddStation_Click(object sender, RoutedEventArgs e)
        {
            AddStation thirdWindow = new AddStation(bl);
            thirdWindow.Closing += WinAddStation_Closing;
            thirdWindow.ShowDialog();
        }

        private void WinAddStation_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //RefreshAllLinesOfStationGrid();
            ////
            //RefreshAllStationComboBox();
            RefreshAllStationComboBox();
            //
            RefreshAllLinesOfStationGrid();
        }

    }
}

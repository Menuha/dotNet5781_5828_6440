using BLAPI;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsWindow.xaml
    /// </summary>
    public partial class StationsWindow : Window
    {
        IBL bl;
        BO.Station sta;
        //ObservableCollection<BO.Station> listStations;
        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            cbStationID.DisplayMemberPath = "StationAdress";
            cbStationID.SelectedItem = "Code";
            RefreshAllStationComboBox();

            gridLinesOfStation.IsReadOnly = true;
        }

        private void cbStationID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sta = (cbStationID.SelectedItem as BO.Station);
            gridOneStation.DataContext = sta;

            if (sta != null)
            {
                //list of lines of selected station
                RefreshAllLinesOfStationGrid();
            }
        }
        void RefreshAllStationComboBox()
        {
            cbStationID.DataContext = bl.GetAllStations();
            cbStationID.SelectedIndex = 0; //index of the object to be selected
        }
        void RefreshAllLinesOfStationGrid()
        {
            gridLinesOfStation.DataContext = bl.GetAllLinesOfStation(sta.Code);
        }


    }
}

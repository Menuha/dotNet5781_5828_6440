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
    /// Interaction logic for Station.xaml
    /// </summary>
    public partial class StationsWindow : Window
    {
        IBL bl;
        //ObservableCollection<BO.Station> listStation;
        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            //listStation = (ObservableCollection<BO.Station>)bl.GetAllStations();
            //cbStationAdd.DataContext = listStation;
            //cbStationAdd.DisplayMemberPath = "StationAdress";
            //cbStationAdd.SelectedItem = "Code";
        }
    }
}

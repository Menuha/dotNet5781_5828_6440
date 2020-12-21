//Menuha Peleg 208095828
//Shira Cohen 207486440
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
using static dotNet5781_03B_5828_6440.Bus3;

namespace dotNet5781_03B_5828_6440
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        List<Bus3> busList;
        public MainWindow()
        {
            InitializeComponent();
            createBusList();
            bus3DataGrid.DataContext = busList;
            bus3DataGrid.IsReadOnly = true;
        }
        void createBusList() 
        {
            busList = new List<Bus3>
            {
                new Bus3
                {
                    LicenseNum ="12-345-67",
                    FirstDate=DateTime.Parse("1.1.2013"),
                    Kilometrage=40000,
                    Gas=111,
                    LastTreatDate=DateTime.Parse("1.5.2020"),
                    LastTreatKm=0
                },
                new Bus3
                {
                    LicenseNum ="12-345-88",
                    FirstDate=DateTime.Parse("1.2.2010"),
                    Kilometrage=50000,
                    Gas=500,
                    LastTreatDate=DateTime.Parse("1.1.2020"),
                    LastTreatKm=19950
                },
                new Bus3
                {
                    LicenseNum ="12-345-99",
                    FirstDate=DateTime.Parse("1.1.2010"),
                    Kilometrage=70000,
                    Gas=1190,
                    LastTreatDate=DateTime.Parse("1.8.2020"),
                    LastTreatKm=12000
                },
                new Bus3
                {
                    LicenseNum ="123-55-111",
                    FirstDate=DateTime.Parse("1.3.2019"),
                    Kilometrage=27000,
                    Gas=100,
                    LastTreatDate=DateTime.Parse("1.1.2020"),
                    LastTreatKm=7500
                },
                new Bus3
                {
                    LicenseNum ="56-345-66",
                    FirstDate=DateTime.Parse("1.1.2017"),
                    Kilometrage=30000,
                    Gas=230,
                    LastTreatDate=DateTime.Parse("1.7.2020"),
                    LastTreatKm=700
                },
                new Bus3
                {
                    LicenseNum ="22-245-67",
                    FirstDate=DateTime.Parse("1.1.2016"),
                    Kilometrage=30000,
                    Gas=567,
                    LastTreatDate=DateTime.Parse("1.1.2020"),
                    LastTreatKm=7800
                },
                new Bus3
                {
                    LicenseNum ="124-45-671",
                    FirstDate=DateTime.Parse("1.1.2019"),
                    Kilometrage=4000,
                    Gas=191,
                    LastTreatDate=DateTime.Parse("1.1.2020"),
                    LastTreatKm=7900
                },
                new Bus3
                {
                    LicenseNum ="12-345-45",
                    FirstDate=DateTime.Parse("1.7.2011"),
                    Kilometrage=80000,
                    Gas=987,
                    LastTreatDate=DateTime.Parse("1.11.2019"),
                    LastTreatKm=15000
                },
                new Bus3
                {
                    LicenseNum ="12-135-88",
                    FirstDate=DateTime.Parse("1.1.2008"),
                    Kilometrage=120000,
                    Gas=100,
                    LastTreatDate=DateTime.Parse("1.1.2020"),
                    LastTreatKm=12000
                },
                new Bus3
                {
                    LicenseNum ="121-34-677",
                    FirstDate=DateTime.Parse("1.2.2018"),
                    Kilometrage=2700,
                    Gas=1000,
                    LastTreatDate=DateTime.Parse("1.2.2020"),
                    LastTreatKm=2700
                }
            };
        }
    }
}

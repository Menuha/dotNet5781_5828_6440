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

using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for Lines.xaml
    /// </summary>
    public partial class LinesWindow : Window
    {
        IBL bl;
        BO.Line curLine;
        public LinesWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            areaComboBox.ItemsSource = Enum.GetValues(typeof(BO.Areas));

            RefreshAllLineComboBox();
            cbLineID.SelectedValuePath = "ID";//selection return only specific Property of object
            dgStationsOfLine.IsReadOnly = true;
            dgOtherStations.IsReadOnly = true;

        }

        void RefreshAllLineComboBox()
        {
            cbLineID.DataContext = bl.GetAllLines();
        }

        void RefreshAllStationsOfLineGrid()
        {
            dgStationsOfLine.DataContext = bl.GetAllStationsOfLine(curLine.ID);
        }
       
        void RefreshAllOtherStationsGrid()
        {
            List<BO.Station> listOfOtherStations = bl.GetAllStations().Where(s1 => bl.GetAllStationsOfLine(curLine.ID).All(s2 => s2.StationCode != s1.Code)).ToList();
            dgOtherStations.DataContext = listOfOtherStations;
        }

        private void cbLineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curLine = (cbLineID.SelectedItem as BO.Line);
            gridOneLine.DataContext = curLine;

            if (curLine != null)
            {
                //list of stations of selected line
                RefreshAllStationsOfLineGrid();
                //list of all stations (that selected line is not registered to it)
                RefreshAllOtherStationsGrid();
            }

        }

        private void btUpdateLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (curLine != null)
                    bl.UpdateLine(curLine);
            }
            catch (BO.BadLineIDException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btDeleteLine_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Delete selected line?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;

            try
            {
                if (curLine != null)
                {
                    bl.DeleteLine(curLine.ID);

                    RefreshAllLineComboBox();
                    RefreshAllOtherStationsGrid();
                    RefreshAllStationsOfLineGrid();
                }
            }
            catch (BO.BadLineIDStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadLineIDException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btAddLine_Click(object sender, RoutedEventArgs e)
        {
            AddLine win = new AddLine(bl);
            win.Closing += WinAddLine_Closing;
            win.ShowDialog();
        }

        private void WinAddLine_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshAllLineComboBox();
            RefreshAllOtherStationsGrid();
            RefreshAllStationsOfLineGrid();
        }

        private void btUnRegisterStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.StationOfLine solBO = ((sender as Button).DataContext as BO.StationOfLine);
                bl.DeleteStationOfLine(curLine.ID, solBO.StationCode);
                RefreshAllOtherStationsGrid();
                RefreshAllStationsOfLineGrid();
            }
            catch (BO.BadLineIDStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btRegisterStation_Click(object sender, RoutedEventArgs e)
        {
            if (curLine == null)
            {
                MessageBox.Show("You must select a line first", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                BO.Station sBO = ((sender as Button).DataContext as BO.Station);
                bl.AddStationOfLine(curLine.ID, sBO.Code);

                RefreshAllOtherStationsGrid();
                RefreshAllStationsOfLineGrid();
            }
            catch (BO.BadLineIDStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}

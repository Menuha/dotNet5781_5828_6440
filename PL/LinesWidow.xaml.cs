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
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public LinesWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            areaComboBox.ItemsSource = Enum.GetValues(typeof(BO.Areas));

            cbLineID.SelectedValuePath = "ID";//selection return only specific Property of object
            RefreshAllLineComboBox();
           
            dgStationsOfLine.IsReadOnly = true;
            dgOtherStations.IsReadOnly = true;

        }
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            lblTimer.Content = d.Hour + " : " + d.Minute + " : " + d.Second;
        }
        void RefreshAllLineComboBox()
        {
            cbLineID.DataContext = bl.GetAllLines();
            cbLineID.SelectedIndex = 0; //index of the object to be selected
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

                    RefreshAllStationsOfLineGrid();
                    RefreshAllOtherStationsGrid();
                    RefreshAllLineComboBox();
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
            RefreshAllStationsOfLineGrid();
            RefreshAllOtherStationsGrid();
            RefreshAllLineComboBox();
        }

        private void btUpdateIndexInLine_Click(object sender, RoutedEventArgs e)
        {
            BO.StationOfLine solBO = ((sender as Button).DataContext as BO.StationOfLine);
            int routeLength = bl.GetAllStationsOfLine(curLine.ID).Count();
            IndexWindow win = new IndexWindow(solBO, routeLength);
            win.Closing += WinUpdateIndex_Closing;
            win.ShowDialog();
        }

        private void WinUpdateIndex_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BO.StationOfLine solBO = (sender as IndexWindow).curSolBO;

            MessageBoxResult res = MessageBox.Show("Update index for selected line?", "Verification", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
            {
                (sender as IndexWindow).cbIndex.Text = (sender as IndexWindow).indexBeforeUpdate.ToString();
            }
            else if (res == MessageBoxResult.Cancel)
            {
                (sender as IndexWindow).cbIndex.Text = (sender as IndexWindow).indexBeforeUpdate.ToString();
                e.Cancel = true; //window stayed open. cancel closing event.
            }
            else
            {
                try
                {
                    bl.UpdateStationIndexInLine(curLine.ID, solBO.StationCode, solBO.StationIndexInLine);
                    RefreshAllStationsOfLineGrid();
                }
                catch (BO.BadLineIDStationCodeException ex)
                {
                    MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
       
        private void btUnRegisterStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.StationOfLine solBO = ((sender as Button).DataContext as BO.StationOfLine);
                bl.DeleteStationOfLine(curLine.ID, solBO.StationCode);

                RefreshAllStationsOfLineGrid();
                RefreshAllOtherStationsGrid();
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

                RefreshAllStationsOfLineGrid();
                RefreshAllOtherStationsGrid();
            }
            catch (BO.BadLineIDStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}

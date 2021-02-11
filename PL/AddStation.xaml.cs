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
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        IBL bl;
        BO.Station newSta = new BO.Station();
        public AddStation(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            gridNewStation.DataContext = newSta;


        }

        private void btContinue_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Add stationt?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;

            //code missing:
            //test each textbox, combobox value with more TKINUT KELET etc.
            // make sure each field has value
            //if not takin shoe message box, and return.
            //else
            try
            {
                bl.AddStation(newSta);
                this.Close();
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void My_Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                btContinue.IsEnabled = false; //e.Error.Exception.Message;
            else
                btContinue.IsEnabled = true; ; //errorMessages.Remove(e.Error.Exception.Message);
        }
    }
}

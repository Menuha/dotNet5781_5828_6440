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

            newSta.Latitude = int.Parse(latitudeTextBox.Text);
            newSta.Longitude = int.Parse(longitudeTextBox.Text);
            //newSta.Name = nameTextBox.Text;
           
            try
            {
                if ((codeTextBox.Text.Length) > 5 || codeTextBox.Text.Length < 5)
                {
                    MessageBoxResult rol = MessageBox.Show("Press station code with 5 nunbers only!");
                    if (rol == MessageBoxResult.OK)
                        return;
                }
                else if (newSta.Latitude < 30 || newSta.Latitude > 34)
                    MessageBox.Show("Press latitude in Israel only!");

                else if (newSta.Latitude < 30 || newSta.Latitude > 34)
                    MessageBox.Show("Press latitude in Israel only!");

                //else if (newSta.Name < 'A' || newSta.Name > 34)
                //    MessageBox.Show("Press latitude in Israel only!");

                else
                {
                    bl.AddStation(newSta);
                    this.Close();
                }
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
        private void codeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(codeTextBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                codeTextBox.Text = codeTextBox.Text.Remove(codeTextBox.Text.Length - 1);
            }
        }
        private void latitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(latitudeTextBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                latitudeTextBox.Text = latitudeTextBox.Text.Remove(latitudeTextBox.Text.Length - 1);
            }
        }
        private void longitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(longitudeTextBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                longitudeTextBox.Text = longitudeTextBox.Text.Remove(longitudeTextBox.Text.Length - 1);
            }
        }
        //private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (System.Text.RegularExpressions.Regex.IsMatch(longitudeTextBox.Text, "[^A-z]"))
        //    {
        //        MessageBox.Show("Please enter only charts.");
        //        longitudeTextBox.Text = longitudeTextBox.Text.Remove(longitudeTextBox.Text.Length - 1);
        //    }
        //}
    }
}

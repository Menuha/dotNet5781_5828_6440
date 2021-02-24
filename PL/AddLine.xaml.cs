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
    /// Interaction logic for AddLine.xaml
    /// </summary>
    public partial class AddLine : Window
    {
        IBL bl;
        BO.Line newLine = new BO.Line();
        public AddLine(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            gridNewLine.DataContext = newLine;

            areaComboBox.ItemsSource = Enum.GetValues(typeof(BO.Areas));
           
        }

        private void btContinue_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Add line?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;

            //code missing:
            //test each textbox, combobox value with more TKINUT KELET etc.
            // make sure each field has value
            //if not takin shoe message box, and return.
            //else
            try
            {
                bl.AddLine(newLine);
                this.Close();
            }
            catch (BO.BadLineIDException ex)
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

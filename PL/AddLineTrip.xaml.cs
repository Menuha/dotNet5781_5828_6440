using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddLineTrip.xaml
    /// </summary>
    public partial class AddLineTrip : Window
    {
        IBL bl;
        BO.LineTrip newLT = new BO.LineTrip();
        public AddLineTrip(IBL _bl, int lineID)
        {
            InitializeComponent();
            bl = _bl;
            newLT.LineID = lineID;

            gridLT.DataContext = newLT;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                newLT.StartAt = TimeSpan.Parse(startAtTextBox.Text);

                string pattern = @"\d\d:\d\d:\d\d$";
                Regex rgx = new Regex(pattern);

                if (!rgx.IsMatch(startAtTextBox.Text))
                {
                    MessageBox.Show("Please enter time in the right pattern.");
                    startAtTextBox.Text = startAtTextBox.Text.Remove(0);
                }

                else
                {
                    bl.AddLineTrip(newLT.LineID, newLT.StartAt);
                    this.Close();
                }
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

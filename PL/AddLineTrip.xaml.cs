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
        public AddLineTrip(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            tSpanTextBox.DataContext = newLT;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                newLT.StartAt = TimeSpan.Parse(tSpanTextBox.Text);

                string pattern = @"\d\d:\d\d:\d\d$";
                Regex rgx = new Regex(pattern);

                if (!rgx.IsMatch(tSpanTextBox.Text))
                {
                    MessageBox.Show("Please enter time in the right pattern.");
                    tSpanTextBox.Text = tSpanTextBox.Text.Remove(0);
                }

                else
                {
                    bl.AddLineTrip(newLT);
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

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

namespace PL
{
    /// <summary>
    /// Interaction logic for IndexWindow.xaml
    /// </summary>
    public partial class IndexWindow : Window
    {
        public BO.StationOfLine curSolBO;
        public int indexBeforeUpdate;
        public IndexWindow(BO.StationOfLine solBO, int routeLength)
        {
            InitializeComponent();
            curSolBO = solBO;
            indexBeforeUpdate = solBO.StationIndexInLine;

            for (int i = 1; i <= routeLength; i++)
            {
                cbIndex.Items.Add(i);
            }

            DataContext = curSolBO;
        }

        private void btUpdateIndex_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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

namespace WpfAppRAP
{
    /// <summary>
    /// Interaction logic for LevelUserControl1.xaml
    /// </summary>
    public partial class LevelUserControl1 : UserControl
    {
        //public static ResearcherController rc;
        public LevelUserControl1()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                Level l = (Level)e.AddedItems[0];
                ResearcherController.filterLevel(l);
            }
        }
    }
}

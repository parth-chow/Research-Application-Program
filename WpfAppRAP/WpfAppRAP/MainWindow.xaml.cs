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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string STAFF_LIST_KEY = "staffList";
        private ResearcherController rc;
        
        public MainWindow()
        {
            InitializeComponent();
            rc = (ResearcherController)(Application.Current.FindResource(STAFF_LIST_KEY) as ObjectDataProvider).ObjectInstance;
            
            
        }

        private void ResearcherListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            
            if (e.AddedItems.Count > 0)
            {
                detailspanel.DataContext = e.AddedItems[0];
            }

        }

        private void Levelcombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Researcher r = new Researcher();
            string text = e.AddedItems[0].ToString();
            MessageBox.Show("The text entered is: " + text);
            //List<Researcher> filtered=rc.filterLevel(text);
            Enum.TryParse(text, out Level myStatus);
            //rc.Filter(text);
            ResearcherController.filterLevel(myStatus);
            //ResearcherListView.Items.Add(filtered);
            //List<Publication> plist = r.pubwork;
            //PublicationListView.Items.Add("String");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Supervisionwindow swindow = new Supervisionwindow();
            //swindow.suplistbox.Ad
            swindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Researcher r = new Researcher();
            poorReportWindow prw = new poorReportWindow(e);
            prw.Show();
            //prw.poorlistbox.Items.Add(r.poor);
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //CumulateCount cc = new CumulateCount();
            //cc.Show();
            foreach (Researcher r in ResearcherController.res) {
                //CumulateDatagrid.ItemsSource = r.Year;
                    }
        }
        private void sampleTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_4(sender, e);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            foreach (Researcher r in ResearcherController.res)
            {
                Researcher.sortpublistascending(r);
            }
            //SelectionChangedEventArgs d;
            //ResearcherListView_SelectionChanged_1(sender, d);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            rc.searchName(sampleTextBox.Text);
        }

        private void PublicationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PublicationWindow pw = new PublicationWindow(e);
            pw.Show();


        }
        /*private BitmapMetadata ConvertImage()
{
foreach (Researcher r in ResearcherController.res)
{
BitmapImage image = new BitmapImage();
image.BeginInit();
image.UriSource = new Uri(r.Photo);
image.EndInit();

return image;
}
return null;
}*/
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MHB.Gadgets.MhbApiService;

namespace MHB.Gadgets.Controls
{
    /// <summary>
    /// Interaction logic for VisitorsMap.xaml
    /// </summary>
    public partial class VisitorsMap : UserControl
    {
        public ObservableCollection<ActionLog> ActionLogs
        {
            get { return (ObservableCollection<ActionLog>)GetValue(ActionLogsProperty); }
            set { SetValue(ActionLogsProperty, value); }
        }

        public static readonly DependencyProperty ActionLogsProperty =
            DependencyProperty.Register("ActionLogs", typeof(ObservableCollection<ActionLog>), typeof(VisitorsMap), null);

        public VisitorsMap()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void LoadCities()
        {
            if (this.ActionLogs != null)
            {
                foreach (City c in Helper.FindVisualChildren<City>(CanvasMap))
                {
                    CanvasMap.Children.Remove(c);
                }

                foreach (ActionLog log in this.ActionLogs.Where(a => !string.IsNullOrEmpty(a.City)).ToArray())
                {
                    City city = new City();
                    city.SetValue(Canvas.LeftProperty, log.RelativeCoordinates.m_Item1);
                    city.SetValue(Canvas.TopProperty, log.RelativeCoordinates.m_Item2);
                    city.CityName = log.City;

                    var aa = this.ActionLogs.Where(a => a.City == log.City).ToArray();

                    var b = aa.GroupBy(al => al.UserEmail).ToArray();

                    var count = b.Select(al => al.First()).Count();

                    city.VisitorsCount = this.ActionLogs.Where(al => al.City == log.City && !string.IsNullOrEmpty(al.UserEmail)).GroupBy(al => al.UserEmail).Select(al => al.First()).Count();
                    city.Radius = 13;

                    CanvasMap.Children.Add(city);
                }
            }
        }
    }
}
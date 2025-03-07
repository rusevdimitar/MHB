using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MHB.Gadgets.Controls
{
    /// <summary>
    /// Interaction logic for City.xaml
    /// </summary>
    public partial class City : UserControl
    {
        public int VisitorsCount
        {
            get { return (int)GetValue(VisitorsCountProperty); }
            set { SetValue(VisitorsCountProperty, value); }
        }

        public static readonly DependencyProperty VisitorsCountProperty =
            DependencyProperty.Register("VisitorsCount", typeof(int), typeof(City), new UIPropertyMetadata(0));

        public string CityName
        {
            get { return (string)GetValue(CityNameProperty); }
            set { SetValue(CityNameProperty, value); }
        }

        public static readonly DependencyProperty CityNameProperty =
            DependencyProperty.Register("CityName", typeof(string), typeof(City), new UIPropertyMetadata(string.Empty));

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(City), new UIPropertyMetadata(0));

        public City()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
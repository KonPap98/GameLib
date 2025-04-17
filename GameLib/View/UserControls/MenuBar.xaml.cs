using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GameLib.View.UserControls
{
    /// <summary>
    /// Interaction logic for usew.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
        }

        private void Searchbt_Click(object sender, RoutedEventArgs e)
        {
            SearchText(SearchBox.Text);
        }
        void SearchText(string text) => Trace.WriteLine("Searched for: " + text);
    }
}

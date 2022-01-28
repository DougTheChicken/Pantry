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

namespace inventory
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // simple function to test knowledge on
        // event handlers in the GUI
        private void btn_QuickAdd(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(item.Text) && !log.Items.Contains(item.Text) &&
                !string.IsNullOrWhiteSpace(type.Text) && !string.IsNullOrWhiteSpace(quantity.Text))
            {
                log.Items.Add(item.Text + "," + type.Text + "," + quantity.Text);
                item.Clear();
                type.Clear();
                quantity.Clear();
            }
        }
    }
}

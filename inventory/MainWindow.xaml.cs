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
using System.IO;

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
                // get current date
                DateTime currentDate = DateTime.Now;

                log.Items.Add(item.Text + "," + type.Text + "," + quantity.Text);
                QuickAdd(currentDate.ToString("d"), currentDate.ToString("t"),
                            item.Text, type.Text, quantity.Text);

                item.Clear();
                type.Clear();
                quantity.Clear();
            }
        }

        private static void CheckForFile(string name)
        {
            // check if file exists
            // create file if it doesn't exist
            if (!File.Exists(name))
                File.Create(name).Close();
        }

        private static void QuickAdd(string date, string time, string item, string type, string quantity)
        {
            // read through file to see if item is already in file
            int index = 0;
            int id = 1;
            string previousCatagory = "null";   // last read item's catagory
            bool update = false;                // true if line needs to be updated
            string addLine = "null";            // line to add to log

            // check if log file exists
            // if does not, then one is created
            CheckForFile("log.csv");

            // read all lines from file to index through
            string[] lines = File.ReadAllLines("log.csv");

            // cycle through each line to check if input is an update
            foreach (string line in lines)
            {
                // seperate the current comma delimited line
                string[] bits = line.Split(new char[1] { ',' });

                // compare the item name and type between the line from
                // the file and the input provided by the user
                // if they are the same, then update quantity
                if (String.Compare(bits[5], item, true) == 0 &&
                    String.Compare(bits[4], type, true) == 0)
                {
                    addLine = UpdateQuantity(lines, item, type, quantity, date, time);
                    update = true;
                    break;
                }

                index++;

                // compare line and input types to determine item id
                // if they are the same, then the item id will be
                // the same as the ids in its catagory but incremented
                // by one

                // if they are different, then check if the current
                // catagory is the same as the last one
                // if they aren't, then increment id by one
                if (String.Compare(type, bits[4], true) == 0)
                    id = Int32.Parse(bits[0]) + 1;
                else if (String.Compare(previousCatagory, bits[4], true) != 0 &&
                            (Int32.Parse(bits[0]) / 1000) == id && id < 1000)
                    id++;

                previousCatagory = bits[4];
            }

            // set id to proper unit place for new catagory
            if (id < 1000)
                id = id * 1000;

            // write item to file
            using (StreamWriter file = File.AppendText("log.csv"))
            {
                if (!update)
                {
                    addLine = id.ToString() + ',' + id.ToString() + "000001" + ',' +
                                date + ',' + time + ',' + type + ',' + item + ',' + quantity;
                }
                file.WriteLine(addLine);
            }
            return;                             // end function
        }

        private static string UpdateQuantity(string[] lines, string item, string type, string quantity,
                                                string date, string time)
        {
            int index = 0;
            int entry = 0;
            string[] compDate = { "00", "00", "00" };
            string[] compTime = { "00", "00" };
            string[] entryDate;
            string[] entryTime;
            string[] bits;

            foreach (string line in lines)
            {
                bits = line.Split(new char[1] { ',' });
                if (String.Compare(bits[5], item, true) == 0 &&
                        String.Compare(bits[4], type, true) == 0)
                {
                    entryDate = bits[2].Split(new char[1] { '/' });
                    entryTime = bits[3].Split(new char[1] { ':' });

                    // compare years
                    if (Int32.Parse(compDate[2]) <=
                            Int32.Parse(entryDate[2]))
                    {
                        // compare months
                        if (Int32.Parse(compDate[0]) <=
                                Int32.Parse(entryDate[0]))
                        {
                            // compare days
                            if (Int32.Parse(compDate[1]) <=
                                    Int32.Parse(entryDate[1]))
                            {
                                // compare hour
                                if (Int32.Parse(compTime[0]) <=
                                        Int32.Parse(entryTime[0]))
                                {
                                    // compare minutes
                                    if (Int32.Parse(compTime[1]) <=
                                            Int32.Parse(entryTime[1]))
                                    {
                                        compDate[0] = entryDate[0];
                                        compDate[1] = entryDate[1];
                                        compDate[2] = entryDate[2];
                                        compTime[0] = entryTime[0];
                                        compTime[1] = entryTime[1];
                                        entry = index;
                                    }
                                }
                            }
                        }
                    }
                }
                index++;
            }

            bits = lines[entry].Split(new char[1] { ',' });
            float updatedQuantity = float.Parse(bits[6]) + float.Parse(quantity);
            string addLine = bits[0] + ',' + bits[1] + ',' + date +
                                ',' + time + ',' + bits[4] + ',' +
                                bits[5] + ',' + updatedQuantity.ToString();
            return addLine;
        }
    }
}

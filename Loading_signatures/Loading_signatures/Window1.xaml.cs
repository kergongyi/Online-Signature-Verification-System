using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Loading_signatures
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        double trace = 0;
        public Window1()
        {
            InitializeComponent();
        }

        private void Verification_Click(object sender, RoutedEventArgs e)
        {
            if(Resultlist.Items.Count > 0)
            {
                Resultlist.Items.Clear();
            }

            string signer;
            int sign1;
            int sign2;
            int index = 0;
            double[] Reference_x = new double[90];
            double[] Reference_y = new double[90];
            double[] test_x = new double[10];
            double[] test_y = new double[10];
            double[] temp = new double[2];
            if (Signer_system.SelectedIndex != -1)
            {
                signer = Signer_system.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                string[] u_value = signer.Split(" ");
                for (int i = 1; i <= 10; i++)
                {
                    for (int j = 1; j <= 10; j++)
                    {
                        sign1 = i;
                        sign2 = j;
                        if (i != j)
                        {
                            temp = DTWCalculate(u_value[1], sign1.ToString(), sign2.ToString());
                            Reference_x[index] = temp[0];
                            Reference_y[index] = temp[1];
                            index = index + 1;
                        }
                    }
                }

                double Reference_x_total = 0;
                double Reference_y_total = 0;
                for (int i = 0; i < 90; i++)
                {
                    Reference_x_total = Reference_x_total + Reference_x[i];
                    Reference_y_total = Reference_y_total + Reference_y[i];
                }
                double threshold_x = Reference_x_total / Reference_x.Length;
                double threshold_y = Reference_y_total / Reference_y.Length;

                Resultlist.Items.Add("threshold_x: " + threshold_x.ToString() + "    threshold_y: " + threshold_y.ToString());

                for(int j = 11; j <= 40; j++)
                {
                    index = 0;

                    for (int i = 1; i <= 10; i++)
                    {
                        sign1 = i;
                        sign2 = j;

                        temp = DTWCalculate(u_value[1], sign1.ToString(), sign2.ToString());
                        test_x[index] = temp[0];
                        test_y[index] = temp[1];
                        index = index + 1;
                    }

                    double test_x_total = 0;
                    double test_y_total = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        test_x_total = test_x_total + test_x[i];
                        test_y_total = test_y_total + test_y[i];
                    }
                    double average_x = test_x_total / test_x.Length;
                    double average_y = test_y_total / test_y.Length;

                    string state;
                    if (average_x <= threshold_x && average_y <= threshold_y)
                    {
                        state = "accept";
                    }
                    else
                    {
                        state = "reject";
                    }

                    Resultlist.Items.Add(j.ToString() + ":Average_x: " + average_x + "    Average_y: " + average_y + "    " + state);
                }
            }
            else
            {
                MessageBox.Show("Please do not forget to select sign");
            }
        }

        private double[] DTWCalculate(string signer, string sign1, string sign2)
        {
           double[] First_x;
           double[] First_y;
           double[] Second_x;
           double[] Second_y;

           int i = 0;
           int j = 0;
           int length_1 = 0;
           int length_2 = 0;
           double base_x1 = 0;
           double base_y1 = 0;
           double base_x2 = 0;
           double base_y2 = 0;
           double totalcost_x = 0;
           double totalcost_y = 0;

            string filename = "U" + signer + "S" + sign1 + ".TXT";
            string filename2 = "U" + signer + "S" + sign2 + ".TXT";
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string length1 = sr.ReadLine();
                    int.TryParse(length1, out length_1);
                    First_x = new double[length_1];
                    First_y = new double[length_1];
                    while (!sr.EndOfStream)
                    {
                        string[] location = sr.ReadLine().Split(" ");

                        double.TryParse(location[0], out First_x[i]);
                        double.TryParse(location[1], out First_y[i]);

                        if (i == 0)
                        {
                            base_x1 = First_x[0];
                            base_y1 = First_y[0];
                        }

                        //Shifting
                        First_x[i] = First_x[i] - base_x1;
                        First_y[i] = First_y[i] - base_y1;

                        i = i + 1;
                    }
                }

                double largestfirst_x = FindLargest(First_x);
                double smallestfirst_x = FindSmallest(First_x);
                for (int i1 = 0; i1 < First_x.Length; i1++)
                {
                    First_x[i1] = 100 * (First_x[i1] - smallestfirst_x) / (largestfirst_x - smallestfirst_x);
                }
                double largestfirst_y = FindLargest(First_y);
                double smallestfirst_y = FindSmallest(First_y);
                for (int i2 = 0; i2 < First_y.Length; i2++)
                {
                    First_y[i2] = 40 * (First_y[i2] - smallestfirst_y) / (largestfirst_y - smallestfirst_y);
                }

                using (StreamReader sr = new StreamReader(filename2))
                {
                    string length2 = sr.ReadLine();
                    int.TryParse(length2, out length_2);
                    Second_x = new double[length_2];
                    Second_y = new double[length_2];
                    while (!sr.EndOfStream)
                    {
                        string[] location = sr.ReadLine().Split(" ");

                        double.TryParse(location[0], out Second_x[j]);
                        double.TryParse(location[1], out Second_y[j]);

                        if (j == 0)
                        {
                            base_x2 = Second_x[0];
                            base_y2 = Second_y[0];
                        }

                        Second_x[j] = Second_x[j] - base_x2;
                        Second_y[j] = Second_y[j] - base_y2;

                        j = j + 1;
                    }
                }

                double largestSecond_x = FindLargest(Second_x);
                double smallestSecond_x = FindSmallest(Second_x);
                for (int i1 = 0; i1 < Second_x.Length; i1++)
                {
                    Second_x[i1] = 100 * (Second_x[i1] - smallestSecond_x) / (largestSecond_x - smallestSecond_x);
                }
                double largestSecond_y = FindLargest(Second_y);
                double smallestSecond_y = FindSmallest(Second_y);
                for (int i2 = 0; i2 < Second_y.Length; i2++)
                {
                    Second_y[i2] = 40 * (Second_y[i2] - smallestSecond_y) / (largestSecond_y - smallestSecond_y);
                }

                //Initialize the cost matrix
                double[,] cost = new double[First_x.Length + 1, Second_x.Length + 1];
                double[,] cost2 = new double[First_y.Length + 1, Second_y.Length + 1];
                for (int x = 0; x < First_x.Length + 1; x++)
                {
                    for (int y = 0; y < Second_x.Length + 1; y++)
                    {
                        cost[x, y] = 0;
                    }
                }
                for (int x = 0; x < First_y.Length + 1; x++)
                {
                    for (int y = 0; y < Second_y.Length + 1; y++)
                    {
                        cost2[x, y] = 0;
                    }
                }
                for (int m = 1; m < First_x.Length + 1; m++)
                {
                    cost[m, 0] = double.MaxValue;
                }
                for (int m = 1; m < First_y.Length + 1; m++)
                {
                    cost2[m, 0] = double.MaxValue;
                }
                for (int n = 1; n < Second_x.Length + 1; n++)
                {
                    cost[0, n] = double.MaxValue;
                }
                for (int n = 1; n < Second_y.Length + 1; n++)
                {
                    cost2[0, n] = double.MaxValue;
                }

                //Fill the cost matrix while keeping traceback information
                double[,] traceback = new double[First_x.Length, Second_x.Length];
                double[,] traceback2 = new double[First_y.Length, Second_y.Length];
                for (int x = 0; x < First_x.Length; x++)
                {
                    for (int y = 0; y < Second_x.Length; y++)
                    {
                        traceback[x, y] = 0;
                    }
                }
                for (int x = 0; x < First_y.Length; x++)
                {
                    for (int y = 0; y < Second_y.Length; y++)
                    {
                        traceback2[x, y] = 0;
                    }
                }
                for (int x = 0; x < First_x.Length; x++)
                {
                    for (int y = 0; y < Second_x.Length; y++)
                    {
                        double minimum = Findminimum(cost[x, y], cost[x, y + 1], cost[x + 1, y]);
                        double distance = Finddistance(First_x[x], Second_x[y]);
                        cost[x + 1, y + 1] = distance + minimum;
                        traceback[x, y] = trace;
                    }
                }
                for (int x = 0; x < First_y.Length; x++)
                {
                    for (int y = 0; y < Second_y.Length; y++)
                    {
                        double minimum = Findminimum(cost2[x, y], cost2[x, y + 1], cost2[x + 1, y]);
                        double distance = Finddistance(First_y[x], Second_y[y]);
                        cost2[x + 1, y + 1] = distance + minimum;
                        traceback2[x, y] = trace;
                    }
                }

                //Traceback from botton right
                int p = First_x.Length - 1;
                int q = Second_x.Length - 1;

                int p2 = First_y.Length - 1;
                int q2 = Second_y.Length - 1;

                ArrayList path_x = new ArrayList();
                ArrayList path_y = new ArrayList();

                while (p > 0 || q > 0)
                {
                    totalcost_x = totalcost_x + cost[p + 1, q + 1];
                    if (traceback[p, q] == 0) //Match
                    {
                        p = p - 1;
                        q = q - 1;
                    }
                    else if (traceback[p, q] == 1) //Insertion
                    {
                        p = p - 1;
                    }
                    else if (traceback[p, q] == 2) // Deletion
                    {
                        q = q - 1;
                    }
                    path_x.Add(p + 1);
                    path_x.Add(q + 1);
                }

                while (p2 > 0 || q2 > 0)
                {
                    totalcost_y = totalcost_y + cost2[p2 + 1, q2 + 1];
                    if (traceback2[p2, q2] == 0) //Match
                    {
                        p2 = p2 - 1;
                        q2 = q2 - 1;
                    }
                    else if (traceback2[p2, q2] == 1) //Insertion
                    {
                        p2 = p2 - 1;
                    }
                    else if (traceback2[p2, q2] == 2) // Deletion
                    {
                        q2 = q2 - 1;
                    }
                    path_y.Add(p2 + 1);
                    path_y.Add(q2 + 1);
                }

                double[] result = new double[2];
                result[0] = totalcost_x;
                result[1] = totalcost_y;
                return result;
            }
            catch (IOException error)
            {
                MessageBox.Show(error.Message);
                return null;
            }
        }
    

        private double Findminimum(double x, double y, double z)
        {
            double minimum = x;
            trace = 0;
            if (minimum > y)
            {
                minimum = y;
                trace = 1;
            }
            if (minimum > z)
            {
                minimum = z;
                trace = 2;
            }
            return minimum;
        }

        private double Finddistance(double x, double y)
        {
            double distance = x - y;
            if (distance > 0)
            {
                return distance;
            }
            else
            {
                return -distance;
            }
        }

        private double FindLargest(double[] list)
        {
            double largestnumber = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i] > largestnumber)
                {
                    largestnumber = list[i];
                }
            }
            return largestnumber;
        }

        private double FindSmallest(double[] list)
        {
            double smallestnumber = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i] < smallestnumber)
                {
                    smallestnumber = list[i];
                }
            }
            return smallestnumber;
        }
    }
}

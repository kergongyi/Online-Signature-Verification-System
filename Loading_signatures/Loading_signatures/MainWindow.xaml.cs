using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

namespace Loading_signatures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double x_scale=1;
        double y_scale=1;
        double x_shift=0;
        double y_shift=0;
        double check=0;
        double trace=0;
        double totalcost_x=0;
        double totalcost_y=0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Draw();
            check=1;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (check==1)
            {
                double.TryParse(Xscale.Value.ToString(), out x_scale);
                this.Redraw();
            }
        }

        private void Yscale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (check==1)
            {
                double.TryParse(Yscale.Value.ToString(), out y_scale);
                this.Redraw();
            }
        }

        private void Xshift_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (check == 1)
            {
                double.TryParse(Xshift.Value.ToString(), out x_shift);
                this.Redraw();
            }
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (check == 1)
            {
                double.TryParse(Yshift.Value.ToString(), out y_shift);
                this.Redraw();
            }
        }

        private void Draw()
        {
            string signer;
            string signature;
            if (Signer.SelectedIndex != -1 && Signature.SelectedIndex != -1)
            {
                signer = Signer.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                signature = Signature.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                string[] u_value = signer.Split(" ");
                string[] s_value = signature.Split(" ");
                double x;
                double y;
                string filename = "U" + u_value[1] + "S" + s_value[1] + ".TXT";
                try
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        Canvas.Children.Clear();
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string[] location = sr.ReadLine().Split(" ");

                            double.TryParse(location[0], out x);
                            double.TryParse(location[1], out y);
                            Ellipse ellipse = new Ellipse();
                            ellipse.Fill = new SolidColorBrush(Colors.Blue);
                            ellipse.Stroke = new SolidColorBrush(Colors.Blue);
                            ellipse.Width = 5;
                            ellipse.Height = 5;
                            ellipse.SetValue(Canvas.ZIndexProperty, 1);
                            ellipse.SetValue(Canvas.LeftProperty, (double)x / 40);
                            ellipse.SetValue(Canvas.TopProperty, (double)208 - (y / 40));
                            Canvas.Children.Add(ellipse);
                        }
                    }
                }
                catch (IOException error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                MessageBox.Show("Please do not forget to select both Signer and Signature");
            }
        }

        private void Redraw()
        {
            string signer;
            string signature;
            if (Signer.SelectedIndex != -1 && Signature.SelectedIndex != -1)
            {
                signer = Signer.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                signature = Signature.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                string[] u_value = signer.Split(" ");
                string[] s_value = signature.Split(" ");
                double x;
                double y;
                string filename = "U" + u_value[1] + "S" + s_value[1] + ".TXT";
                try
                {
                   using (StreamReader sr = new StreamReader(filename))
                    {
                        Canvas2.Children.Clear();
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string[] location = sr.ReadLine().Split(" ");

                            double.TryParse(location[0], out x);
                            double.TryParse(location[1], out y);

                            x=x*x_scale+x_shift;
                            y=y*y_scale+y_shift;

                            Ellipse ellipse = new Ellipse();
                            ellipse.Fill = new SolidColorBrush(Colors.Blue);
                            ellipse.Stroke = new SolidColorBrush(Colors.Blue);
                            ellipse.Width = 5;
                            ellipse.Height = 5;
                            ellipse.SetValue(Canvas.ZIndexProperty, 1);
                            ellipse.SetValue(Canvas.LeftProperty, (double)x / 40);
                            ellipse.SetValue(Canvas.TopProperty, (double)208 - (y / 40));
                            Canvas2.Children.Add(ellipse);
                        }
                    }
                }
                catch (IOException error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                MessageBox.Show("Please do not forget to select both Signer and Signature");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //read coordinates from files
            string signer1;
            string signer2;
            string signature1;
            string signature2;
            double[] First_x;
            double[] First_y;
            double[] Second_x;
            double[] Second_y;
            if (Signer.SelectedIndex != -1 && Signature.SelectedIndex != -1 && Signer2.SelectedIndex != -1 && Signature2.SelectedIndex != -1)
            {
                Canvas3.Children.Clear();
                signer1 = Signer.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                signature1 = Signature.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                signer2 = Signer2.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                signature2 = Signature2.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                string[] u_value = signer1.Split(" ");
                string[] s_value = signature1.Split(" ");
                string[] u_value2 = signer2.Split(" ");
                string[] s_value2 = signature2.Split(" ");
                int i = 0;
                int j = 0;
                int length_1 = 0;
                int length_2 = 0;
                double base_x1 = 0;
                double base_y1 = 0;
                double base_x2 = 0;
                double base_y2 = 0;
                string filename = "U" + u_value[1] + "S" + s_value[1] + ".TXT";
                string filename2 = "U" + u_value2[1] + "S" + s_value2[1] + ".TXT";
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
                            if (Shift.IsChecked == true)
                            {
                                First_x[i] = First_x[i] - base_x1;
                                First_y[i] = First_y[i] - base_y1;
                            }

                            Ellipse ellipse = new Ellipse();
                            ellipse.Fill = new SolidColorBrush(Colors.Blue);
                            ellipse.Stroke = new SolidColorBrush(Colors.Blue);
                            ellipse.Width = 5;
                            ellipse.Height = 5;
                            ellipse.SetValue(Canvas.ZIndexProperty, 1);
                            ellipse.SetValue(Canvas.LeftProperty, (double)First_x[i] / 40);
                            ellipse.SetValue(Canvas.TopProperty, (double)208 - (First_y[i] / 40 + 200));
                            Canvas3.Children.Add(ellipse);
                            i = i + 1;
                        }
                    }
                    if(Scale.IsChecked == true)
                    {
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
                            if(Shift.IsChecked == true)
                            {
                                Second_x[j] = Second_x[j] - base_x2;
                                Second_y[j] = Second_y[j] - base_y2;
                            }

                            Ellipse ellipse = new Ellipse();
                            ellipse.Fill = new SolidColorBrush(Colors.Blue);
                            ellipse.Stroke = new SolidColorBrush(Colors.Blue);
                            ellipse.Width = 5;
                            ellipse.Height = 5;
                            ellipse.SetValue(Canvas.ZIndexProperty, 1);
                            ellipse.SetValue(Canvas.LeftProperty, (double)Second_x[j] / 40);
                            ellipse.SetValue(Canvas.TopProperty, (double)208 - (Second_y[j] / 40 + 80));
                            Canvas3.Children.Add(ellipse);
                            j = j + 1;
                        }
                    }

                    if (Scale.IsChecked == true)
                    {
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

                    DTWY.Text = totalcost_y.ToString();
                    DTWX.Text = totalcost_x.ToString();
                    totalcost_x = 0;
                    totalcost_y = 0;
                }
                catch (IOException error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                MessageBox.Show("Please do not forget to select both Signer and Signature");
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
            for(int i = 1; i < list.Length; i++)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }
    }
}

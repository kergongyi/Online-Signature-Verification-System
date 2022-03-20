﻿using System;
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
            double[] Second_x;
            if (Signer.SelectedIndex != -1 && Signature.SelectedIndex != -1 && Signer2.SelectedIndex != -1 && Signature2.SelectedIndex != -1)
            {
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
                int length_x1 = 0;
                int length_x2 = 0;
                string filename = "U" + u_value[1] + "S" + s_value[1] + ".TXT";
                string filename2 = "U" + u_value2[1] + "S" + s_value2[1] + ".TXT";
                try
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string length1 = sr.ReadLine();
                        int.TryParse(length1, out length_x1);
                        First_x = new double[length_x1];
                        while (!sr.EndOfStream)
                        {
                            string[] location = sr.ReadLine().Split(" ");

                            double.TryParse(location[0], out First_x[i]);
                            i = i + 1;
                        }
                    }

                    using (StreamReader sr = new StreamReader(filename2))
                    {
                        string length2 = sr.ReadLine();
                        int.TryParse(length2, out length_x2);
                        Second_x = new double[length_x2];
                        while (!sr.EndOfStream)
                        {
                            string[] location = sr.ReadLine().Split(" ");

                            double.TryParse(location[0], out Second_x[j]);
                            j = j + 1;
                        }
                    }

                    //Initialize the cost matrix
                    double[,] cost = new double[First_x.Length + 1, Second_x.Length + 1];
                    for (int x = 0; x < First_x.Length + 1; x++)
                    {
                        for (int y = 0; y < Second_x.Length + 1; y++)
                        {
                            cost[x, y] = 0;
                        }
                    }
                    for (int m = 1; m < First_x.Length + 1; m++)
                    {
                        cost[m, 0] = double.MaxValue;
                    }
                    for (int n = 1; n < Second_x.Length + 1; n++)
                    {
                        cost[0, n] = double.MaxValue;
                    }

                    //Fill the cost matrix while keeping traceback information
                    double[,] traceback = new double[First_x.Length, Second_x.Length];
                    for (int x = 0; x < First_x.Length; x++)
                    {
                        for (int y = 0; y < Second_x.Length; y++)
                        {
                            traceback[x, y] = 0;
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

                      //Traceback from botton right
                      int p = First_x.Length - 1;
                      int q = Second_x.Length - 1;
                      
                      while ( p > 0 || q > 0 )
                      {
                          totalcost_x = totalcost_x + cost[p+1,q+1];
                          if (traceback[p, q] == 0) //Match
                          {
                              p = p - 1;
                              q = q - 1;
                          }
                          else if(traceback[p, q] == 1) //Insertion
                          {
                              p = p - 1;
                          }
                          else if(traceback[p, q] == 2) // Deletion
                          {
                              q = q - 1;
                          }
                      }
                    MessageBox.Show(totalcost_x.ToString());
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
    }
}

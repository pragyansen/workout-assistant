using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace Workout_Assitant
{
    public partial class Result : PhoneApplicationPage
    {
        IsolatedStorageSettings settings;
        public Result()
        {
            InitializeComponent();
            double diff,speed,cal = 0;
            int weight;
            settings = IsolatedStorageSettings.ApplicationSettings;
            diff = (double)settings["time"];
            if (diff < 0)
                diff = 0;
            textBlock2.Text = diff.ToString("0.00") + " Min";
            
            double path = (double)settings["path"];
            textBlock4.Text = path.ToString("0.00") + " m";
            speed = (path*3)/(diff*50);
            textBlock6.Text = speed.ToString("0.00") + " Km/hr";

            if (settings.Contains("weight"))
            {
                weight = Convert.ToInt32((string)settings["weight"]);
                if (speed < 3)
                    cal = 0.78 * weight * (path / 1000);
                else if (speed < 6)
                    cal = 0.82 * weight * (path / 1000);
                else if (speed < 9)
                    cal = 0.85 * weight * (path / 1000);
                else
                    cal = 0.9 * weight * (path / 1000);
            }
            else
            {
                MessageBox.Show("Please Update Weight in Profile");
            }


            textBlock8.Text = cal.ToString("0.00") + " KCal";

            string fb;
             fb = "Burnt " + textBlock8.Text + " by running " + path/1000 + " km at speed of " + textBlock6.Text + " for " + textBlock2.Text +" :D";
             settings["fb"] = fb;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/share.xaml", UriKind.Relative));
        }
        
    }
}
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
    public partial class settings : PhoneApplicationPage
    {
        
        public settings()
        {
            InitializeComponent();
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            //int sl;

            if (settings.Contains("EnableLocation"))
                checkBox1.IsChecked = (bool)settings["EnableLocation"];
            else
                settings["EnableLocation"] = true;

            if (settings.Contains("color"))
            {
                int color;
                color = (int)settings["color"];
                if (color == 1)
                {
                    radioButton1.IsChecked = true;
                }
                else if (color == 2)
                {
                    radioButton2.IsChecked = true;
                }
                else if (color == 3)
                {
                    radioButton3.IsChecked = true;
                }
                else if (color == 4)
                {
                    radioButton4.IsChecked = true;
                }
            }
            else
                settings["color"] = 1;

            if (settings.Contains("slider"))
                slider1.Value = Convert.ToDouble(settings["slider"]);
            else
                settings["slider"] = "5";
            if (settings.Contains("map"))
            {
                if ((int)settings["map"] == 1)
                {
                    toggleButton.Content = "Road Mode";
                    toggleButton.IsChecked = true;
                }
                if ((int)settings["map"] == 0)
                {
                    toggleButton.IsChecked = false;
                    toggleButton.Content = "Aerial Mode";
                }
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["color"] = 1;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["color"] = 2;
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["color"] = 3;
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["color"] = 4;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;

            if (slider1 != null)
            {
                settings["slider"] = e.NewValue.ToString();
            }
            
        }

        private void toggleButton_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
                settings["map"] = 1;
                toggleButton.Content = "Road Mode";
        }
        private void toggleButton_UnChecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["map"] = 0;
            toggleButton.Content = "Aerial Mode";
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["EnableLocation"] = true;
        }
        private void checkBox1_UnChecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            settings["EnableLocation"] = false;
        }

        
       
    }
}
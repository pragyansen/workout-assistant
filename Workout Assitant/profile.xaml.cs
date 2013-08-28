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
    public partial class profile : PhoneApplicationPage
    {
        public profile()
        {
            InitializeComponent();
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("name"))
            {
                textBox.Text = (string)settings["name"];
            }
            if (settings.Contains("age"))
            {
                textBox_age.Text = (string)settings["age"];
            }
            if (settings.Contains("height"))
            {
                textBox_height.Text = (string)settings["height"];
            }
            if (settings.Contains("weight"))
            {
                textBox_weight.Text = (string)settings["weight"];
            }

        }
        private void save_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;


            settings["name"] = textBox.Text;
            settings["age"] = textBox_age.Text;
            settings["height"] = textBox_height.Text;
            settings["weight"] = textBox_weight.Text;
            settings.Save();

            MessageBox.Show("Saved");
        }
    }
}
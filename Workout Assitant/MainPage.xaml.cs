using Microsoft.Phone.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System;
using System.Windows;
using Microsoft.Phone.Controls.Maps.Platform;
using System.IO.IsolatedStorage;

namespace Workout_Assitant
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor

       
        MapPolyline _joggingPolyLine;
        IsolatedStorageSettings settings;
        bool start = false, locked = false;
        double path, var; 
            double speed=0;
        GeoCoordinateWatcher watcher;
        Pushpin pin1 = new Pushpin();
        DateTimeOffset time1 = DateTime.Now,time2 = DateTime.Now;
               
        public MainPage()
        {
            InitializeComponent();
            map1.ZoomLevel = 18;
            settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("map"))
            {
                if ((int)settings["map"] == 0)
                {
                    map1.Mode = new AerialMode();
                }
            }
            else
            { 
                map1.Mode = new RoadMode();
            }
            pin1.Location = map1.Center;
            map1.Children.Add(pin1);

            if (settings.Contains("name"))
            {
                if ((string)settings["name"] != "")
                    MessageBox.Show("Welcome back " + (string)settings["name"] + ". Get Ready.");
            }
            else
                MessageBox.Show("Welcome to Workout Assistant. U may consider to create/update your profile in settings->profile");

            if (settings.Contains("EnableLocation"))
            {
                if ((bool)settings["EnableLocation"] == false)
                {
                    MessageBoxResult m = MessageBox.Show("This App cannot work without Location service, Enable Location service?", "Problem Spotted!!", MessageBoxButton.OKCancel);
                    if (m == MessageBoxResult.OK)
                        settings["EnableLocation"] = true;
                    else
                    {
                        if (NavigationService.CanGoBack)
                        {
                            NavigationService.GoBack();
                        }  
                    }

                }
            }
            else
                settings["EnableLocation"] = true;


            StartLocationService(GeoPositionAccuracy.High, true);
           

        }

        
        private void StartLocationService(GeoPositionAccuracy accuracy, bool toggle)
        {
            if (toggle == true) // Reinitialize the GeoCoordinateWatcher
            {
                watcher = new GeoCoordinateWatcher(accuracy);
                settings = IsolatedStorageSettings.ApplicationSettings;
                if (settings.Contains("slider"))
                {
                    watcher.MovementThreshold = Convert.ToDouble(settings["slider"]);
                    path = Convert.ToDouble(settings["slider"]);
                    var = Convert.ToDouble(settings["slider"]);
                }
                else
                {
                    settings["slider"] = 5;
                    path = 5; var = 5;
                }
                // Add event handlers for StatusChanged and PositionChanged events

                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

                // Start data acquisition
                watcher.Start();
            }
            else if (toggle == false)
            {
                // if(watcher.Status)
                watcher.Stop();
                //watcher.Dispose();
            }
        }
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyPositionChanged(e));
        }


        void MyPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // Update the TextBlocks to show the current location
            //textBlock4.Text = e.Position.Location.Latitude.ToString("0.000000000");
            //textBlock5.Text = e.Position.Location.Longitude.ToString("0.000000000");
            map1.Center = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            pin1.Location = e.Position.Location;
            map1.SetView(pin1.Location, 18);
            
            //textBox4.Text = e.Position.Location.VerticalAccuracy.ToString();

            if (e.Position.Location.HorizontalAccuracy < 30 && e.Position.Location.VerticalAccuracy < 30)
            {
                textBlock3.Text = "GPS Locked, Press Start to Begin";
                locked = true;
            }
            else
            {   
                if (locked==false)
                textBlock3.Text = "Please Wait, GPS Locking";
            }

            if (start == true)
            {
                _joggingPolyLine.Locations.Add(new Location() { Latitude = e.Position.Location.Latitude, Longitude = e.Position.Location.Longitude });
                path = path + var;
                if (speed < e.Position.Location.Speed)
                    speed = e.Position.Location.Speed;
                textBlock1.Text = "Distance: " + path.ToString() + " m";
            }

        }

        private void start_Click(object sender, EventArgs e)
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
            _joggingPolyLine = new MapPolyline();
            time1 = watcher.Position.Timestamp;
            if (settings.Contains("color"))
            {
                int color;
                color = (int)settings["color"];
                if (color == 1)
                {
                    _joggingPolyLine.Stroke = new SolidColorBrush(Colors.Red);
                }
                else if (color == 2)
                {
                    _joggingPolyLine.Stroke = new SolidColorBrush(Colors.Blue);
                }
                else if (color == 3)
                {
                    _joggingPolyLine.Stroke = new SolidColorBrush(Colors.Green);
                }
                else if (color == 4)
                {
                    _joggingPolyLine.Stroke = new SolidColorBrush(Colors.Cyan);
                }
            }
            else
            {
                settings["color"] = 2;
                _joggingPolyLine.Stroke = new SolidColorBrush(Colors.Blue);
            }
            
            _joggingPolyLine.StrokeThickness = 5;
            _joggingPolyLine.Opacity = 0.7;
            _joggingPolyLine.Locations = new LocationCollection();
            map1.Children.Add(_joggingPolyLine);
            start = true;
            textBlock3.Text = "Press Stop to finish";
        }

        
        private void stop_Click(object sender, EventArgs e)
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
            StartLocationService(GeoPositionAccuracy.High, false);
            time2 = watcher.Position.Timestamp;
            start = false;
            TimeSpan diff;
            diff = time2 - time1;
            settings["time"] = diff.TotalMinutes;
            settings["path"] = path;
            settings["speed"] = speed;
            if (settings.Contains("weight"))
                NavigationService.Navigate(new Uri("/Result.xaml", UriKind.Relative));
            else
            {
                MessageBox.Show("Please Update Weight in Profile");
                NavigationService.Navigate(new Uri("/profile.xaml", UriKind.Relative));
            }
            
        }
        private void setting_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/settings.xaml", UriKind.Relative));
        }
    }
}
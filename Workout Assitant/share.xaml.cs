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
using System.Text;
using System.Windows.Navigation;
using System.IO;
using System.Runtime.Serialization;

using System.Runtime.Serialization.Json;
using System.IO.IsolatedStorage;


namespace Workout_Assitant
{
    public partial class share : PhoneApplicationPage
    {
        public share()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var uriParams = new Dictionary<string, string>() {
                        {"client_id", "266709873344044"},
                        {"response_type", "token"},
                        {"scope", "user_about_me, offline_access, publish_stream"},
                        {"redirect_uri", "http://www.facebook.com/connect/login_success.html"},
                        {"display", "touch"}
                    };
            StringBuilder urlBuilder = new StringBuilder();
            foreach (var current in uriParams)
            {
                if (urlBuilder.Length > 0)
                {
                    urlBuilder.Append("&");
                }
                var encoded = HttpUtility.UrlEncode(current.Value);
                urlBuilder.AppendFormat("{0}={1}", current.Key, encoded);
            }
            var loginUrl = "http://www.facebook.com/dialog/oauth?" + urlBuilder.ToString();

            webBrowser1.Navigate(new Uri(loginUrl));
            webBrowser1.Visibility = Visibility.Visible;


        }
        public string AccessToken { get; set; }
            
        private void BrowserNavigated(object sender, NavigationEventArgs e) 
        {
            if (string.IsNullOrEmpty(e.Uri.Fragment)) return;
            if (e.Uri.AbsoluteUri.Replace(e.Uri.Fragment,"")==
            "https://www.facebook.com/connect/login_success.html") {
            string text = HttpUtility.HtmlDecode(e.Uri.Fragment).TrimStart('#');
            var pairs = text.Split('&');
            foreach (var pair in pairs) {
                var kvp = pair.Split('=');
                if (kvp.Length == 2) {
                    if (kvp[0] == "access_token") {
                        AccessToken = kvp[1];
                        MessageBox.Show("Access granted");
                    }
                }
            }
            if (string.IsNullOrEmpty(AccessToken)) {
                MessageBox.Show("Unable to authenticate");
            }
            textBlock1.Text = "Access Granted Press Post";
            button2.Visibility = System.Windows.Visibility.Visible;
            button1.Visibility = System.Windows.Visibility.Collapsed;
            webBrowser1.Visibility = System.Windows.Visibility.Collapsed;
            }
        }


            private void PostUpdateClick(object sender, RoutedEventArgs e) {
                IsolatedStorageSettings settings;
            settings = IsolatedStorageSettings.ApplicationSettings;
                string status = (string)settings["fb"];

                PostStatusUpdate(status, (success,ex)=> {
                this.Dispatcher.BeginInvoke(() => {
                if (success && ex == null) {
                    MessageBox.Show("Status updated");
                }
                else {
                MessageBox.Show("Unable to update status");
                }
                });
             });
            }



        private void PostStatusUpdate(string status, Action<bool, Exception> callback)
        {
            var request = HttpWebRequest.Create("https://graph.facebook.com/me/feed");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream((reqResult) =>
            {
                using (var strm = request.EndGetRequestStream(reqResult))
                using (var writer = new StreamWriter(strm))
                {
                    writer.Write("access_token=" + AccessToken);
                    writer.Write("&message=" + HttpUtility.UrlEncode(status));
                }
                request.BeginGetResponse((result) =>
                {
                    try
                    {
                        var response = request.EndGetResponse(result);
                        using (var rstrm = response.GetResponseStream())
                        {
                            var serializer = new DataContractJsonSerializer(typeof(FacebookPostResponse));
                            var postResponse = serializer.ReadObject(rstrm) as FacebookPostResponse;
                            callback(true, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        callback(false, ex);
                    }
                }, null);
            }, null);
        }
        [DataContract]
        public class FacebookPostResponse
        {
            [DataMember(Name = "id")]
            public string Id { get; set; }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace BugSenseW8.Error
{
    [DataContract]
    internal class BugSenseEnvironment
    {
        [DataMember(Name="phone")]
        public string Phone { get { return "windows8-metro"; } set { } }

        [DataMember(Name = "appver")]
        public string AppVer { get; set; }

        [DataMember(Name = "appname")]
        public string AppName { get; set; }

        [DataMember(Name = "osver")]
        public string OsVersion { get; set; }

        [DataMember(Name = "wifi_on")]
        public bool? IsWifiOn { get; set; }

        [DataMember(Name = "mobile_net_on")]
        public bool? IsMobileNetOn { get; set; }

        [DataMember(Name = "gps_on")]
        public bool? IsGpsOn { get; set; }

        [DataMember(Name = "screen:height")]
        public double ScreenHeight { get; set; }

        [DataMember(Name = "screen:width")]
        public double ScreenWidth { get; set; }

        [DataMember(Name = "screen:orientation")]
        public string Orientation { get; set; }

        public BugSenseEnvironment()
        {
            var ver = Package.Current.Id.Version;
            
            AppVer = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Revision, ver.Build);
            
            Orientation = ApplicationView.Value.ToString();
            ScreenHeight = Window.Current.Bounds.Height;
            ScreenWidth = Window.Current.Bounds.Width;
            
            IsGpsOn = null;
            IsWifiOn = null;
            IsMobileNetOn = null;
        }
    }
}

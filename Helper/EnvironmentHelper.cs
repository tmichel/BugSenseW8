using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;

namespace BugSenseW8.Helper
{
    internal static class EnvironmentHelper
    {
        public static async Task<string> GetAppDisplayName()
        {
            var f = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
            var d = await FileIO.ReadTextAsync(f);
            var doc = XDocument.Parse(d);
            var nodes = from n in doc.Descendants()
                        where n.Name.LocalName == "DisplayName"
                        select n.Value;

            return nodes.FirstOrDefault();
        }

        public static async Task<string> GetOsVersion()
        {
            var f = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
            var d = await FileIO.ReadTextAsync(f);
            var doc = XDocument.Parse(d, LoadOptions.None);
            var nodes = from n in doc.Descendants()
                        where n.Name.LocalName == "Item" && n.Attribute("Name").Value == "OperatingSystem"
                        select n.Attribute("Version").Value;
            return nodes.FirstOrDefault();
        }
    }
}

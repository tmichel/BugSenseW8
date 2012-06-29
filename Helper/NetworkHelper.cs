using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace BugSenseW8.Helper
{
    internal enum NetworkInterfaceType
    {
        /*
         * 1 Some other type of network interface.
         * 6 An Ethernet network interface.
         * 9 A token ring network interface.
         * 23 A PPP network interface.
         * 24 A software loopback network interface.
         * 37 An ATM network interface.
         * 71 An IEEE 802.11 wireless network interface.
         * 131 A tunnel type encapsulation network interface.
         * 144 An IEEE 1394 (Firewire) high performance serial bus network interface.
         */

        Wifi = 71,
        Ethernet = 6
    }

    public static class NetworkHelper
    {
        public static string CurrentIPAddress()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp != null && icp.NetworkAdapter != null)
            {
                var hostname = NetworkInformation.GetHostNames().SingleOrDefault(
                    hn => hn.IPInformation != null
                        && hn.IPInformation.NetworkAdapter != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId);
                if (hostname != null)
                {
                    return hostname.CanonicalName;
                }
            }
            return string.Empty;
        }
    }
}

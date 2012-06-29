using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BugSenseW8.Error
{
    [DataContract]
    internal class BugSenseRequest
    {
        [DataMember(Name="remote_ip")]
        public string RemoteIPAddress { get; set; }

        [DataMember(Name = "custom_data")]
        public Dictionary<string, string> CustomData { get; set; }

        public BugSenseRequest()
        {
            CustomData = new Dictionary<string, string>();
        }

        public BugSenseRequest(string ip, Dictionary<string, string> customData)
        {
            RemoteIPAddress = ip;
            CustomData = customData;
        }

    }
}

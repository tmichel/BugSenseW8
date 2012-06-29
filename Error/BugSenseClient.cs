using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BugSenseW8.Error
{
    [DataContract]
    internal class BugSenseClient
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="version")]
        public string Version { get; set; }


    }
}

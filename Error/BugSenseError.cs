using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BugSenseW8.Error;

namespace BugSenseW8.Error
{
    [DataContract]
    internal class BugSenseError
    {
        [DataMember(Name="client")]
        public BugSenseClient Client { get; set; }

        [DataMember(Name = "request")]
        public BugSenseRequest Request { get; set; }

        [DataMember(Name = "exception")]
        public BugSenseException Exception { get; set; }

        [DataMember(Name = "application_environment")]
        public BugSenseEnvironment Environment { get; set; }


    }
}

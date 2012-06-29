using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BugSenseW8.Error
{
    [DataContract]
    internal class BugSenseException
    {
        [DataMember(Name="message")]
        public string Message { get; set; }

        [DataMember(Name = "where")]
        public string Where { get; set; }

        [DataMember(Name = "klass")]
        public string Class { get; set; }

        [DataMember(Name = "backtrace")]
        public string StackTrace { get; set; }

        public BugSenseException()
        {

        }

        public BugSenseException(string message, string where, string klass, string stacktrace)
        {
            Message = message;
            Where = where;
            Class = klass;
            StackTrace = stacktrace;
        }

        public BugSenseException(Exception ex)
        {
            Message = ex.Message;
            Where = GetErrorPosition(ex.StackTrace);
            Class = ex.GetType().FullName;
            StackTrace = ex.StackTrace;
        }

        private string GetErrorPosition(string stacktrace)
        {
            int idx = stacktrace.IndexOf('\n');
            var fl = stacktrace.Substring(0, (idx < 0 ? stacktrace.Length : idx));
            idx = fl.LastIndexOf('\\');
            return fl.Substring(idx + 1, fl.Length - idx - 1);
        }


    }
}

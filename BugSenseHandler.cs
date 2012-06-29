using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using BugSenseW8.Error;
using BugSenseW8.Helper;

namespace BugSenseW8
{
    public enum ReportMode
	{
        Normal,
        Secure
	}

    public sealed class BugSenseHandler
    {
        #region singleton

        private static BugSenseHandler _current;
        public static BugSenseHandler Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new BugSenseHandler();
                }
                return _current;
            }
        }

        #endregion

        #region constants

        private const string ApiKeyHttpHeader = "X-BugSense-Api-Key";
        private const string Version = "0.1";
        private const string ClientName = "bugsense-metro";
        private const string NormalAddress = "http://www.bugsense.com";
        private const string SecureAddress = "https://bugsense.appspot.com";
        private const string RequestUri = "api/errors";
        
        #endregion

        #region fields

        private bool _initialized = false;
        private string _apiKey;
        private BugSenseClient _client;
        private BugSenseEnvironment _appEnv;
        private ReportMode _mode;

        #endregion

        #region properties

        public ReportMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        #endregion

        private BugSenseHandler()
        {

        }

        #region public methods

        /// <summary>
        /// Initializes the BugSenseHandler object. Gets all the information about the system.
        /// Subscribes to the UnhandledException event.
        /// </summary>
        /// <param name="apiKey">api key obtained from bugsense</param>
        /// <param name="mode">connection mode: http or https</param>
        public async Task Initialize(string apiKey, ReportMode mode = ReportMode.Normal)
        {
            if (_initialized) { return; }

            _initialized = true;
            _apiKey = apiKey;
            _client = new BugSenseClient() { Name = ClientName, Version = Version };
            _appEnv = new BugSenseEnvironment();
            _appEnv.AppName = await EnvironmentHelper.GetAppDisplayName();
            _appEnv.OsVersion = await EnvironmentHelper.GetOsVersion();
            _mode = mode;

            Windows.UI.Xaml.Application.Current.UnhandledException += Application_UnhandledException;
        }

        public void LogError(Exception ex)
        {
            LogError(ex, null);
        }

        public void LogError(Exception ex, Dictionary<string, string> customData)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("BugSenseHandler must be initialized first!");
            }

            BugSenseError error = new BugSenseError();
            error.Client = _client;
            error.Request = new BugSenseRequest(NetworkHelper.CurrentIPAddress(), customData);
            error.Exception = new BugSenseException(ex);
            error.Environment = _appEnv;

            SendRequest(error);
        }

        #endregion

        #region private methods

        private void Application_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                LogError(e.Exception, new Dictionary<string, string>() { { "UnhandledExceptionMessage", e.Message } });
            }
        }

        private async Task SendRequest(BugSenseError er)
        {
            HttpClient client = new HttpClient();
            switch (_mode)
            {
                case ReportMode.Normal:
                    client.BaseAddress = new Uri(NormalAddress, UriKind.Absolute);
                    break;
                case ReportMode.Secure:
                    client.BaseAddress = new Uri(SecureAddress, UriKind.Absolute);
                    break;
                default:
                    throw new Exception("Ajjaj, nagy gaz van.");
            }
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.Add(ApiKeyHttpHeader, _apiKey);

            StringContent content = new StringContent("data=" + Uri.EscapeDataString(GetJson(er)), Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = null;

            try
            {
                response = await client.PostAsync(RequestUri, content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("## Error in the error reporting process :)\n## Message: {0}", ex.Message);
            }
        }

        private string GetJson(BugSenseError er)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(BugSenseError));

            string json = null;
            using (var stream = new MemoryStream())
            {

                try
                {
                    s.WriteObject(stream, er);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                byte[] payload = stream.ToArray();
                json = Encoding.UTF8.GetString(payload, 0, payload.Length);
            }
            System.Diagnostics.Debug.WriteLine(json);
            return json;
        }

        #endregion
    }
}

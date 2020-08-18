using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace Sandbox.Modules.CSharp.API_Integration.TD_Ameritrade {
    public class TDAmeritradeService {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static TDAmeritradeService instance;
        public static TDAmeritradeService Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new TDAmeritradeService();

                return instance;
            }
        }

        #endregion

        #region Constants

        private const string OAUTH_URL = "https://api.tdameritrade.com/v1/oauth2/token";

        #endregion

        #region Fields

        private string tdAccessCode = string.Empty;
        private string tdRefreshCode = string.Empty;

        #endregion

        #region Properties

        public string RedirectURL { get; private set; }
        public string ClientID { get; private set; }

        #endregion

        #region Constructors

        private TDAmeritradeService() { }

        #endregion

        #region Public Methods

        public void Initialize(string redirectUrl, string clientID, string accessCode) {
            RedirectURL = redirectUrl;
            ClientID = clientID;
            tdAccessCode = accessCode;
            authorizationThread.Start();
        }

        #endregion

        #region Authentication

        private Thread authorizationThread = new Thread(new ThreadStart(Authenticate)) { IsBackground = true };
        private static void Authenticate() {
            bool refresh = false;
            do {
                try {
                    string grant = refresh ? "refresh_token" : "authorization_code";
                    string accessType = refresh ? string.Empty : "offline";
                    string codeType = refresh ? "refresh_token" : "code";
                    string encodedAccessCode = HttpUtility.UrlEncode(refresh ? Instance.tdRefreshCode : Instance.tdAccessCode);
                    string encodedRedirect = HttpUtility.UrlEncode(Instance.RedirectURL);
                    string encodedClientId = HttpUtility.UrlEncode(Instance.ClientID);
                    RestClient client = new RestClient(OAUTH_URL);
                    RestRequest postRequest = new RestRequest(Method.POST);
                    postRequest.AddHeader("cache-control", "no-cache");
                    postRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                    postRequest.AddParameter("application/x-www-form-urlencoded",
                                             $"grant_type={grant}&access_type={accessType}&{codeType}={encodedAccessCode}&client_id={encodedClientId}&redirect_uri={encodedRedirect}",
                                             ParameterType.RequestBody);

                    IRestResponse response = client.Execute(postRequest);
                    JsonSerializer serializer = new JsonSerializer();
                    Instance.tdAccessCode = serializer.Deserialize<Dictionary<string, object>>(response)["access_token"].ToString();

                    if (!refresh) {
                        Instance.tdRefreshCode = serializer.Deserialize<Dictionary<string, object>>(response)["refresh_token"].ToString();
                        refresh = true;
                    }

                    Instance.OnAuthenticationSucceeded("Authentication successful.");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                } catch (Exception e) {
                    Instance.OnAuthenticationFailed($"Authentication Failed: {e.Message}");
                    break;
                }
            } while (true);
        }
        public event EventHandler<string> AuthenticationSucceeded;
        protected virtual void OnAuthenticationSucceeded(string message) => AuthenticationSucceeded?.Invoke(this, message);
        public event EventHandler<string> AuthenticationFailed;
        protected virtual void OnAuthenticationFailed(string message) => AuthenticationFailed?.Invoke(this, message);

        #endregion

    }
}
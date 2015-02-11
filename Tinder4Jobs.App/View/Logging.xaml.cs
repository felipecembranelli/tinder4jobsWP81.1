using PrototypeTinder4Jobs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Tinder4Jobs.OAuth;
using Windows.Storage;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Tinder4Jobs.Library;
using System.Text;
using Windows.UI.Popups;
using Tinder4Jobs.Utils;
using Tinder4Jobs.oAuth;
using Tinder4Jobs.oAuth.Linkedin;
using Tinder4Jobs.Model;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Tinder4Jobs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Logging : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();


        #region "temp"

        private static string DB_PATH = Path.Combine(Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, DB_FILE_NAME_TEMPLATE));//DataBase Name
        private const string DB_FILE_NAME_TEMPLATE = "Tinder4Jobs_v01.sqlite";
        private const string DB_FILE_NAME_COPY = "Tinder4Jobs_copy_v01.sqlite";

        LinkedinUser _LoggedUser;

        string _consumerKey = "772jmojzy2vnra";
        string _consumerSecretKey = "hQu5DFEqP5JQyPGr";
        string _linkedInRequestTokenUrl = "https://api.linkedin.com/uas/oauth/requestToken";
        string _linkedInAccessTokenUrl = "https://api.linkedin.com/uas/oauth/accessToken";
        string _requestPeopleUrl = "http://api.linkedin.com/v1/people/~";
        string _requestConnectionsUrl = "http://api.linkedin.com/v1/people/~/connections";
        string _requestPositionsUrl = "http://api.linkedin.com/v1/people/~:(positions)";
        string _requestJobsUrl = "http://api.linkedin.com/v1/people/~/suggestions/job-suggestions";
        string _requestJobsByKeyWordsUrl = "https://api.linkedin.com/v1/job-search?keywords=quality";
        string _oAuthAuthorizeLink = "";
        string _requestToken = "";
        string _oAuthVerifier = "";
        string _requestTokenSecretKey = "";
        string _accessToken = "";
        string _accessTokenSecretKey = "";
        string _linkedInProfile = "";
        string callback = "https://www.linkedin.com/sucess.htm";

        OAuthUtil oAuthUtil = new OAuthUtil();

        #endregion

        public Logging()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            InitDatabase();
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            //webBrowser.Navigate(new Uri("https://www.linkedin.com", UriKind.Absolute));

            this.GetRequestToken();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region "Database methods"

        private async void InitDatabase()
        {
            if (FileExists(DB_FILE_NAME_COPY).Result)
            {
                // file exists;
                DB_PATH = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                 DB_FILE_NAME_COPY);
            }
            else
            {
                // file does not exist
                StorageFile databaseFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(DB_FILE_NAME_TEMPLATE);
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder, DB_FILE_NAME_COPY);
                DB_PATH = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DB_FILE_NAME_COPY);
            }
        }

        private async Task<bool> FileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                return true;
            }
            catch
            {
            }
            return false;
        }
        #endregion

        //#region "Authetication Methods"

        //private async System.Threading.Tasks.Task Authenticate(string accessToken, string lkId)
        //{

        //    // INICIO
        //    string _requestPeopleUrl = "http://api.linkedin.com/v1/people/~";
        //    //string _requestPeopleUrl = "https://api.linkedin.com/v1/people/~:(id,first-name,last-name,headline)";

        //    string nonce = oAuthUtil.GetNonce();
        //    string timeStamp = oAuthUtil.GetTimeStamp();
        //    bool authenticationError = false;
        //    string authenticationErrorMsg = string.Empty;

        //    try
        //    {

        //        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        //        httpClient.MaxResponseContentBufferSize = int.MaxValue;
        //        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        //        System.Net.Http.HttpRequestMessage requestMsg = new System.Net.Http.HttpRequestMessage();
        //        requestMsg.Method = new System.Net.Http.HttpMethod("GET");
        //        requestMsg.RequestUri = new Uri(_requestPeopleUrl, UriKind.Absolute);
        //        requestMsg.Headers.Add("x-li-format", "json");


        //        string sigBaseStringParams = "oauth_consumer_key=" + _consumerKey;
        //        sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
        //        sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
        //        sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
        //        sigBaseStringParams += "&" + "oauth_token=" + _accessToken;
        //        sigBaseStringParams += "&" + "oauth_verifier=" + _oAuthVerifier;
        //        sigBaseStringParams += "&" + "oauth_version=1.0";
        //        string sigBaseString = "GET&";
        //        sigBaseString += Uri.EscapeDataString(_requestPeopleUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

        //        // LinkedIn requires both consumer secret and request token secret
        //        string signature = oAuthUtil.GetSignature(sigBaseString, _consumerSecretKey, _accessTokenSecretKey);

        //        string data = "realm=\"http://api.linkedin.com/\", oauth_consumer_key=\"" + _consumerKey
        //                      +
        //                      "\", oauth_token=\"" + _accessToken +
        //                      "\", oauth_verifier=\"" + _oAuthVerifier +
        //                      "\", oauth_nonce=\"" + nonce +
        //                      "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + timeStamp +
        //                      "\", oauth_version=\"1.0\", oauth_signature=\"" + Uri.EscapeDataString(signature) + "\"";

        //        requestMsg.Headers.Authorization = new AuthenticationHeaderValue("OAuth", data);
        //        var response = await httpClient.SendAsync(requestMsg);

        //        var text = response.Content.ReadAsStringAsync();

        //        _linkedInProfile = await text;

        //    }
        //    catch (Exception Err)
        //    {
        //        throw;
        //    }

        //    //<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        //    //<person>
        //    //  <first-name>Felipe</first-name>
        //    //  <last-name>Cembranelli</last-name>
        //    //  <headline>Manager at CI&amp;T</headline>
        //    //  <site-standard-profile-request>
        //    //    <url>https://www.linkedin.com/profile/view?id=3770090&amp;authType=name&amp;authToken=moVF&amp;trk=api*a3576543*s3647743*</url>
        //    //  </site-standard-profile-request>
        //    //</person>


        //    var linkedinUser = JsonConvert.DeserializeObject<LinkedinUser>(_linkedInProfile);

        //    // FIM

        //    //ProfilePhoto.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(String.Format("https://graph.facebook.com/me/picture?access_token={0}&height=100&width=100", accessToken))) };

        //    LinkedInSessionInfo sessionInfo = new LinkedInSessionInfo();
        //    sessionInfo.AcessToken = accessToken;
        //    sessionInfo.LinkedInID = linkedinUser.FirstName;
        //    sessionInfo.LinkedinUser = linkedinUser;

        //    //Geolocator location = new Geolocator();
        //    //location.DesiredAccuracy = PositionAccuracy.Default;
        //    //var usrLocation = await location.GetGeopositionAsync();

        //    //TinderSession activeSession = TinderSession.CreateNewSession(sessionInfo, new GeographicalCordinates() { Latitude = usrLocation.Coordinate.Latitude, Longitude = usrLocation.Coordinate.Longitude });

        //    TinderSession activeSession = TinderSession.CreateNewSession(sessionInfo);

        //    try
        //    {
        //        if (await activeSession.Authenticate(_consumerKey, _accessToken, _oAuthVerifier, _consumerSecretKey, _accessTokenSecretKey))
        //        {
        //            //TODO : verificar por que está null o rightsidebar
        //            // (App.Current as App).RightSideBar.DataContext = activeSession.Matches;

        //            //TopBarViewModel.ShowTopButtons = Visibility.Visible;

        //            Frame.Navigate(typeof(MainPage));

        //            //(App.Current as App).RootFrameInstance.Navigate(typeof(Main));

        //            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

        //            //App.RootFrame.RemoveBackEntry();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        authenticationError = true;
        //        authenticationErrorMsg = ex.Message;
        //    }

        //    if (authenticationError)
        //    {
        //        var dialog = new MessageDialog("Unable to authenticate user: " + authenticationErrorMsg);
        //        await dialog.ShowAsync();
        //    }


        //    // Persist authentication data
        //    oAuthData oAuthData = new oAuthData();
        //    oAuthData.OAuthVerifier = _oAuthVerifier;
        //    oAuthData.AccessToken = _accessToken;
        //    oAuthData.AccessTokenSecretKey = _accessTokenSecretKey;

        //    oAuthSessionManager.Save(oAuthData);

        //}


        //#endregion

        #region "LinkedIn auth"

        private async void RequestLinkedInApi(string url)
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            try
            {
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                System.Net.Http.HttpRequestMessage requestMsg = new System.Net.Http.HttpRequestMessage();
                requestMsg.Method = new System.Net.Http.HttpMethod("GET");
                requestMsg.RequestUri = new Uri(url, UriKind.Absolute);

                string sigBaseStringParams = "oauth_consumer_key=" + _consumerKey;
                sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
                sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
                sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
                sigBaseStringParams += "&" + "oauth_token=" + _accessToken;
                sigBaseStringParams += "&" + "oauth_verifier=" + _oAuthVerifier;
                sigBaseStringParams += "&" + "oauth_version=1.0";
                string sigBaseString = "GET&";
                sigBaseString += Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(sigBaseStringParams);

                // LinkedIn requires both consumer secret and request token secret
                string signature = oAuthUtil.GetSignature(sigBaseString, _consumerSecretKey, _accessTokenSecretKey);

                string data = "realm=\"http://api.linkedin.com/\", oauth_consumer_key=\"" + _consumerKey
                              +
                              "\", oauth_token=\"" + _accessToken +
                              "\", oauth_verifier=\"" + _oAuthVerifier +
                              "\", oauth_nonce=\"" + nonce +
                              "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + timeStamp +
                              "\", oauth_version=\"1.0\", oauth_signature=\"" + Uri.EscapeDataString(signature) + "\"";

                requestMsg.Headers.Authorization = new AuthenticationHeaderValue("OAuth", data);
                var response = await httpClient.SendAsync(requestMsg);

                var text = response.Content.ReadAsStringAsync();

                _linkedInProfile = await text;

            }
            catch (Exception Err)
            {
                throw;
            }
        }

        public string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        private async System.Threading.Tasks.Task GetRequestToken()
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            string sigBaseStringParams = "oauth_callback=" + this.UrlEncode(callback);
            sigBaseStringParams += "&" + "oauth_consumer_key=" + _consumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;

            sigBaseStringParams += "&" + "oauth_version=1.0";

            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(_linkedInRequestTokenUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            string signature = oAuthUtil.GetSignature(sigBaseString, _consumerSecretKey);

            var responseText = await oAuthUtil.PostData(_linkedInRequestTokenUrl, sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature));

            if (!string.IsNullOrEmpty(responseText))
            {
                string oauth_token = null;
                string oauth_token_secret = null;
                string oauth_authorize_url = null;
                string[] keyValPairs = responseText.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            oauth_token = splits[1];
                            break;
                        case "oauth_token_secret":
                            oauth_token_secret = splits[1];
                            break;
                        case "xoauth_request_auth_url":
                            oauth_authorize_url = splits[1];
                            break;
                    }
                }

                _requestToken = oauth_token;
                _requestTokenSecretKey = oauth_token_secret;
                _oAuthAuthorizeLink = Uri.UnescapeDataString(oauth_authorize_url + "?oauth_token=" + oauth_token);

                //// Step 2 : Call linkedin web page for authentication
                webBrowser.Navigate(new Uri(_oAuthAuthorizeLink, UriKind.Absolute));

            }
            else
            {
                var dialog = new MessageDialog("Unable to authenticate user.");
                await dialog.ShowAsync();
            }

        }

        private async System.Threading.Tasks.Task GetAccessToken()
        {
            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();

            string sigBaseStringParams = "oauth_consumer_key=" + _consumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_token=" + _requestToken;
            sigBaseStringParams += "&" + "oauth_verifier=" + _oAuthVerifier;
            sigBaseStringParams += "&" + "oauth_version=1.0";

            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(_linkedInAccessTokenUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            // LinkedIn requires both consumer secret and request token secret
            string signature = oAuthUtil.GetSignature(sigBaseString, _consumerSecretKey, _requestTokenSecretKey);

            var responseText = await oAuthUtil.PostData(_linkedInAccessTokenUrl, sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature));

            if (!string.IsNullOrEmpty(responseText))
            {
                string oauth_token = null;
                string oauth_token_secret = null;
                string[] keyValPairs = responseText.Split('&');

                for (int i = 0; i < keyValPairs.Length; i++)
                {
                    String[] splits = keyValPairs[i].Split('=');
                    switch (splits[0])
                    {
                        case "oauth_token":
                            oauth_token = splits[1];
                            break;
                        case "oauth_token_secret":
                            oauth_token_secret = splits[1];
                            break;
                    }
                }

                _accessToken = oauth_token;
                _accessTokenSecretKey = oauth_token_secret;

                //if (oauth_token == null)
                //    rootPage.NotifyUser("Error getting accessToken", NotifyType.ErrorMessage);
                //else
                //    rootPage.NotifyUser("accessToken:" + oauth_token, NotifyType.StatusMessage);

                if (_oAuthVerifier == "")
                {
                    //if (Pulsate.GetCurrentState() == ClockState.Active)
                    //{
                    //    Pulsate.Stop();
                    //}
                    //await webBrowser.ClearCookiesAsync();
                    WebViewBorder.Visibility = Visibility.Collapsed;
                    //LoginButtonBorder.Visibility = Visibility.Visible;
                    //LoggedInPanel.Visibility = Visibility.Collapsed;
                    //MessageBox.Show("Unable to login using Linkedin.  Please try again.");
                }

           //if (user == null)
                //{
                //    if (Pulsate.GetCurrentState() == ClockState.Active)
                //    {
                //        Pulsate.Stop();
                //    }
                //    await webBrowser.ClearCookiesAsync();
                //    WebViewBorder.Visibility = System.Windows.Visibility.Collapsed;
                //    LoginButtonBorder.Visibility = System.Windows.Visibility.Visible;
                //    LoggedInPanel.Visibility = System.Windows.Visibility.Collapsed;
                //    MessageBox.Show("Unable to login using Facebook.  Please try again.");
                //}
                else
                {
                    LinkedinAuthentication lkdAuth = new LinkedinAuthentication();

                    await lkdAuth.Authenticate(_accessToken,
                        _oAuthVerifier,
                        _accessTokenSecretKey,
                        this.Frame);

                    //await Authenticate(_accessToken, _requestToken);

                }

            }
        }
        #endregion

        #region "WebView Events"

        private void webBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs e)
        {
            {
                if (e.Uri.ToString().StartsWith("https://www.linkedin.com/sucess.htm"))
                {
                    //e.Cancel = true;

                    WebViewBorder.Visibility = Visibility.Collapsed;
                    //LoginButtonBorder.Visibility = Visibility.Collapsed;
                    //LoggedInPanel.Visibility = Visibility.Visible;


                    try
                    {

                        //accessToken = e.Uri.ToString().Substring(e.Uri.ToString().IndexOf("access_token=") + "access_token=".Length);
                        //accessToken = _accessToken;

                        string queryParams = e.Uri.Query;

                        if (queryParams.Length > 0)
                        {
                            QueryString qs = new QueryString(queryParams);

                            if (qs["oauth_verifier"] != null)
                            {
                                this._oAuthVerifier = qs["oauth_verifier"];
                            }
                        }


                        //if (accessToken.IndexOf("&") > 0)
                        //    accessToken = accessToken.Substring(0, accessToken.IndexOf("&"));

                        //user = await FacebookUserResponse.GetFacebookUser(accessToken);
                        this.GetAccessToken();

                    }
                    catch { }


                }
            }
        }

        #endregion

    }
}

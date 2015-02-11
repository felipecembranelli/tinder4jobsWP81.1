using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tinder4Jobs.Library;
using Tinder4Jobs.Model;
using Tinder4Jobs.OAuth;
using Windows.UI.Xaml.Controls;

namespace Tinder4Jobs.oAuth.Linkedin
{
    class LinkedinAuthentication
    {
        public async System.Threading.Tasks.Task Authenticate(string accessToken, string oAuthVerifier, string accessTokenSecretKey, Frame pageFrame)
        {
            OAuthUtil oAuthUtil = new OAuthUtil();
            string _consumerKey = "772jmojzy2vnra";
            string _consumerSecretKey = "hQu5DFEqP5JQyPGr";
            //string _linkedInRequestTokenUrl = "https://api.linkedin.com/uas/oauth/requestToken";
            //string _linkedInAccessTokenUrl = "https://api.linkedin.com/uas/oauth/accessToken";
            //string _requestPeopleUrl = "http://api.linkedin.com/v1/people/~";
            //string _requestConnectionsUrl = "http://api.linkedin.com/v1/people/~/connections";
            //string _requestPositionsUrl = "http://api.linkedin.com/v1/people/~:(positions)";
            //string _requestJobsUrl = "http://api.linkedin.com/v1/people/~/suggestions/job-suggestions";
            //string _requestJobsByKeyWordsUrl = "https://api.linkedin.com/v1/job-search?keywords=quality";
            //string _oAuthAuthorizeLink = "";
            //string _requestToken = "";
            //string _oAuthVerifier = "";
            //string _requestTokenSecretKey = "";
            //string _accessToken = "";
            //string _accessTokenSecretKey = "";
            string _linkedInProfile = "";
            //string callback = "https://www.linkedin.com/sucess.htm";


            // INICIO
            string _requestPeopleUrl = "http://api.linkedin.com/v1/people/~";
            //string _requestPeopleUrl = "https://api.linkedin.com/v1/people/~:(id,first-name,last-name,headline)";

            string nonce = oAuthUtil.GetNonce();
            string timeStamp = oAuthUtil.GetTimeStamp();
            bool authenticationError = false;
            string authenticationErrorMsg = string.Empty;

            try
            {

                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                System.Net.Http.HttpRequestMessage requestMsg = new System.Net.Http.HttpRequestMessage();
                requestMsg.Method = new System.Net.Http.HttpMethod("GET");
                requestMsg.RequestUri = new Uri(_requestPeopleUrl, UriKind.Absolute);
                requestMsg.Headers.Add("x-li-format", "json");


                string sigBaseStringParams = "oauth_consumer_key=" + _consumerKey;
                sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
                sigBaseStringParams += "&" + "oauth_signature_method=" + "HMAC-SHA1";
                sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
                sigBaseStringParams += "&" + "oauth_token=" + accessToken;
                sigBaseStringParams += "&" + "oauth_verifier=" + oAuthVerifier;
                sigBaseStringParams += "&" + "oauth_version=1.0";
                string sigBaseString = "GET&";
                sigBaseString += Uri.EscapeDataString(_requestPeopleUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

                // LinkedIn requires both consumer secret and request token secret
                string signature = oAuthUtil.GetSignature(sigBaseString, _consumerSecretKey, accessTokenSecretKey);

                string data = "realm=\"http://api.linkedin.com/\", oauth_consumer_key=\"" + _consumerKey
                              +
                              "\", oauth_token=\"" + accessToken +
                              "\", oauth_verifier=\"" + oAuthVerifier +
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

            //<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            //<person>
            //  <first-name>Felipe</first-name>
            //  <last-name>Cembranelli</last-name>
            //  <headline>Manager at CI&amp;T</headline>
            //  <site-standard-profile-request>
            //    <url>https://www.linkedin.com/profile/view?id=3770090&amp;authType=name&amp;authToken=moVF&amp;trk=api*a3576543*s3647743*</url>
            //  </site-standard-profile-request>
            //</person>


            var linkedinUser = JsonConvert.DeserializeObject<LinkedinUser>(_linkedInProfile);


            if (linkedinUser.FirstName == null)
                throw new Exception("User not authenticated.");
            // FIM

            //ProfilePhoto.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(String.Format("https://graph.facebook.com/me/picture?access_token={0}&height=100&width=100", accessToken))) };

            LinkedInSessionInfo sessionInfo = new LinkedInSessionInfo();
            sessionInfo.AcessToken = accessToken;
            sessionInfo.LinkedInID = linkedinUser.FirstName;
            sessionInfo.LinkedinUser = linkedinUser;

            //Geolocator location = new Geolocator();
            //location.DesiredAccuracy = PositionAccuracy.Default;
            //var usrLocation = await location.GetGeopositionAsync();

            //TinderSession activeSession = TinderSession.CreateNewSession(sessionInfo, new GeographicalCordinates() { Latitude = usrLocation.Coordinate.Latitude, Longitude = usrLocation.Coordinate.Longitude });

            TinderSession activeSession = TinderSession.CreateNewSession(sessionInfo);

            try
            {
                if (await activeSession.LoadLinkedinJobSuggestions(_consumerKey, accessToken, oAuthVerifier, _consumerSecretKey, accessTokenSecretKey))
                {
                    //TODO : verificar por que está null o rightsidebar
                    // (App.Current as App).RightSideBar.DataContext = activeSession.Matches;

                    //TopBarViewModel.ShowTopButtons = Visibility.Visible;

                    pageFrame.Navigate(typeof(MainPage));

                    //(App.Current as App).RootFrameInstance.Navigate(typeof(Main));

                    //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

                    //App.RootFrame.RemoveBackEntry();
                }

            }
            catch (Exception ex)
            {

                authenticationError = true;
                authenticationErrorMsg = ex.Message;
            }

            //if (authenticationError)
            //{
            //    var dialog = new MessageDialog("Unable to authenticate user: " + authenticationErrorMsg);
            //    await dialog.ShowAsync();
            //}

            // Persist authentication data
            oAuthData oAuthData = new oAuthData();
            oAuthData.OAuthVerifier = oAuthVerifier;
            oAuthData.AccessToken = accessToken;
            oAuthData.AccessTokenSecretKey = accessTokenSecretKey;

            oAuthSessionManager.Save(oAuthData);

        }
    }
}

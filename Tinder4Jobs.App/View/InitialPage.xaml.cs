using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Tinder4Jobs.Library;
using Tinder4Jobs.Library.Linkedin;
using Tinder4Jobs.oAuth;
using Tinder4Jobs.oAuth.Linkedin;
using Tinder4Jobs.OAuth;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Tinder4Jobs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitialPage : Page
    {
        public InitialPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // verify if already logged in authenticator authority
            if (oAuthSessionManager.Load().AccessToken != null)
            {
                // create session
                LinkedinAuthentication lkdAuth = new LinkedinAuthentication();

                lkdAuth.Authenticate(oAuthSessionManager.Load().AccessToken,
                    oAuthSessionManager.Load().OAuthVerifier,
                    oAuthSessionManager.Load().AccessTokenSecretKey,
                    this.Frame);

                // call main page
                //Frame.Navigate(typeof(MainPage));
            }
            else
            {
                LoginButtonBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            //// verify if already logged in authenticator authority
            //if (oAuthSessionManager.Load().AccessToken != null)
            //{
            //    // create session
            //    Authenticate(oAuthSessionManager.Load().AccessToken, 
            //        oAuthSessionManager.Load().OAuthVerifier,
            //        oAuthSessionManager.Load().AccessTokenSecretKey);

            //    // call main page
            //    //Frame.Navigate(typeof(MainPage));
            //}
            //else
            //{
                Frame.Navigate(typeof(Logging));
            //}

            
        }

    

    }
}

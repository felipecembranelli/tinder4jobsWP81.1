using PrototypeTinder4Jobs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tinder4Jobs.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class JobList : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        //List<SampleData> list = new List<SampleData>();
            

        public JobList()
        {


            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;

        }

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
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //}

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            LinkedinJob job = new LinkedinJob();

            var list = job.GetAllJobsByStatus(LinkedinJobStatus.Approved.ToString());

            this.DefaultViewModel["Like"] = list;

            this.ListViewApproved.ItemsSource = list;
        }

        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {

            LinkedinJob job = new LinkedinJob();

            var list = job.GetAllJobsByStatus(LinkedinJobStatus.NotApproved.ToString());

            this.DefaultViewModel["Pass"] = list;

            this.ListViewReproved.ItemsSource = list;

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

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //// Navigate to the appropriate destination page, configuring the new page
            //// by passing required information as a navigation parameter
            var itemId = ((LinkedinJob)e.ClickedItem).Id;
            if (!Frame.Navigate(typeof(JobDetail), itemId))
            {
                throw new Exception("Job detail not found");
            }

        }
    }
}

using PrototypeTinder4Jobs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace PrototypeTinder4Jobs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JobList : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        List<SampleData> list = new List<SampleData>();
            

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

            //this.DefaultViewModel[FirstGroupName] = sampleDataGroup;

            string jobDesc = this.FormatText(@"Description : successful Relationship Manager has a strategic approach to selling, able to bring value to the customer and serve as a trusted advisor. They invest in making the customer stronger and proactively seek out opportunities for growth. Have the best interest of the client in mind and act as an internal advocate to ensure they are set up for success.
Responsibilities
Drive revenue
Proactive selling and growing the account off-cycle is a key aspect of the success of a RM. This is not just an Account Manager or renewals job. LinkendIn Relationship Manager are sales people and thrive on growing their book of business by creating value;
We have a strong value for bringing an exceptional client experience and it is the job of the Relationship Manager to make sure that we support our clients' business with excellence. We want our customers to not just do business with us because we have the best talent solutions in the market, but also because they love doing business with us.
Invest in the team
We have very strong teams here at LinkedIn. One of the greatest things about working in sales at LinkedIn is the people that you will work with. Our Relationship Managers invest heavily in each other, growing and finding success in the job by learning from one anothers' successes and failures and by building each other up.
Requirements
Proven record of driving results in a high-growth company environment;
Established reputation as a high integrity, top performer;
Good experience with sales;
Experience in staffing agencies is a plus;
Experience with Salesforce.com platform is a plus.");

            SampleData s1 = new SampleData() { CompanyName = "Microsoft", DescriptionSnippet = jobDesc, JobId= "JobId: 8787", JobPoster="Job Poster: Mary" };
            SampleData s2 = new SampleData() { CompanyName = "Dell", DescriptionSnippet = jobDesc, JobId = "JobId: 8787", JobPoster = "Job Poster: Mary" };
            SampleData s3 = new SampleData() { CompanyName = "xxx", DescriptionSnippet = jobDesc, JobId = "JobId: 8787", JobPoster = "Job Poster: Mary" };
            SampleData s4 = new SampleData() { CompanyName = "xxx", DescriptionSnippet = jobDesc, JobId = "JobId: 8787", JobPoster = "Job Poster: Mary" };

            list.Add(s1);
            list.Add(s2);
            list.Add(s3);
            list.Add(s4);
            list.Add(s1);
            list.Add(s2);
            list.Add(s3);
            list.Add(s4);
            list.Add(s1);
            list.Add(s2);
            list.Add(s3);
            list.Add(s4);

            this.DefaultViewModel["Like"] = list;

            this.ListViewApproved.ItemsSource = list;
        }

        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            this.DefaultViewModel["Pass"] = list;

            this.ListViewReproved.ItemsSource = list;
        }

        class SampleData
        {
            public string CompanyName { get; set; }
            public string DescriptionSnippet { get; set; }
            public string JobId { get; set; }
            public string JobPoster { get; set; }
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
            //var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            //if (!Frame.Navigate(typeof(ItemPage), itemId))
            //{
            //    throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            //}
            Frame.Navigate(typeof(JobDetail));
        }

        private string FormatText(string value)
        {
            return string.Format("{0} ...", value.Substring(0, 100));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tinder4Jobs.Model;
using Tinder4Jobs.ViewModel;
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
    public sealed partial class MainPage : Page
    {
        private JobReccommendationsViewModel viewModel;
        private int likeCount = 0;
        private int passCount = 0;

        public MainPage()
        {
            this.InitializeComponent();

            viewModel = new JobReccommendationsViewModel();
            DataContext = viewModel;
            this.txtJobCount.Text = string.Format("Jobs({0}) : Likes({1}) / Pass({2})", viewModel.JobCount, likeCount, passCount);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
        }

        private void btnLeftMenu(object sender, RoutedEventArgs e)
        {
            LeftBarTransform.TranslateX = 0;
            //RightBarTransform.TranslateX = 320;
            this.Overlay.Visibility = Windows.UI.Xaml.Visibility.Visible;
            
        }

        private void btnRightMenu(object sender, RoutedEventArgs e)
        {
            //RightBarTransform.TranslateX = 150;
            LeftBarTransform.TranslateX = -320;
            this.Overlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void LeftSideBar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //RightBarTransform.TranslateX = 150;
            //LeftBarTransform.TranslateX = -320;
            //this.Overlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btnInfo(object sender, RoutedEventArgs e)
        {
            var jobId = viewModel.CurrentJobReccomendation.Id;

            if (!Frame.Navigate(typeof(JobDetail), jobId))
            {
                throw new Exception("Job detail not found");
            }

        }

        private void btnJobList(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(JobList));
        }

        private void btnProfile(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Profile));
        }

        private void btnHome(object sender, RoutedEventArgs e)
        {
            //RightBarTransform.TranslateX = 150;
            LeftBarTransform.TranslateX = -320;
            this.Overlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        #region Swiping

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.OnDragDelta(sender, e);
        }

        private void OnDragDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // HorizontalChange and VerticalChange from DragDeltaGestureEventArgs are now
            // DeltaManipulation.Translation.X and DeltaManipulation.Translation.Y.
            transform.TranslateX += e.Delta.Translation.X;

            var trasnform = (sender as FrameworkElement).RenderTransform as CompositeTransform;
            trasnform.TranslateX += e.Delta.Translation.X;
            trasnform.TranslateY += e.Delta.Translation.Y;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //if (e.IsInertial)
            //{
            //    this.OnFlick(sender, e);
            //}
            //else
            //    transform.TranslateX = 0;


            //double horizontalVelocity = e.Velocities.Linear.X;
            //double verticalVelocity = e.Velocities.Linear.Y;

            //double angle = Math.Round(this.GetAngle(horizontalVelocity, verticalVelocity));

            if (transform.TranslateX > 500)
            {
                viewModel.LikeUser();
                likeCount++;
                
            }


            if (transform.TranslateX < -500)
            {
                viewModel.RejectUser();
                passCount++;
            }

            this.txtJobCount.Text = string.Format("Jobs({0}) : Likes({1}) / Pass({2})", viewModel.JobCount, likeCount, passCount);

            transform.TranslateX = 0;

        }

        private void OnFlick(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            double horizontalVelocity = e.Velocities.Linear.X;
            double verticalVelocity = e.Velocities.Linear.Y;

            double angle = Math.Round(this.GetAngle(horizontalVelocity, verticalVelocity));

            if (this.GetDirection(horizontalVelocity, verticalVelocity) == Windows.UI.Xaml.Controls.Orientation.Horizontal)
            {
                if (angle >= 180)
                    viewModel.RejectUser();
                else
                    viewModel.LikeUser();

                transform.TranslateX = 0;
            }
        }

        private Orientation GetDirection(double x, double y)
        {
            return Math.Abs(x) >= Math.Abs(y) ? Windows.UI.Xaml.Controls.Orientation.Horizontal : Windows.UI.Xaml.Controls.Orientation.Vertical;
        }

        private double GetAngle(double x, double y)
        {
            // Note that this function works in xaml coordinates, where positive y is down, and the
            // angle is computed clockwise from the x-axis. 
            double angle = Math.Atan2(y, x);

            // Atan2() returns values between pi and -pi.  We want a value between
            // 0 and 2 pi.  In order to compensate for this, we'll add 2 pi to the angle
            // if it's less than 0, and then multiply by 180 / pi to get the angle
            // in degrees rather than radians, which are the expected units in XAML.
            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            return angle * 180 / Math.PI;
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string x = "test";
        }

        private void Border_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            //double horizontalVelocity = e.Velocities.Linear.X;
            //double verticalVelocity = e.Velocities.Linear.Y;
        }

        private void Border_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            double horizontalVelocity = e.Velocities.Linear.X;
            double verticalVelocity = e.Velocities.Linear.Y;

            double angle = Math.Round(this.GetAngle(horizontalVelocity, verticalVelocity));

            if (this.GetDirection(horizontalVelocity, verticalVelocity) == Windows.UI.Xaml.Controls.Orientation.Horizontal)
            {
                if (angle >= 180)
                    viewModel.RejectUser();
                else
                    viewModel.LikeUser();

                transform.TranslateX = 0;
            }
        }

        private void Border_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            //double horizontalVelocity = e.Velocities.Linear.X;
            //double verticalVelocity = e.Velocities.Linear.Y;
        }

        #endregion

     

    }
}

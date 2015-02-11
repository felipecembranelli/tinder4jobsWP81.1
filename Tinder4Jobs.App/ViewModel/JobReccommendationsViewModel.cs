using System;
using System.Windows.Input;
//using System.Windows.Media;
//using TinderApp.Lib;
//using TinderApp.Lib.API;
//using TinderApp.Library;
//using TinderApp.Library.MVVM;
//using TinderApp.Library.Linkedin;
using Windows.UI.Xaml.Media;
using Windows.UI;
using TinderApp.DbHelper;
using Tinder4Jobs.Library.MVVM;
using TinderApp.Library.MVVM;
using Tinder4Jobs.Library;
using Tinder4Jobs.Model;

namespace Tinder4Jobs.ViewModel
{
    public class JobReccommendationsViewModel : ObservableObject
    {
        DatabaseLinkedinJobHelperClass dbHelper = new DatabaseLinkedinJobHelperClass();

        private static SolidColorBrush GRAY_BRUSH = new SolidColorBrush(Colors.LightGray);
        private static SolidColorBrush BLACK_BRUSH = new SolidColorBrush(Colors.Black);

        private readonly ICommand _likeUserCommand;

        private readonly ICommand _rejectUserCommand;

        private LinkedinJob _currentJob;

        public JobReccommendationsViewModel()
        {
            _likeUserCommand = new RelayCommand(LikeUser);
            _rejectUserCommand = new RelayCommand(RejectUser);

            if (TinderSession.CurrentSession != null 
                && TinderSession.CurrentSession.JobRecommendations.Count > 0)
            {
                _currentJob = TinderSession.CurrentSession.JobRecommendations.Pop();

                RaisePropertyChanged("Id");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("DescriptionSnippet");
                RaisePropertyChanged("LocationDescription");
                RaisePropertyChanged("CurrentJobReccomendation");
                RaisePropertyChanged("JobCount");
            }
        }

        public event EventHandler<AnimationEventArgs> OnAnimation;

        public event EventHandler OnMatch;

        public String DescriptionSnippet
        {
            get
            {
                if (_currentJob == null)
                    return "No description found";
                return this.FormatText(_currentJob.DescriptionSnippet);
            }
        }

        public String LocationDescription
        {
            get
            {
                if (_currentJob == null)
                    return "No location found";
                return _currentJob.LocationDescription;
            }
        }

        public LinkedinJob CurrentJobReccomendation
        {
            get { return _currentJob; }
        }

        public Int32 JobCount
        {
            get
            {
                if (TinderSession.CurrentSession.JobRecommendations.Count == null)
                    return 0;
                return TinderSession.CurrentSession.JobRecommendations.Count;
            }
        }

        public SolidColorBrush FriendsBrush
        {
            get
            {
                return JobCount > 0 ? BLACK_BRUSH : GRAY_BRUSH;
            }
        }
        
        public SolidColorBrush LikesBrush
        {
            get
            {
                return JobCount > 0 ? BLACK_BRUSH : GRAY_BRUSH;
            }
        }

        public ICommand LikeUserCommand
        {
            get { return _likeUserCommand; }
        }

        public String Name
        {
            get
            {
                if (_currentJob == null)
                    return "No job found";
                return "Company : " + _currentJob.Company.Name;
            }
        }

        public String Id
        {
            get
            {
                if (_currentJob == null)
                    return "No job found";
                return _currentJob.Id;
            }
        }

        public ICommand RejectUserCommand
        {
            get { return _rejectUserCommand; }
        }

        public async void LikeUser()
        {
            RaiseAnimation("Like");


            if (TinderSession.CurrentSession.JobRecommendations.Count > 0)
            {
                _currentJob = TinderSession.CurrentSession.JobRecommendations.Peek();
                dbHelper.UpdateJob(_currentJob, LinkedinJobStatus.Approved.ToString());
            }

            NextJobSuggestion();
        }

        public async void NextJobSuggestion()
        {
            if (TinderSession.CurrentSession.JobRecommendations.Count > 0)
                _currentJob = TinderSession.CurrentSession.JobRecommendations.Pop();
            else
                _currentJob = null;

            RaisePropertyChanged("Id");
            RaisePropertyChanged("Name");
            RaisePropertyChanged("DescriptionSnippet");
            RaisePropertyChanged("LocationDescription");
            RaisePropertyChanged("CurrentJobReccomendation");
            RaisePropertyChanged("JobCount");

        }

        public async void RejectUser()
        {
            RaiseAnimation("Pass");

            if (TinderSession.CurrentSession.JobRecommendations.Count > 0)
            {
                _currentJob = TinderSession.CurrentSession.JobRecommendations.Peek();

                dbHelper.UpdateJob(_currentJob, LinkedinJobStatus.NotApproved.ToString());
            }
            
            NextJobSuggestion();
        }

        private void RaiseAnimation(string animation)
        {
            if (OnAnimation != null)
                OnAnimation(this, new AnimationEventArgs(animation));
        }

        private void RaiseOnMatch()
        {
            if (OnMatch != null)
                OnMatch(this, new EventArgs());
        }

        public class AnimationEventArgs : EventArgs
        {
            public AnimationEventArgs(string animation)
            {
                AnimationName = animation;
            }

            public String AnimationName { get; set; }
        }

        #region "Utils"

        private string FormatText(string value)
        {
            return string.Format("{0} ...", value.Substring(0, 200));
        }
        #endregion
    }
}
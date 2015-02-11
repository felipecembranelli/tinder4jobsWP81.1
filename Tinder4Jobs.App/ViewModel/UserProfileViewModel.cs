using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinder4Jobs.Library.MVVM;

namespace Tinder4Jobs.ViewModel
{
    public class UserProfileViewModel: ObservableObject
    {
        public UserProfileViewModel()
        {
            this.FirstName = Tinder4Jobs.App.LinkedinUser.FirstName;
            this.LastName = Tinder4Jobs.App.LinkedinUser.LastName;
            this.HeadLine = Tinder4Jobs.App.LinkedinUser.HeadLine;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string HeadLine { get; set; }
    }
}

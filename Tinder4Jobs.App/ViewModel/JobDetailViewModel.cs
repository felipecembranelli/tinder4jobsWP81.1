using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinder4Jobs.Library.MVVM;

namespace Tinder4Jobs.ViewModel
{
    public class JobDetailViewModel : ObservableObject
    {
        public string CompanyName { get; set; }

        public string DescriptionSnippet { get; set; }

        public string Id { get; set; }

        public string JobPosterFirstName { get; set; }

        public string LocationDescription { get; set; }
    }
}

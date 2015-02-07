using System.Windows;
using Windows.UI.Xaml;

namespace TinderApp.Library.MVVM
{
    public class TopBarViewModel : ObservableObject
    {
        public static Visibility ShowTopButtons { get; set; }
    }
}
using System.Windows.Input;

namespace Peruser
{
    public static class GlobalCommands
    {
        public static readonly RoutedUICommand DeleteLibrary = new RoutedUICommand("Delete Library", "DeleteLibrary", typeof(BrowserWindow));
        public static readonly RoutedUICommand SearchGoogleForImage = new RoutedUICommand("Search Google For Image", "SearchGoogleForImage", typeof(BrowserWindow));
    }
}

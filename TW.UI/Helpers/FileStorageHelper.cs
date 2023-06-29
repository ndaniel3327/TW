namespace TW.UI.Helpers
{
    public static class FileStorageHelper
    {
        private static char _separator = '@';
        private static readonly int _indexOfId = 0;
        private static readonly int _indexOfName = 1;
        private static readonly int _indexOfSelected = 2;

        public static string ReturnId(string playlist)
        {
            var contents = playlist.Split(_separator);
            string id = contents[_indexOfId].Substring(contents[_indexOfId].IndexOf("id=") + 3);
            return id;
        }

        public static string ReturnName(string playlist)
        {
            var contents = playlist.Split(_separator);
            string fullProperty = contents[_indexOfName];
            string name = fullProperty.Substring(fullProperty.IndexOf("name=") + 5);
            return name;
        }

        public static string ReturnSelected(string playlist)
        {
            var contents = playlist.Split(_separator);
            string selected = contents[_indexOfSelected].Substring(contents[_indexOfSelected].IndexOf("selected=") + 9);
            return selected;
        }
    }
}

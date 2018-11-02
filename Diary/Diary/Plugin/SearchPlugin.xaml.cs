using System;
using Xamarin.Forms;

namespace Diary.Plugin
{
    public delegate void SearchPluginEventHandler(string text);
    public partial class SearchPlugin : ContentView
    {
        public event SearchPluginEventHandler Search;

        public SearchPlugin()
        {
            InitializeComponent();
        }
        private void SearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Search?.Invoke(searchEntry.Text);
        }

        private void OnClicked(object sender, EventArgs e)
        {
            //TODO
            var button = (Button) sender;

            switch (button.BorderColor.ToString())
            {
                case "Red":

                    break;

                    
            }
        }
    }
}
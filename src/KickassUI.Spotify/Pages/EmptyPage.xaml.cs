using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace KickassUI.Spotify.Pages
{
    public partial class EmptyPage : ContentPage
    {
        public EmptyPage()
        {
            InitializeComponent();

            // Hide the navbar on Android
            if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}

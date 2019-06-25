﻿using System;

using OnePomodoro.ViewModels;

using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using OnePomodoro.Helpers;
using Windows.UI.Xaml;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Navigation;

namespace OnePomodoro.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class OptionsPage : Page
    {
        private OptionsViewModel ViewModel => DataContext as OptionsViewModel;

        public OptionsPage()
        {
            InitializeComponent();
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
            SystemNavigationManager.GetForCurrentView().BackRequested += BlankPage1_BackRequested;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);
            //var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            UpdateTitleBarLayout(coreTitleBar);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TitleBarHelper.UpdatePageTitleColor(this);
        }

        private void BlankPage1_BackRequested(object sender, BackRequestedEventArgs e)
        {
            On_BackRequested();
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            //LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            //RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
            //TitleBarButton.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayRightInset, 0);

            // Update title bar control size as needed to account for system size changes.
            BackButton.Height = coreTitleBar.Height;
            AppTitleBar.Height = coreTitleBar.Height;
        }


        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
           
        }

        private void OnBackClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }
    }
}

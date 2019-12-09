﻿using System;
using OnePomodoro.PomodoroViews;
using OnePomodoro.Services;
using OnePomodoro.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI.ViewManagement;
using OnePomodoro.Helpers;

namespace OnePomodoro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        private Type _pomodoroViewType;

        private bool _isInCompactOverlay;

        private bool _canEnterCompactOverlay;

        private CompactOverlayAttribute _currentCompactOverlayAttribute;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Window.Current.SizeChanged += OnWindowCurrentSizeChanged;
            //ChangePomodoroContent(typeof(TheFirst));
        }

        private void OnWindowCurrentSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            UpdateButtonsVisibility();
        }

        private void OnOptionsClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OptionsPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //Window.Current.SetTitleBar(null);

            var viewType = PomodoroView.Views.FirstOrDefault(t => t.Name == (SettingsService.Current.ViewType ?? string.Empty));

            if (viewType == null)
                viewType = PomodoroView.Views.FirstOrDefault();

            if (_pomodoroViewType != viewType)
                ChangePomodoroContent(viewType);

            _pomodoroViewType = viewType;
        }


        private void ChangePomodoroContent(Type type)
        {
            var view = Activator.CreateInstance(type) as PomodoroView;
            PomodoroContent.Content = view;
            RequestedTheme = (view as FrameworkElement).RequestedTheme;

            var attributes = type.GetCustomAttributes(true);
            _currentCompactOverlayAttribute = attributes.OfType<CompactOverlayAttribute>().FirstOrDefault();
            UpdateButtonsVisibility();
        }


        private async void OnPinClick(object sender, RoutedEventArgs e)
        {
            var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            preferences.CustomSize = new Windows.Foundation.Size(_currentCompactOverlayAttribute.CustomWidth, _currentCompactOverlayAttribute.CustomHeight);
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, preferences);
            _isInCompactOverlay = true;
            UpdateButtonsVisibility();
        }

        private async void OnUnpinClick(object sender, RoutedEventArgs e)
        {
            var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.Default);
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default, preferences);
            _isInCompactOverlay = false;
            UpdateButtonsVisibility();
        }

        private void OnFullScreenClick(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }

        private void UpdateButtonsVisibility()
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                FullScreenButton.Visibility = Visibility.Collapsed;
                PinButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (_isInCompactOverlay)
                {
                    FullScreenButton.Visibility = Visibility.Collapsed;
                    OptionsButton.Visibility = Visibility.Collapsed;
                    PinButton.Visibility = Visibility.Collapsed;
                    UnpinButton.Visibility = Visibility.Visible;
                }
                else
                {
                    FullScreenButton.Visibility = Visibility.Visible;
                    OptionsButton.Visibility = Visibility.Visible;
                    UnpinButton.Visibility = Visibility.Collapsed;
                    PinButton.Visibility = _currentCompactOverlayAttribute == null ? Visibility.Collapsed : Visibility.Visible;
                }

            }
        }
    }
}

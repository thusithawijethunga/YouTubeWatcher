﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using SharedCode.SQLite;
using System.Diagnostics;
using Windows.Storage;
using Windows.System;

namespace MediaLibrary
{
    

    public sealed partial class MainPage : Page
    {
        private const string mediaPath = "d:\\deleteme\\downloadedMedia";
        private const string dbName = "youtubewatcher";
        private const string youtubeHomeUrl = "https://www.youtube.com";
        private const double taskbarHeight = 40;

        public MainPage()
        {
            this.InitializeComponent();

            // initialize the sqlite db
            AppDatabase.Current(mediaPath, dbName).Init();

            // setup views
            SetupLibraryView();
            SetupMediaview();
        }

        private void SetupLibraryView()
        {
            ShowHideLibrary(true);
            tbMediaDirectory.Text = mediaPath;
        }

        private void SetupMediaview() {
           
        }

        private void CloseLibrary(object sender, RoutedEventArgs e) => ShowHideLibrary(false);

        private void ShowMediaFolder(object sender, RoutedEventArgs e) => OpenMediaFolder();

        private void PlayMedia(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;
            if (but.DataContext is ViewMediaMetadata)
            {
                var vmd = (ViewMediaMetadata)but.DataContext;

                //mePlayer.Source = new Uri($"{mediaPath}\\{vmd.YID}.mp4", UriKind.Absolute);
                ShowHideMediaPlayer(true);
            }
        }

        private void ShowHideMediaPlayer(bool show)
        {
            //if (show)
            //{
            //    grdMediaPlayer.Visibility = Visibility.Visible;
            //    isPlaying = true;
            //    mePlayer.Play();
            //}
            //else
            //{
            //    mePlayer.Stop();
            //    isPlaying = false;
            //    mePlayer.Source = null;
            //    mePlayerSlider.Value = 0;
            //    grdMediaPlayer.Visibility = Visibility.Collapsed;
            //}
        }
        
        //bool isPlaying = false;
        //private void TogglePausePlay()
        //{
        //    if (isPlaying) mePlayer.Pause();
        //    else mePlayer.Play();
        //    isPlaying = !isPlaying;
        //}

        private void ShowHideLibrary(bool show)
        {
            //wvMain.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
            grdLibrary.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            LoadLibraryItems(show);
            //ShowHideMediaPlayer(false);
        }

        private async void OpenMediaFolder()
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(mediaPath);
            await Launcher.LaunchFolderAsync(folder);

            //Process process = new Process();
            //process.StartInfo.UseShellExecute = true;
            //process.StartInfo.FileName = mediaPath;
            //process.Start();
        }

        private void LoadLibraryItems(bool load)
        {
            if (load)
            {
                var MediaItems = new ObservableCollection<ViewMediaMetadata>();
                var foundItems = DBContext.Current.RetrieveAllEntities<MediaMetadata>();
                foundItems.Reverse();
                foreach (var foundItem in foundItems)
                {
                    MediaItems.Add(new ViewMediaMetadata()
                    {
                        Title = foundItem.Title,
                        YID = foundItem.YID,
                        ThumbUri = new Uri($"{mediaPath}\\{foundItem.YID}-medium.jpg", UriKind.Absolute),
                        Quality = foundItem.Quality,
                        MediaType = foundItem.MediaType,
                        Size = foundItem.Size,
                    });

                }
                icLibraryItems.ItemsSource = MediaItems;
            }
            else
            {
                //icLibraryItems.Items.Clear();
                icLibraryItems.ItemsSource = null;
            }
        }

        private void grdMediaPlayer_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        private void CloseMediaPlayer(object sender, RoutedEventArgs e)
        {

        }

        private void ScrubMedia(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        private void mePlayer_MediaOpened(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void ShowMediaFolder(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }


    public struct ViewMediaMetadata
    {
        public string YID { get; set; }
        public string Title { get; set; }
        public Uri ThumbUri { get; set; }
        public string Quality { get; set; }
        public string MediaType { get; set; }
        public long Size { get; set; }
    }
}


// add permission to folder, for all access to files
// https://www.mr-kg.com/uwp-file-access/

// add restricted capability to allow opening folder
// https://stackoverflow.com/questions/50559764/broadfilesystemaccess-uwp
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPPhotoGallery.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPPhotoGallery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPhotostoAlbumPage : Page
    {
        public ObservableCollection<Photo> PhotoCollection;
        public ObservableCollection<Photo> SelectedPhotos;
        private Photo coverPhoto;
        public AddPhotostoAlbumPage()
        {
            this.InitializeComponent();
            PhotoCollection = new ObservableCollection<Photo>();
            SelectedPhotos = new ObservableCollection<Photo>();
            this.Loaded += AddPhotostoAlbumPage_Loaded;
            
        }

        private async void AddPhotostoAlbumPage_Loaded(object sender, RoutedEventArgs e)
        {
            await PhotoManager.GetPhotosAsync(PhotoCollection);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<Photo> selectedPhotos = new List<Photo>();
            
            var photos = PhotoSelectedGrid.Items.ToList();
            foreach(Photo p in photos) { selectedPhotos.Add(p); }
            PhotoManager.selectedPhotos = selectedPhotos;

            if (coverPhoto == null)
            {
                //pick any image from the seelcted photos
                coverPhoto = selectedPhotos[0];
            }
            //album name
            string albumName;
            if (AlbumName.Text == String.Empty)
            {
                int count = PhotoManager.albums.Count + 1;
                albumName = $"Album{count}";

            }
            else
            {
                albumName = AlbumName.Text;
            }

            //add this album
            Album newalbum = new Album
            {
                CoverPhotoFile = coverPhoto.imageFile,
                Name = albumName,
                CoverImage = (BitmapImage)coverPhoto.Thumbnail

            };


            PhotoManager.AddAlbum(newalbum);
            PhotoManager.SetAlbumNameinSelectedPhotos(albumName);
            this.Frame.Navigate(typeof(AlbumsPage));
            //go to add albums page now
            //this.Frame.Navigate(typeof(AddAlbumPage));

        }

        private void PhotoSelectionGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //just enbale add button
            AddButton.IsEnabled = true;
            AlbumName.Visibility = Visibility.Visible;
            var photos = PhotoSelectionGrid.SelectedItems.ToList();
            SelectedPhotos.Clear();
            foreach (Photo p in photos) { SelectedPhotos.Add(p); }
        }

      

        private void Photo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Photo selected = PhotoSelectedGrid.SelectedItem as Photo;
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
           
               
            coverPhoto = PhotoSelectedGrid.SelectedItem as Photo;
        }
    }
}

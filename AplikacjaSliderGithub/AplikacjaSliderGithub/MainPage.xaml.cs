using AplikacjaSliderGithub.Classes;
using AplikacjaSliderGithub.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AplikacjaSliderGithub
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadImages();
        }

        public void LoadImages()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SlidersImages");
            List<Images> imagesList = new List<Images>();

            if (Directory.Exists(path))
            {
                string[] imageFiles = Directory.GetFiles(path);

                foreach (var file in imageFiles)
                {
                    imagesList.Add(new Images { ImageSource = FileImageSource.FromFile(file), FilePath = file });
                }
            }
            else
            {
                Console.WriteLine("Ścieżka do folderu z obrazami nie istnieje.");
            }

            imageCarousel.ItemsSource = imagesList;
        }

        private async void GoToYourGallery(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    await Navigation.PushAsync(new EditImagePage(photo.FullPath));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił błąd: {ex.Message}", "OK");
            }
        }

        private void DeleteImage(object sender, EventArgs e)
        {
            var button = sender as Button;
            var image = button?.BindingContext as Images;

            if (image != null)
            {
                if (File.Exists(image.FilePath))
                {
                    File.Delete(image.FilePath);
                    LoadImages();
                }
            }
        }
    }
}

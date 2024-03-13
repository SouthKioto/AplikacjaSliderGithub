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
        }

        private async void GoToYourGallery(object sender, EventArgs e)
        {
            try
            {
                // Otwieranie folderu z obrazami
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    // Przekierowanie użytkownika do strony edycji obrazu
                    await Navigation.PushAsync(new EditImagePage(photo.FullPath));
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                await DisplayAlert("Błąd", $"Wystąpił błąd: {ex.Message}", "OK");
            }
        }
    }
}

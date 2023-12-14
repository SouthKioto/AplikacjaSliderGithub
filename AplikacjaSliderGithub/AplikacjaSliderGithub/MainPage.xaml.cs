using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AplikacjaSliderGithub
{
    public partial class MainPage : ContentPage
    {

        private List<Images> images;
        private int currentIndex = 0;
        private bool isAutoSlideEnabled = true;

        public MainPage()
        {
            InitializeComponent();

            images = new List<Images>
            {
                new Images { ImageSource = "Bleach.jpg", ImageName="Bleach plakat 1" },
                new Images { ImageSource = "bleach2.jpg", ImageName="Bleach plakat 2" },
                new Images { ImageSource = "hellboy.jpg", ImageName="Hellboy 2" },
            };

            imageCarousel.ItemsSource = images;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (isAutoSlideEnabled)
                    MoveToNextSlide();

                return true;
            });
        }

        private void MoveToNextSlide()
        {
            currentIndex = (currentIndex + 1) % images.Count;
            imageCarousel.Position = currentIndex;
        }

        private void OnToggleButtonClicked(object sender, EventArgs e)
        {
            isAutoSlideEnabled = !isAutoSlideEnabled;
            toggleButton.Text = isAutoSlideEnabled ? "Stop Auto Slide" : "Start Auto Slide";
        }
    }
}

using AplikacjaSliderGithub.Classes;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace AplikacjaSliderGithub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditImagePage : ContentPage
    {
        SKBitmap bitmap;
        SKPaint paint;

        private string AddedText;
        private float x;
        private float y;
        private float textSize;

        public EditImagePage(string imagePath)
        {
            InitializeComponent();

            bitmap = SKBitmap.Decode(imagePath);
            skiaView.PaintSurface += OnPaintSurface;
        }

        private void DisplayCustomPromptAsync()
        {
            string text = "";
            float x = 0;
            float y = 0;
            float textSize = 0;

            //TextSettings data = new TextSettings();

            StackLayout layout = new StackLayout();
            Entry textEntry = new Entry { Placeholder = "Enter text" };
            Entry xEntry = new Entry { Placeholder = "Enter X position" };
            Entry yEntry = new Entry { Placeholder = "Enter Y position" };
            Entry textSizeEntry = new Entry { Placeholder = "Enter text size" };
            Button addData = new Button { Text = "Add Data and Back" };

            addData.Clicked += (sender, e) =>
            {
                text = textEntry.Text;
                float.TryParse(xEntry.Text, out x);
                float.TryParse(yEntry.Text, out y);
                float.TryParse(textSizeEntry.Text, out textSize);

                if (string.IsNullOrEmpty(textEntry.Text))
                    text = "Przykładowy tekst";

                AddedText = text;
                this.x = x;
                this.y = y;
                this.textSize = textSize;
                addData.IsEnabled = false;
                skiaView.InvalidateSurface();
                Navigation.PopModalAsync();
            };

            layout.Children.Add(textEntry);
            layout.Children.Add(xEntry);
            layout.Children.Add(yEntry);
            layout.Children.Add(textSizeEntry);
            layout.Children.Add(addData);

            ContentPage page = new ContentPage();
            page.Content = layout;

            Navigation.PushModalAsync(page);

            //return data;
        }

        private void AddTextOnPhoto(object sender, EventArgs e)
        {
            DisplayCustomPromptAsync();           
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(bitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height));

            if (!string.IsNullOrWhiteSpace(AddedText))
            {
                paint = new SKPaint
                {
                    Color = SKColors.Red,
                    TextSize = textSize,
                    IsAntialias = true,
                };

                canvas.DrawText(AddedText, x, y, paint);
            }
        }

        private void SaveChanges_Clicked(object sender, EventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var pathDir = Path.Combine(path, "SlidersImages");

            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            string fileName = $"EditedImage_{DateTime.Now:yyyyMMddHHmmss}.png";
            string filePath = Path.Combine(pathDir, fileName);

            // Tworzenie nowego obrazu z tekstem
            using (var newBitmap = new SKBitmap(bitmap.Width, bitmap.Height))
            {
                using (var canvas = new SKCanvas(newBitmap))
                {

                    canvas.DrawBitmap(bitmap, new SKPoint(0, 0));
                    if (!string.IsNullOrWhiteSpace(AddedText))
                    {
                        using (var paint = new SKPaint
                        {
                            Color = SKColors.Red,
                            TextSize = textSize,
                            IsAntialias = true,
                        })
                        {
                            canvas.DrawText(AddedText, x, y, paint);
                        }
                    }
                }

                using (var image = SKImage.FromBitmap(newBitmap))
                using (var data = image.Encode())
                using (var stream = File.OpenWrite(filePath))
                {
                    data.SaveTo(stream);
                }
            }

            DisplayAlert("Success", "Image saved successfully", "OK");
        }
    }
}

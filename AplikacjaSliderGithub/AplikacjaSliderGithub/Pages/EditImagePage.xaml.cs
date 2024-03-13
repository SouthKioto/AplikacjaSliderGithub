using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AplikacjaSliderGithub.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditImagePage : ContentPage
    {
        SKBitmap bitmap;
        SKPaint paint;

        private string AddedText;
        private float x = 50; // Domyślna wartość x
        private float y = 50; // Domyślna wartość y

        public EditImagePage(string imagePath)
        {
            InitializeComponent();

            bitmap = SKBitmap.Decode(imagePath);
            skiaView.PaintSurface += OnPaintSurface;

            paint = new SKPaint
            {
                Color = SKColors.Red,
                TextSize = 50,
                IsAntialias = true,
            };
        }

        private async Task<(string, float, float)> DisplayCustomPromptAsync()
        {
            string text = "";
            float x = 0;
            float y = 0;

            StackLayout layout = new StackLayout();
            Entry textEntry = new Entry { Placeholder = "Enter text", Text="Przykładowy tekst" };
            Entry xEntry = new Entry { Placeholder = "Enter X position" };
            Entry yEntry = new Entry { Placeholder = "Enter Y position" };
            Button addData = new Button { Text = "Add Data and Back" };
            addData.Clicked += BackModal;
            layout.Children.Add(textEntry);
            layout.Children.Add(xEntry);
            layout.Children.Add(yEntry);
            layout.Children.Add(addData);

            ContentPage page = new ContentPage();
            page.Content = layout;
            await Navigation.PushModalAsync(page);

            if (String.IsNullOrEmpty(text))
            {
                text = "Przykładowy tekst";
                x = 0;
                y = 0;
            }
            else
            {
                text = textEntry.Text;
                x = float.Parse(xEntry.Text);
                y = float.Parse(yEntry.Text);
            }


            return (text, x, y);
            
            
        }

        public void BackModal(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void AddTextOnPhoto(object sender, EventArgs e)
        {
            (string text, float x, float y) = await DisplayCustomPromptAsync();
            if (!string.IsNullOrWhiteSpace(text))
            {
                AddedText = text;
                this.x = x;
                this.y = y;
                skiaView.InvalidateSurface();
            }
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(bitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height));

            if (!string.IsNullOrWhiteSpace(AddedText))
            {
                canvas.DrawText(AddedText, x, y, paint);
            }
        }

        private void SaveChanges_Clicked(object sender, EventArgs e)
        {

        }
    }
}

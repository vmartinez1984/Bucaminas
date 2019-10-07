using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Buscaminas
{
    /// <summary>
    /// Lógica de interacción para WinYouWin.xaml
    /// </summary>
    public partial class WinYouWin : Window
    {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        public WinYouWin()
        {
            InitializeComponent();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImagenFinal.Source = GetImagen(Properties.Resources.BanderaSovieticaReichstag);
            player.SoundLocation = @"C:\Users\vmartinez\Videos\SovietHimno.wav";
            player.Play();
        }

        private ImageSource GetImagen(System.Drawing.Bitmap caraFeliz)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(caraFeliz.GetHbitmap(),
                                      IntPtr.Zero,
                                      Int32Rect.Empty,
                                      BitmapSizeOptions.FromEmptyOptions());
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            player.Stop();
        }
    }
}
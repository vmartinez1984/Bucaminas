using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tool.Convertir
{
    public class Imagen
    {
        public static ImageSource Bytes2ImageSource(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        public static Bitmap Bytes2Bitmap(byte[] bytesImg)
        {
            try
            {
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(bytesImg);
                Bitmap bitmap1 = new Bitmap(img);
                return bitmap1;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Takes a bitmap and converts it to an image that can be handled by WPF ImageBrush
        /// </summary>
        /// <param name="src">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF</returns>
        public static BitmapImage Bitmap2BitmapImage(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            return new Bitmap(bitmapImage.StreamSource);
        }

        public static Bitmap Get24bppRgb(Image image)
        {
            var bitmap = new Bitmap(image);
            var bitmap24 = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bitmap24))
            {
                gr.DrawImage(bitmap, new Rectangle(0, 0, bitmap24.Width, bitmap24.Height));
            }
            return bitmap24;
        }

        public static Bitmap BinaryImage(Bitmap source, int umb)
        {
            try
            {
                // Bitmap con la imagen binaria
                Bitmap target = new Bitmap(source.Width, source.Height, source.PixelFormat);
                // Recorrer pixel de la imagen
                for (int i = 0; i < source.Width; i++)
                {
                    for (int e = 0; e < source.Height; e++)
                    {
                        // Color del pixel
                        System.Drawing.Color col = source.GetPixel(i, e);
                        // Escala de grises
                        byte gray = (byte)(col.R * 0.3f + col.G * 0.59f + col.B * 0.11f);
                        // Blanco o negro
                        byte value = 0;
                        if (gray > umb)
                        {
                            value = 255;
                        }
                        // Asginar nuevo color
                        System.Drawing.Color newColor = System.Drawing.Color.FromArgb(value, value, value);
                        target.SetPixel(i, e, newColor);

                    }
                }
                return target;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {

            BitmapData bmpdata = null;

            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }

        }

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;

            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);


            memoryStream.Position = 0;


            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);


            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;

            return bmpReturn;
        }
    }
}

using System.Drawing;

namespace Utilerias
{
    public class ImgUtil
    {
        public static byte[] bmptobyte(Bitmap bmp)
        {
            return bmptobyte(bmp, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static byte[] bmptobyte(Bitmap bmp, System.Drawing.Imaging.ImageFormat format)
        {
            try
            {
                if (bmp == null) return null;

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bmp.Save(ms, format);
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                byte[] b = new byte[ms.Length];
                ms.Read(b, 0, (int)ms.Length);

                ms.Close();

                return b;
            }
            catch
            {
                return null;
            }



        }

        public static Bitmap CrearImagen(byte[] b)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                Bitmap bmp = new Bitmap(ms);

                return bmp;
            }
            catch
            {
                return null;
            }
        }


        public static Bitmap CargaImagen(string ruta)
        {
            byte[] b = System.IO.File.ReadAllBytes(ruta);

            return CrearImagen(b);

        }


        public static Bitmap AjustaTamanoBitmap(Bitmap bmp, int MaxW, int MaxH)
        {
            Bitmap retBitmap = new Bitmap(bmp);

            if (retBitmap.Width >= MaxW)
            {
                int newh = (int)((float)bmp.Height * (float)MaxW / (float)bmp.Width);

                Bitmap temp = new Bitmap(MaxW, newh);
                Graphics g = Graphics.FromImage(temp);

                g.DrawImage(retBitmap, new Rectangle(0, 0, MaxW, newh), new Rectangle(0, 0, retBitmap.Width, retBitmap.Height), GraphicsUnit.Pixel);

                retBitmap.Dispose();
                retBitmap = temp;
            }

            if (retBitmap.Height >= MaxH)
            {
                int neww = (int)((float)bmp.Width * (float)MaxH / (float)bmp.Height);

                Bitmap temp = new Bitmap(neww, MaxH);
                Graphics g = Graphics.FromImage(temp);

                g.DrawImage(retBitmap, new Rectangle(0, 0, neww, MaxH), new Rectangle(0, 0, retBitmap.Width, retBitmap.Height), GraphicsUnit.Pixel);

                retBitmap.Dispose();
                retBitmap = temp;
            }

            return new Bitmap(retBitmap);
        }

    }
}
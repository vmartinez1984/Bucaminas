using Buscaminas.Dao;
using Buscaminas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Buscaminas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        DispatcherTimer Timer = new DispatcherTimer();
        int t = 0;
        int MinasTotal = 10;
        int BanderasTotal = 10;
        List<Casilla> LstCasillas = new List<Casilla>();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            PnlMain.IsEnabled = false;

            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);
        }

        private ImageSource GetImagen(System.Drawing.Bitmap caraFeliz)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(caraFeliz.GetHbitmap(),
                                      IntPtr.Zero,
                                      Int32Rect.Empty,
                                      BitmapSizeOptions.FromEmptyOptions());
        }

        private void FillCasillas()
        {
            PnlMain.Children.Clear();
            LstCasillas = CasillaDao.GetLst();
            for (int i = 0; i < LstCasillas.Count; i++)
            {
                var btn = new Button();
                Image image = new Image();
                image.Stretch = System.Windows.Media.Stretch.Fill;
                Label label = new Label();
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Add(image);
                //label.Content = LstCasillas[i].Id;
                stackPanel.Children.Add(label);
                btn.Content = stackPanel;
                btn.Tag = LstCasillas[i].Id;

                btn.Click += Btn_Click;
                btn.MouseRightButtonDown += Btn_MouseRightButtonDown;
                btn.MouseRightButtonUp += Btn_MouseRightButtonUp;
                btn.PreviewMouseUp += Btn_PreviewMouseUp;
                btn.PreviewMouseDown += Btn_PreviewMouseDown;

                PnlMain.Children.Add(btn);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TxtTiempo.Text = t.ToString().PadLeft(3, '0');
            t++;
        }

        private void Btn_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);
        }

        private void Btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraSorprendido);
        }


        private void Btn_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);
        }


        private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //cambio de carita 
            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraSorprendido);

            #region Variables
            int id = 0;
            var stackpanel = new StackPanel();
            var image = new Image();
            var casilla = new Casilla();
            var button = new Button();
            var banderasUsadas = 0;
            #endregion

            button = sender as Button;
            id = Convert.ToInt32(button.Tag);
            stackpanel = button.Content as StackPanel;
            image = stackpanel.Children[0] as Image;
            casilla = LstCasillas.Where(x => x.Id == id).FirstOrDefault(); 
            banderasUsadas = LstCasillas.Where(x => x.IsBandera).Count();
            if (image.Source == null)
            {
                if (banderasUsadas < BanderasTotal)
                {
                    image.Source = GetImagen(Properties.Resources.Bandera);
                    casilla.IsBandera = true;
                    if (casilla.IsMina)
                        casilla.IsBanderaCorrecta = true;
                }
            }
            else
            {
                image.Source = null;
                casilla.IsBandera = false;
                casilla.IsBanderaCorrecta = false;
            }

            stackpanel.Children[0] = image;

            if (LstCasillas.Count(x => x.IsBanderaCorrecta == true) == MinasTotal)
            {
                Timer.Stop();
                PnlMain.IsEnabled = false;
                MessageBox.Show("Congratulations");
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            #region Variables
            int id = 0;
            var stackpanel = new StackPanel();
            var image = new Image();
            var casilla = new Casilla();
            #endregion

            var button = sender as Button;
            id = Convert.ToInt32(button.Tag);
            casilla = LstCasillas.Where(x=>x.Id == id).FirstOrDefault();
            stackpanel = button.Content as StackPanel;

            if (casilla.IsMina == true)
            {
                image = stackpanel.Children[0] as Image;
                image.Source = GetImagen(Properties.Resources.Explosion);
                BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraLlorando);
                Timer.Stop();
                PnlMain.IsEnabled = false;
            }
            else
            {
                button.IsEnabled = false;
                button.Opacity = 0.3;
                var label = stackpanel.Children[1] as Label;
                label.Content = casilla.NumeroDeMinasCerca.ToString();
                image = stackpanel.Children[0] as Image;
                image.Source = null;
            }
            stackpanel.Children[0] = image;
        }


        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            t = 0;
            Timer.Start();
            PnlMain.IsEnabled = true;
            FillCasillas();
            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
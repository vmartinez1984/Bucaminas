using Buscaminas.Dao;
using Buscaminas.Dto;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Tool.Convertir;

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
        bool IsIniciado = false;
        int BombasTotal = 10;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Timer.Tick += new EventHandler(Timer_Tick); 
            Timer.Interval = new TimeSpan(0, 0, 1);

            FillCasillas();
        }

        private void FillCasillas()
        {
            var lst = CasillaDao.GetLst();
            for (int i = 0; i < lst.Count; i++)
            {
                var btn = new Button();
                Image image = new Image();
                image.Stretch = System.Windows.Media.Stretch.Fill;
                Label label = new Label();
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Add(image);
                label.Content = lst[i].Id;
                stackPanel.Children.Add(label);
                btn.Content = stackPanel;
                btn.Tag = lst[i];
                //btn.IsEnabled = false;
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
            BtnIniciarImg.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.CaraFeliz);
        }

        private void Btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            BtnIniciarImg.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.CaraSorprendido);
           
        }


        private void Btn_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnIniciarImg.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.CaraFeliz);
        }


        private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            BtnIniciarImg.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.CaraSorprendido);
            var button = sender as Button;
            var casilla = button.Tag as Casilla;
            var stackpanel = button.Content as StackPanel;
            var image = stackpanel.Children[0] as Image;

            if(casilla.Estado == Estado.SinAbrir)
            {
                casilla.Estado = Estado.Bandera;
            }
            if (casilla.Estado == Estado.Duda)
            {
                casilla.Estado = Estado.Bandera;
            }
            if (casilla.Estado == Estado.Bandera)
            {
                casilla.Estado = Estado.Duda;
            }

            switch (casilla.Estado)
            {
                case Estado.SinAbrir:

                    break;
                case Estado.Abierta:
                    break;
                case Estado.Duda:
                    image.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.Duda);
                    break;
                case Estado.Bandera:
                    image.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.Bandera);
                    break;
                default:
                    break;
            }
            stackpanel.Children[0] = image;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var casilla = button.Tag as Casilla;
            var stackpanel = button.Content as StackPanel;

            if (casilla.IsMina == true)
            {
                var image = stackpanel.Children[0] as Image;
                image.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.Explosion);
                BtnIniciarImg.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.CaraLlorando);
                stackpanel.Children[0] = image;
                Timer.Stop();
            }
            else
            {
                button.IsEnabled = false;
                var label = stackpanel.Children[1] as Label;
                label.Content = casilla.NumeroDeMinasCerca.ToString();
            }
        }


        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (!IsIniciado)
                Timer.Start();
        }
    }
}

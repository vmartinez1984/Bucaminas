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
        const string ImgNombre = "ImgNombre";
        const string LblNombre = "LblNombre";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Eventos
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            PnlMain.IsEnabled = false;

            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);
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

            TxtMinasNumero.Text = (BanderasTotal - LstCasillas.Count(x => x.IsBandera)).ToString();
            stackpanel.Children[0] = image;

            VerificarSiYaGano();
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
            casilla = LstCasillas.Where(x => x.Id == id).FirstOrDefault();
            stackpanel = button.Content as StackPanel;

            if (casilla.IsMina == true)
            {
                image = stackpanel.Children[0] as Image;
                image.Source = GetImagen(Properties.Resources.Explosion);
                BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraLlorando);
                Timer.Stop();
                PnlMain.IsEnabled = false;

                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = @"C:\Users\vmartinez\Videos\Explosion.wav";
                player.Play();

                //Mostrar la minas
                ShowMinas(casilla.Id);
            }
            else
            {
                CasillaAbrir(casilla.Id);
                var lstContguasConCero = new List<Casilla>();
                var lstContiguas = new List<Casilla>();

                //solo si su número de minas cerca es cero
                if (casilla.NumeroDeMinasCerca == 0)
                {
                    //obtner las casillas continuas
                    lstContguasConCero = GetLstCasillasContiguas(casilla, true);

                    //abrimos las casillas
                    var length = lstContguasConCero.Count;
                    for (int i = 0; i < length; i++)
                    {
                        var item = lstContguasConCero[i];
                        if (item.IsBandera == false)
                        {
                            CasillaAbrir(item.Id);
                            lstContguasConCero.AddRange(GetLstCasillasContiguas(item, true));
                            foreach (var itemConCero in lstContguasConCero)
                            {
                                CasillaAbrir(itemConCero.Id);
                            }
                            length = lstContguasConCero.Count;
                        }
                    }

                    foreach (var item in lstContguasConCero)
                    {
                        lstContiguas.AddRange(GetLstCasillasContiguas(item, false));
                    }

                    foreach (var item in lstContiguas)
                    {
                        CasillaAbrir(item.Id);
                    }
                }
            }
        }//Btn_Click

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            t = 0;
            Timer.Start();
            PnlMain.IsEnabled = true;
            FillCasillas();
            BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);

            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @"C:\Users\vmartinez\Videos\Marcha.wav";
            player.PlayLooping();

        }
        #endregion

        #region Funciones
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
                image.Name = ImgNombre + LstCasillas[i].Id.ToString().PadLeft(3, '0');
                image.Stretch = System.Windows.Media.Stretch.Fill;
                Label label = new Label();
                label.Name = LblNombre + LstCasillas[i].Id.ToString().PadLeft(3, '0');
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

        void VerificarSiYaGano()
        {
            if (LstCasillas.Count(x => x.IsBanderaCorrecta == true) == MinasTotal)
            {
                Timer.Stop();
                PnlMain.IsEnabled = false;
                BtnIniciarImg.Source = GetImagen(Properties.Resources.CaraFeliz);
                WinYouWin winYouWin = new WinYouWin();
                winYouWin.ShowDialog();
            }
        }

        void CasillaAbrir(int id)
        {
            #region Variables
            var casilla = new Casilla();
            var stackpanel = new StackPanel();
            var image = new Image();
            var label = new Label();
            #endregion

            //obtenermos la casilla de la lista
            casilla = LstCasillas.Where(x => x.Id == id).FirstOrDefault();
            foreach (Button item in PnlMain.Children)
            {
                if (id == Convert.ToInt32(item.Tag))
                {
                    item.IsEnabled = false;
                    item.Opacity = 0.3;
                    stackpanel = item.Content as StackPanel;
                    label = stackpanel.Children[1] as Label;
                    label.Content = casilla.NumeroDeMinasCerca.ToString();
                    image = stackpanel.Children[0] as Image;
                    image.Source = null;
                    break;
                }
            }
            //Pasamo el estado de la casilla a abierta            
            casilla.Estado = Estado.Abierta;
        }

        /// <summary>
        /// Obtiene una lista de casillas contiguas que tienen cero o diferente a cero
        /// </summary>
        /// <param name="casilla"></param>
        /// <param name="isMinaCero">true es con cero, false diferente a cero</param>
        /// <returns></returns>
        private List<Casilla> GetLstCasillasContiguas(Casilla casilla, bool isMinaCero)
        {
            var lst = new List<Casilla>();
            var casillaContigua = new Casilla();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    casillaContigua = LstCasillas.Where(_casilla => _casilla.X == (casilla.X + i) && _casilla.Y == (casilla.Y + j)).FirstOrDefault();
                    if (casillaContigua != null)
                        lst.Add(casillaContigua);
                }
            }
            //obtnemos las que no esten abierta
            lst = lst.Where(x => x.Estado == Estado.SinAbrir).ToList();
            if (isMinaCero)
            {
                //obtenemos las que el número de minas sea 0
                lst = lst.Where(x => x.NumeroDeMinasCerca == 0).ToList();
            }
            else
            {
                //obtenemos las que el número de minas sea 0
                lst = lst.Where(x => x.NumeroDeMinasCerca != 0).ToList();
            }
            return lst;
        }

        private void ShowMinas(int idConMina)
        {
            #region Variables
            var id = 0;
            var casilla = new Casilla();
            var stackpanel = new StackPanel();
            var image = new Image();
            var label = new Label();
            #endregion
            foreach (Button item in PnlMain.Children)
            {
                id = Convert.ToInt32(item.Tag);
                if (id != idConMina)
                {
                    casilla = LstCasillas.Where(x => x.Id == id).FirstOrDefault();
                    item.IsEnabled = false;
                    item.Opacity = 0.3;
                    stackpanel = item.Content as StackPanel;
                    label = stackpanel.Children[1] as Label;
                    image = stackpanel.Children[0] as Image;

                    if (casilla.IsMina && casilla.IsBandera)
                        image.Source = GetImagen(Properties.Resources.Correcto);
                    else if (!casilla.IsMina && casilla.IsBandera)
                        image.Source = GetImagen(Properties.Resources.Error);
                    else if (casilla.IsMina && !casilla.IsBandera)
                        image.Source = GetImagen(Properties.Resources.Mina1);
                    else
                        image.Source = null;
                }
            }
        }//ShowMinas
        #endregion

        #region Barra de título
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

    }//fin de control
}//fin de namespace
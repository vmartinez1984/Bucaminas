using Buscaminas.Dao;
using Buscaminas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tool.Convertir;

namespace Buscaminas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var lst = CasillaDao.GetLst();
            for (int i = 0; i < lst.Count; i++)
            {
                var btn = new Button();
                Image image = new Image();
                Label label = new Label();
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Add(image);
                label.Content = lst[i].Id;
                stackPanel.Children.Add(label);
                btn.Content = stackPanel;
                btn.Tag = lst[i];
                //btn.IsEnabled = false;
                btn.Click += Btn_Click;
                PnlMain.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var casilla = button.Tag as Casilla;
            if (casilla.IsMina == true)
            {
                var stackpanel = button.Content as StackPanel;
                var image = stackpanel.Children[0] as Image;
                image.Source = Imagen.Bitmap2BitmapImage(Properties.Resources.Explosion);

                stackpanel.Children[0] = image;
                //stackpanel.Children.
            }
        }
    }
}

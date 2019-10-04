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
           var lst =  CasillaDao.GetLst();
            for (int i = 0; i <= lst.Count; i++)
            {
                var btn = new Button();
                btn.Content = lst[i].Id;
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
                MessageBox.Show("Valio pepino");
        }
    }
}

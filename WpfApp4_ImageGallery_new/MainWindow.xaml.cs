using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4_ImageGallery_new
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PhotoCollection Photos;
        public MainWindow()
        {
            ///
            InitializeComponent();
            Directory.CreateDirectory(Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImagePath"].ToString());

            // Photos = new PhotoCollection();
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Обзор
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
                if (Photos != null)
                    Photos.Clear();
               // Photos = (PhotoCollection)(this.Resources["Photos"] as ObjectDataProvider).Data;
                Photos.Path = dialog.SelectedPath;
            }
        }

        private void OnPhotoClick(object sender, MouseButtonEventArgs e)
        {
            var photo = (Photo)PhotosListBox.SelectedItem;
            //System.Windows.MessageBox.Show(photo.Source);
            Window1 wnd = new Window1();
            wnd.Width = 1366;
            wnd.Height = 768;
            wnd.imageBox1.Source = new BitmapImage(new Uri(photo.SourceOriginal));
            wnd.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) // Перед закрытие приложения
        {
            if (Photos != null)
                Photos.Clear();
            Directory.Delete(Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImagePath"].ToString(), true); //true - если директория не пуста (удалит и файлы и папки)

        }
    }
}

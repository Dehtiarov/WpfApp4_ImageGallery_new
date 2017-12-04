using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp4_ImageGallery_new
{
   
    public class Photo
    {
        private string _path;
        private string _pathOriginal; // путь к оригинальной фотке
        //private Uri _source;
        private BitmapFrame _image;

        public Photo(string path)
        {
            _pathOriginal = path;
            _path = Environment.CurrentDirectory + ConfigurationManager.AppSettings["ImagePath"].ToString() + Guid.NewGuid().ToString() + ".jpg";
            using (var image = System.Drawing.Image.FromFile(_pathOriginal))
            {
                var newImageSmall = ImageConverter.ImageWorker.ConverImageToBitmap(image, 320, 240);
                if (newImageSmall != null)
                {
                    newImageSmall.Save(_path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _image = BitmapFrame.Create(new Uri(_path));
                }
                //_source = new Uri(path);
            }
            
        }

        //public override string ToString()
        //{
        //    return _source.ToString();
        //}
        public string Source { get { return _path; } }

        public string SourceOriginal { get { return _pathOriginal; } }

        public BitmapFrame Image { get { return _image; } set { _image = value; } }

        public string ImageName { get { return Path.GetFileNameWithoutExtension(_pathOriginal); }  }

      
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Web;
using Thoughts.UI.WPF.ViewModels.Base;

namespace Thoughts.UI.WPF.ViewModels
{
    public class FilesViewModel : ViewModel
    {
        private readonly string _path = @"C:\Users\vital\source\repos\Thoughts\UI\Thoughts.UI.WPF\Data\Test\";

        private static ObservableCollection<TestFile> _filesCollection = new ObservableCollection<TestFile>();
        public static ObservableCollection<TestFile> FilesCollection { get => _filesCollection; set => _filesCollection = value; }


        public FilesViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            var x = Directory.GetFiles(_path);
            foreach (string str in x)
            {
                var tf = new TestFile();

                var fi = new FileInfo(str);
                if (fi.Exists)
                {
                    tf.Name = fi.Name;
                    tf.Path = str;
                }

                FilesCollection.Add(tf);
            }
        }

        public class TestFile
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Path { get; set; }
            public TestFile()
            {
            }
        }
    }
}

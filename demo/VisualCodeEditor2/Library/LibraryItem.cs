using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class LibraryItem : INotifyPropertyChanged
    {
        public Library Library { get; internal set; }
        public ImageSource Image { get; set; }
        public bool IsStoreLibrary { get; set; } = false;
        public string StoreId { get; set; } = null;
        public bool IsPurchased { get; set; } = true;
        public string Price { get; set; }
        public string Assembly { get
            {
                return Library.GetType().Assembly.GetName().Name;
            }
        }
        public void ChangeProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsImported { get; set; } = false;
    }
}

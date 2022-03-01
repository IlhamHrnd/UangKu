using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Root
{
    public class ConnectionErrorModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionErrorModel()
        {

        }

        public Command Refresh { get; set; }

        private bool isrefreshing = false;

        public bool IsRefreshing
        {
            get { return isrefreshing; }
            set { SetProperty(ref isrefreshing, value); }
        }

        private string message = "Jaringan Tidak Ditemukan";

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string message2 = "Silahkan Periksa Jaringan Anda";

        public string Message2
        {
            get { return message2; }
            set { SetProperty(ref message2, value); }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }
            else
            {
                backingStore = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}

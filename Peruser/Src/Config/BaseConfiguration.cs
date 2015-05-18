using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Peruser
{
    public abstract class BaseConfiguration : INotifyPropertyChanged
    {
        public static string Serialize(PeruserConfig thisConfiguration)
        {
            return JsonConvert.SerializeObject(thisConfiguration);
        }

        public static T Deserialize<T>(string thisConfiguration)
        {
            return JsonConvert.DeserializeObject<T>(thisConfiguration);
        }

        protected abstract void AfterPropertyChanged();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

            AfterPropertyChanged();
        }
    }

    public enum ScrubKind
    {
        Percent,
        Seconds,
        Ticks
    }
}

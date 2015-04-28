using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading;
using Newtonsoft.Json;
using Peruser.Annotations;

namespace Peruser
{
    public class Configuration : INotifyPropertyChanged
    {
        private string[] _allowedFileTypes;
        private bool _alwaysOnTop;
        private bool _mute;

        public string[] AllowedFileTypes
        {
            get { return _allowedFileTypes; }
            set
            {
                if (Equals(value, _allowedFileTypes)) return;
                _allowedFileTypes = value;
                OnPropertyChanged();
            }
        }

        public bool AlwaysOnTop
        {
            get { return _alwaysOnTop; }
            set
            {
                if (value.Equals(_alwaysOnTop)) return;
                _alwaysOnTop = value;
                OnPropertyChanged();
            }
        }

        public bool Mute
        {
            get { return _mute; }
            set
            {
                if (value.Equals(_mute)) return;
                _mute = value;
                OnPropertyChanged();
            }
        }

        private Configuration()
        {
            AllowedFileTypes = new[] {"webm", "jpg", "gif", "png", "jpeg", "bmp", "mp4", "avi", "mkv", "flv"};
            AlwaysOnTop = false;
            Mute = false;
        }

        private static string _configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

        private static Configuration _curConfig;
        public static Configuration Current
        {
            get
            {
                if (_curConfig != null) return _curConfig;

                if (!File.Exists(_configPath))
                {
                    File.WriteAllText(_configPath, Serialize(new Configuration()));
                }

                _curConfig = Deserialize(File.ReadAllText(_configPath));

                return _curConfig;
            }
            set
            {
                _curConfig = value;
            }
        }

        public static string Serialize(Configuration thisConfiguration)
        {
            return JsonConvert.SerializeObject(thisConfiguration);
        }

        public static Configuration Deserialize(string thisConfiguration)
        {
            return JsonConvert.DeserializeObject<Configuration>(thisConfiguration);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

            if (_curConfig != null)
            {
                File.WriteAllText(_configPath, Serialize(_curConfig));
            }
        }
    }
}

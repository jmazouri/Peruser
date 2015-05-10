using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Peruser.Annotations;

namespace Peruser
{
    public class Configuration : INotifyPropertyChanged
    {
        private string[] _allowedFileTypes;
        private bool _alwaysOnTop;
        private bool _mute;
        private ScrubKind _scrubKind;
        private float _scrubAmount;

        [DisplayName("Scrub Amount")]
        public float ScrubAmount
        {
            get { return _scrubAmount; }
            set
            {
                if (Equals(value, _scrubAmount)) return;
                _scrubAmount = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Scrub Type")]
        public ScrubKind ScrubType
        {
            get { return _scrubKind; }
            set
            {
                if (Equals(value, _scrubKind)) return;
                _scrubKind = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Allowed File Types")]
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

        [DisplayName("Always on Top")]
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

        [DisplayName("Mute on Startup")]
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

    public enum ScrubKind
    {
        Percent,
        Seconds,
        Ticks
    }
}

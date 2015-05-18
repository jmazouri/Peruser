using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Peruser.Annotations;

namespace Peruser
{
    public class PeruserConfig : BaseConfiguration
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

        private PeruserConfig()
        {
            AllowedFileTypes = new[] {"webm", "jpg", "gif", "png", "jpeg", "bmp", "mp4", "avi", "mkv", "flv"};
            AlwaysOnTop = false;
            Mute = false;
            ScrubType = ScrubKind.Seconds;
            ScrubAmount = 3;
        }

        private static string _configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

        private static PeruserConfig _curConfig;
        public static PeruserConfig Current
        {
            get
            {
                if (_curConfig != null) return _curConfig;

                if (!File.Exists(_configPath))
                {
                    File.WriteAllText(_configPath, Serialize(new PeruserConfig()));
                }

                _curConfig = Deserialize<PeruserConfig>(File.ReadAllText(_configPath));

                return _curConfig;
            }
            set
            {
                _curConfig = value;
            }
        }

        protected override void AfterPropertyChanged()
        {
            if (_curConfig != null)
            {
                File.WriteAllText(_configPath, Serialize(_curConfig));
            }
        }
    }
}

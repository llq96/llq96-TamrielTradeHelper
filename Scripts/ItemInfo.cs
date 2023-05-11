using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using VladB.Utility;
using System.Drawing;
using System.Diagnostics;

namespace TamrielTradeApp
{
    public class ItemInfo
    {
        public string Name;
        public string Place;
        public string Guild;
        private string _time;

        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                UpdateTimeMinutes();
            }
        }

        public float OnePrice;
        public int Count;
        public float FullPrice;

        public ItemQuality Quality;

        private string _imageHtmlName;

        public string ImageHtmlName
        {
            get { return _imageHtmlName; }
            set
            {
                _imageHtmlName = value;
                Task task = new Task(() => { Image = ItemImages.GetImage(_imageHtmlName); });
                task.Start();
            }
        }

        public Image Image;

        private string _guid;

        public string GUID
        {
            get
            {
                if (_guid.IsNullOrEmpty())
                {
                    _guid = Extensions.GetRandomGuid();
                }

                return _guid;
            }
        }

        public int TimeMinutes;
        public override string ToString() => $"{Name}, {Place}, {Guild}, {Time}, {OnePrice}, {Count}, {FullPrice}";

        private bool _isHidden;

        public bool IsHidden
        {
            get { return _isHidden; }
            set
            {
                _isHidden = value;
                if (_isHidden)
                {
                    OnAnyItemHidden?.Invoke();
                }
                else
                {
                    OnAnyItemUnHidden?.Invoke();
                }
            }
        }

        public static Action OnAnyItemHidden;
        public static Action OnAnyItemUnHidden;

        public bool IsEqual(ItemInfo other)
        {
            return Name == other.Name &&
                   Quality == other.Quality &&
                   Place == other.Place &&
                   Guild == other.Guild &&
                   OnePrice == other.OnePrice &&
                   Count == other.Count &&
                   FullPrice == other.FullPrice;
        }

        private void UpdateTimeMinutes()
        {
            if (Time.Contains("сейчас"))
            {
                TimeMinutes = 0;
            }
            else
            {
                var regexMinutes = new Regex(@"^(\d+) мин. назад$").Match(Time);
                var regexHours = new Regex(@"^(\d+) ч. назад$").Match(Time);
                if (regexMinutes.Success)
                {
                    int val = int.Parse(regexMinutes.Groups[1].Value);
                    TimeMinutes = val;
                }
                else if (regexHours.Success)
                {
                    int val = int.Parse(regexHours.Groups[1].Value);
                    TimeMinutes = 60 * val;
                }
                else
                {
                    Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAA: " + Time);
                }
            }
        }

        public bool IsTrash()
        {
            if (Form1.Instance.checkBox_IsHideOld.Checked)
            {
                if (TimeMinutes >= 120)
                {
                    return true;
                }
            }

            if (Form1.Instance.checkBox_IsHideUserHided.Checked)
            {
                if (IsHidden)
                {
                    return true;
                }
            }

            return false;
        }

        public static Color[] QualityColors = new Color[]
        {
            Color.White,
            Color.White,
            Color.FromArgb(255, 105, 231, 117),
            Color.FromArgb(255, 40, 179, 237),
            Color.FromArgb(255, 232, 108, 228),
            Color.FromArgb(255, 231, 227, 105),
        };
    }
}
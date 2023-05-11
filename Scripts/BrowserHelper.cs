using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace TamrielTradeApp
{
    public class BrowserHelper
    {
        public ChromiumWebBrowser BrowserView { get; private set; }

        public void Init()
        {
            //Создаём настройки, указывая путь до кэша чтобы не вводить логин/пароль каждый раз
            CefSettings settings = new();
            settings.CachePath = Path.Combine(Application.StartupPath, "BrowserCache"); //Путь до кэша
            if (Cef.IsInitialized == false)
                Cef.Initialize(settings);

            //Создаём контрол на форме
            BrowserView = new ChromiumWebBrowser();
            //Делаем его на всю доступную область
            BrowserView.Dock = DockStyle.Fill;
            //Отступаем немного от краёв
            BrowserView.Padding = new Padding(5, 5, 5, 5);
        }
    }
}
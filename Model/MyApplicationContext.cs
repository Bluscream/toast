using System.Configuration;
using SoundSwitch.Framework.Banner;
using SoundSwitch.Framework.Banner.Position;

namespace Notify_Toast.Model {
    public class MyApplicationContext : System.Windows.Forms.ApplicationContext {
        private readonly BannerManager _bannerManager = new();
        private readonly BannerPositionFactory _bannerPositionFactory = new();
        public MyApplicationContext(string[] args) {

            BannerManager.Setup();
            var toastData = new BannerData();
            toastData.Image = Properties.Resources.powershell_ise.ToBitmap();
            toastData.Text = args[0];
            toastData.Title = args.Length > 1 ? args[1] : "New Notification";
            toastData.Position = new PositionTopLeft();
            var appSettings = ConfigurationManager.AppSettings;
            toastData.Ttl = TimeSpan.FromSeconds(10);//TimeSpan.Parse(ConfigurationManager.AppSettings.Get("BannerOnScreenTime"));
            _bannerManager.ShowNotification(toastData);
        }
    }
}
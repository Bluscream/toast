using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Notify_Toast.Model;
using System.Configuration;

namespace Notify_Toast {
    internal static class Program {

        private static WindowsFormsSynchronizationContext _synchronizationContext;

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [HandleProcessCorruptedStateExceptions]
        [STAThread]
        private static void Main(string[] args) {
            Thread.CurrentThread.Name = "Main Thread";
            SetProcessDPIAware();

            Application.EnableVisualStyles();
#if NETCORE
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif
            Application.SetCompatibleTextRenderingDefault(false);
#if !DEBUG
            try
            {
#endif
            _synchronizationContext = new WindowsFormsSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
            Application.Run(new MyApplicationContext(args));
#if !DEBUG
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
#endif
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Restarts the application itself.
        /// </summary>
        public static void RestartApp() {
            var info = new ProcessStartInfo {
                Arguments = $"/C ping 127.0.0.1 -n 3 && \"{Application.ExecutablePath}\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            };
            Process.Start(info);
            Application.Exit();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            HandleException(e.Exception);
        }

        private static void HandleException(Exception exception) {
            if (exception == null)
                return;
        }
    }
}

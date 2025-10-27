using CardGame.Engine;
using System.Windows;

namespace CardGame.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var vm = new MainWindowViewModel();
            var main = new MainWindow
            {
                DataContext = vm
            };

            main.Show();
        }
    }
}

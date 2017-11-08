using System.Windows;
using Autofac;
using Infotecs.MiniJournal.Application.ViewModels;

namespace Infotecs.MiniJournal.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var resolver = new MiniJournalDependencyResolver();
            
            var logger = resolver.Resolve<ILogger>();
            logger.LogEnvironmentInfo();

            var window = new Views.MainWindow
            {
                DataContext = resolver.Resolve<ArticlesViewModel>()
            };
            window.Show();
        }
    }
}

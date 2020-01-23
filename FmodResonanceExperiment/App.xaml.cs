using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FmodResonanceExperiment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ResonanceAudio.Free();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ResonanceAudio.Init();

        }/*mtd*/
    }
}

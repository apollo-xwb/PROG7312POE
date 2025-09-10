using System;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // Enable modern visual styles for WinForms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Launch the main menu form
            Application.Run(new MainMenuForm());
        }
    }
}



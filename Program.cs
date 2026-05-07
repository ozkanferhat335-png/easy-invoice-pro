using System;
using System.Windows.Forms;
using EasyInvoicePro.UI;

namespace EasyInvoicePro
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

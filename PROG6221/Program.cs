using System;
using System.Windows.Forms;
using PROG6221.Services;

namespace PROG6221
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize chatbot service
            var chatbotService = new ChatbotService();

            // Pass service to main form
            Application.Run(new MainForm(chatbotService));
        }
    }
}
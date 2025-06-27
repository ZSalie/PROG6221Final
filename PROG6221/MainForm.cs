using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PROG6221
{
    public partial class MainForm : Form
    {
        private CyberBotBot? _bot;
        private readonly ResponseManager _responseManager = new();
        private readonly SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();

        public MainForm(Services.ChatbotService chatbotService)
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            string name = TxtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please Enter Your Name.");
                return;
            }

            _bot = new CyberBotBot();

            // ASCII logo
            string asciiLogo = @"
  ____                 _                 _            
 |  _ \ ___  ___ _   _| | ___  _ __ ___ (_) ___  ___  
 | |_) / _ \/ __| | | | |/ _ \| '_ ` _ \| |/ _ \/ __| 
 |  _ <  __/ (__| |_| | | (_) | | | | | | |  __/\__ \ 
 |_| \_\___|\___|\__,_|_|\___/|_| |_| |_|_|\___||___/ 
                                                      ";

            AppendBotMessage(asciiLogo);

            // Voice and text greeting
            string greetingMessage = "Welcome to Cybersecurity Awareness Bot!";
            AppendBotMessage(greetingMessage);
            _speechSynthesizer.SpeakAsync(greetingMessage);

            string personalMessage = $"Nice To Meet You, {name}! Let's Learn How To Stay Safe Online.";
            AppendBotMessage(personalMessage);
            _speechSynthesizer.SpeakAsync(personalMessage);

            AppendBotMessage("You Can Ask Me About Phishing, Password Safety, Safe Browsing, Cyber Hygiene, Or Manage Tasks.\n" +
                             "Type 'Start Quiz' To Begin The Cybersecurity Quiz.");

            TxtQuestion.Enabled = true;
            BtnAsk.Enabled = true;
            TxtName.Enabled = false;
            BtnStart.Enabled = false;
        }

        private void BtnAsk_Click(object? sender, EventArgs e)
        {
            string input = TxtQuestion.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(input);
            string response = _bot?.Respond(input, _responseManager) ?? "Bot Not Initialized";
            AppendBotMessage(response);
            TxtQuestion.Clear();
        }

        private void BtnViewTasks_Click(object? sender, EventArgs e)
        {
            if (_bot == null) return;
            string response = _bot.Respond("List Tasks", _responseManager);
            AppendBotMessage(response);
        }

        private void AppendBotMessage(string message)
        {
            RtbConversation.SelectionColor = Color.Blue;
            RtbConversation.AppendText($"Bot: {message}\n");
            RtbConversation.ScrollToCaret();
        }

        private void AppendUserMessage(string message)
        {
            RtbConversation.SelectionColor = Color.DarkGreen;
            RtbConversation.AppendText($"You: {message}\n");
            RtbConversation.ScrollToCaret();
        }
    }
}

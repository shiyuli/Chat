using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chat.Core;

namespace Chat.UI
{
    internal partial class ChatPage : Page
    {
        private readonly string _roomKey;
        private readonly Client _client;

        public ChatPage(string roomKey)
        {
            InitializeComponent();

            _roomKey = roomKey;
            _client = new Client(GetHost(), _roomKey);

            _client.MessageReceived += OnMessageReceived;
        }

        private string GetHost()
        {
            const string filename = "host.sec";
            string[] lines = System.IO.File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                return line.Trim();
            }

            return null;
        }

        private bool CheckMessage(string message)
        {
            return message != null;
        }

        private void SendMessage()
        {
            string message = TextBoxInput.Text.Trim();
            if (CheckMessage(message))
            {
                _client.SendMessage(message);
            }

            TextBoxInput.Text = string.Empty;
        }

        private void OnMessageReceived(string message)
        {
            //ChatList.Dispatcher.Invoke(
            //    new Action(
            //        () => ChatList.Text += message + '\n'));
        }

        private void ButtonSend_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void TextBoxInput_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        public delegate void LogoutHandler();
        public event LogoutHandler LogoutSuccess;

        private void ButtonQuitRoom_OnClick(object sender, RoutedEventArgs e)
        {
            LogoutSuccess?.Invoke();
        }
    }
}

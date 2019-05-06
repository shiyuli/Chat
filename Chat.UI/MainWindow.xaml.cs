using System;
using System.Windows;
using System.Windows.Controls;

namespace Chat.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoginPage loginPage = new LoginPage();
            loginPage.LoginSuccess += OnLoginSuccess;

            LoadPage(loginPage);
        }

        private void OnLoginSuccess(string roomKey)
        {
            ChatPage chatPage = new ChatPage(roomKey);
            chatPage.LogoutSuccess += OnLogoutSuccess;

            LoadPage(chatPage);
        }

        private void OnLogoutSuccess()
        {
        }

        private void LoadPage(Page page)
        {
            this.Content = page;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Environment.Exit(0);
        }
    }
}

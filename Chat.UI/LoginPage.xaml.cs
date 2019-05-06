using System.Windows;
using System.Windows.Controls;

namespace Chat.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    internal partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public delegate void LoginHandler(string roomKey);
        public event LoginHandler LoginSuccess;

        private static bool CheckRoomKey(string roomKey)
        {
            return true;
        }

        private void ButtonEnterRoom_OnClick(object sender, RoutedEventArgs e)
        {
            string roomKey = RoomKey.Text.Trim();

            if (CheckRoomKey(roomKey))
            {
                LoginSuccess?.Invoke(roomKey);
            }
        }
    }
}

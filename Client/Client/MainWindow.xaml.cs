using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Client
{
    public partial class MainWindow : Window
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private string _host = "127.0.0.1";
        private int _port = 12345;

        public MainWindow()
        {
            InitializeComponent();
            userIndexText.Text = "Z01D2K1";

            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_host, _port);
                _networkStream = _tcpClient.GetStream();
            }
            catch(Exception exception)
            {
                MessageBox.Show($"Error: {exception.Message}");
                return;
            }
            
        }

        private void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_tcpClient.Client.Connected)
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(_host, _port);
                    _networkStream = _tcpClient.GetStream();
                }

                const int BUFFER_SIZE = 1024;
                const int OFFSET = 0;

                if (userIndexText.Text == string.Empty)
                {
                    MessageBox.Show("Заполните текстовое поле!");
                    return;
                }

                var bufferIndex = Encoding.UTF8.GetBytes(userIndexText.Text);

                _networkStream.Write(bufferIndex, OFFSET, bufferIndex.Length);

                var bufferStreets = new byte[BUFFER_SIZE];

                int bufferStreetsSize = _networkStream.Read(bufferStreets, OFFSET, bufferStreets.Length);

                string streets = Encoding.UTF8.GetString(bufferStreets, OFFSET, bufferStreetsSize);

                streetsText.Text += streets;

                _tcpClient.Close();
            }
            catch (Exception exception)
            {
                _tcpClient.Close();
                MessageBox.Show($"Error: {exception.Message}");
            }
        }
    }
}

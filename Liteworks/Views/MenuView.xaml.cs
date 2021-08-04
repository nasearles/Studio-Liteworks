using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Liteworks.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public static SerialPort serialPort;
        string[] ports;
        bool isConnected = false;
        public MenuView()
        {
            InitializeComponent();
            InitializeSerialPorts();
        }

        private void SerialRefresh(object sender, RoutedEventArgs e)
        {
            InitializeSerialPorts();
        }

        public static SerialPort GetPort()
        {
            return serialPort;
        }

        private void InitializeSerialPorts()
        {
            ports = SerialPort.GetPortNames();
            if (ports.Count() != 0)
            {
                foreach (string part in ports)
                {
                    var item = Port.Items;
                    if (!item.Contains(part))
                    {
                        Port.Items.Add(part);
                    }
                }
                Port.SelectedItem = ports[0];
            }
            serialPort = null;
        }
        private void ConnectToArduino()
        {
            try
            {
                string selectecSerialPort = Port.SelectedItem.ToString();
                serialPort = new SerialPort(selectecSerialPort, 9600);
                serialPort.Open();
                isConnected = true;
                serialConnectButton.Content = "Disconnect";
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("The selected port is busy", "Busy", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("There is no serial port!", "Empty Serial Port", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void DisconnectFromArduino()
        {
            serialConnectButton.Content = "Connect";
            isConnected = false;
            serialPort.Close();
        }

        private void ConnectToSerial(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
            {
                ConnectToArduino();
            }
            else
            {
                DisconnectFromArduino();
            }
        }

        private void OpenDevices(object sender, RoutedEventArgs e)
        {
            ConnectDevicesMenu connectDevicesMenu = new ConnectDevicesMenu();
            connectDevicesMenu.Show();
        }

    }
}

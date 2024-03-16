using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private string imagePath;

        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
            client.Connect("192.168.1.69", 27001);
        }

        private void DownloadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                DisplayImage(imagePath);
            }
        }

        private void SendImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                SendImage(imagePath);
            }
            else
            {
                MessageBox.Show("Please firstly download an image");
            }
        }

        private void DisplayImage(string imagePath)
        {
            try
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = new System.Windows.Controls.Image()
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath))
                };
                imageListBox.Items.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying image: " + ex.Message);
            }
        }

        private void SendImage(string imagePath)
        {
            try
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                using (NetworkStream stream = client.GetStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(imageData.Length);
                    writer.Write(imageData);
                }
                MessageBox.Show("Image sended successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending image: " + ex.Message);
            }
        }
    }
}

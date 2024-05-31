﻿using System;
using System.Collections.Generic;
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

namespace ClientServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ClientServerViewModel();
        }



        private void ClientConnect(object sender, RoutedEventArgs e)
        {
            buttonClientConnect.IsEnabled = false;
            buttonClientDisconnect.IsEnabled = true;
            buttonClientSendMessage.IsEnabled = true;
        }

        private void ClientDisconnect(object sender, RoutedEventArgs e)
        {
            buttonClientDisconnect.IsEnabled = false;
            buttonClientConnect.IsEnabled = true;
            buttonClientSendMessage.IsEnabled = false;
        }

        private void ServerStart(object sender, RoutedEventArgs e)
        {
            buttonServerStart.IsEnabled = false;
            buttonServerClose.IsEnabled = true;
        }

        private void ServerClose(object sender, RoutedEventArgs e)
        {
            buttonServerStart.IsEnabled = true;
            buttonServerClose.IsEnabled = false; 
        }


    }
}

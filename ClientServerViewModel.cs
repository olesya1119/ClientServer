using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;

namespace ClientServer
{
    public class ClientServerViewModel: INotifyPropertyChanged
    {
        private string clientMessage, serverMessage, ipAddress, path;
        private Client client;
        private Server server;
        private Thread thread;

        public delegate void SetMessage(string message);
        public event SetMessage SetServerMessage;

        public void SetserverMessage(string mes)
        {
            ServerMessage += mes;
        }


        public ClientServerViewModel()
        {
            SetServerMessage += SetserverMessage;
            client = new Client();
            server = new Server(SetServerMessage);
            IpAddress = "127.0.0.1";
            ClientMessage = string.Empty;
            ServerMessage = string.Empty;
            path = $"C:\\";
            
        }

        public string ClientMessage
        {   get { return clientMessage; } 
            set {
                clientMessage = value;
                OnPropertyChanged("ClientMessage");      
            } 
        }

        public string ServerMessage
        {
            get { return serverMessage; }
            set
            {
                serverMessage = value;
                OnPropertyChanged("ServerMessage");                
            }
        }

        public string IpAddress { 
            get { return ipAddress; } set
            {
                ipAddress = value;
                OnPropertyChanged("IpAddress");
            } 
        }

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }



        private RelayCommand clientConnect;
        public RelayCommand ClientConnect
        {
            get
            {
                return clientConnect ??
                  (clientConnect = new RelayCommand(obj =>
                  {
                      client.Connect(IpAddress);
                      ClientMessage += "Соединение с сервером " + IpAddress + " установлено" + DateTime.Now + "\n";
                  }));
            }
        }

        private RelayCommand clientDisconnect;
        public RelayCommand ClientDisconnect
        {
            get
            {
                return clientDisconnect ??
                  (clientDisconnect = new RelayCommand(obj =>
                  {
                      client.Disconnect();
                      ClientMessage += "Клиент отключился " + DateTime.Now + "\n";
                  }));
            }
        }

        private RelayCommand clientSendMessage;
        public RelayCommand ClientSendMessage
        {
            get
            {
                return clientSendMessage ??
                  (clientSendMessage = new RelayCommand(obj =>
                  {
                      ClientMessage += "Клиент получил " + DateTime.Now + "\n" + client.SendMessage(Path);
                  }));
            }
        }
        private RelayCommand serverStart;
        public RelayCommand ServerStart
        {
            get
            {
                return serverStart ??
                  (serverStart = new RelayCommand(obj =>
                  {
                      server.Start();
                      ServerMessage += "Сервер создан " + DateTime.Now.ToString() + "\n";
                      thread = new Thread(server.CreateNewConnection);
                      thread.Start();
                  }));
            }
        }

        private RelayCommand serverStop;
        public RelayCommand ServerStop
        {
            get
            {
                return serverStop ??
                  (serverStop = new RelayCommand(obj =>
                  {
                      server.Close();
                      ServerMessage += "Сервер отключен " + DateTime.Now.ToString() + "\n";
                      thread.Abort();
                  }));
            }
        }


        private RelayCommand сelectCatalog;
        /// <summary>Выбрать каталог</summary>
        public RelayCommand CelectCatalog
        {
            get
            {
                return сelectCatalog ??
                  (сelectCatalog = new RelayCommand(obj =>
                  {
                      FolderBrowserDialog dlg = new FolderBrowserDialog();
                      if (dlg.ShowDialog() == DialogResult.OK)
                      {
                          Path = dlg.SelectedPath;
                      }
                  }));
            }
        }

        private RelayCommand celectFile;
        /// <summary>Выбрать файл</summary>
        public RelayCommand CelectFile
        {
            get
            {
                return celectFile ??
                  (celectFile = new RelayCommand(obj =>
                  {
                      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                      dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                      if (dlg.ShowDialog() == true)
                      {
                          Path = dlg.FileName;
                      }
                  }));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

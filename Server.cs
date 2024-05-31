using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ClientServer.ClientServerViewModel;
using System.Windows.Forms;
using System.Windows.Markup;
using System.IO;

namespace ClientServer
{
    public class Server
    {
        Socket socket;
        SetMessage SetServerMessage;

        public Server(SetMessage setServerMessage)
        {
            SetServerMessage = setServerMessage;
        }

        /// <summary> Запуск сервера </summary>
        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);        
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            

            socket.Bind(endPoint); //Говорим сокету слушать этот порт
            socket.Listen(5); //Слушаем

        }

        /// <summary>Создание сокета для прослушивания, прослушивание и отправка сообщений</summary>
        public void CreateNewConnection()
        {
            Socket client = socket;
            while (true)
            {
                client = socket.Accept();
                byte[] buffer = new byte[256]; //Буфер размером 0,25 Кб
                int size; //Размер процитанного сообщения
                StringBuilder data = new StringBuilder();
                SetServerMessage.Invoke("Клиент подключен с адреса " + client.RemoteEndPoint.ToString() + " " + DateTime.Now.ToString() + "\n");

                while (client.Connected)
                {
                    try {
                        do
                        {
                            size = client.Receive(buffer);
                            data.Append(Encoding.UTF8.GetString(buffer, 0, size)); //Раскодируем
                        }
                        while (client.Available > 0); //Пока есть что читать

                        if (data.Length > 0)
                        {
                            SetServerMessage.Invoke("Сервер получил " + DateTime.Now.ToString() + " " + data.ToString() + "\n");
                        }
                    }
                    catch  { break; }


                    //Возращаем сообщение с результатом
                    client.Send(Encoding.UTF8.GetBytes(CreateMessage(data.ToString())));
                    data = new StringBuilder();
                }

                SetServerMessage.Invoke("Клиент отключился " + DateTime.Now.ToString() + "\n");
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }


        private string CreateMessage(string path)
        {
            string message;
            if (path.Length >= 5) { 
            if (path.Substring(path.Length - 4, 4) == ".txt")
            {
                    try
                    {
                        StreamReader streamReader = new StreamReader(path);
                        string s = streamReader.ReadToEnd();
                        streamReader.Close();
                        return s;
                    }
                    catch { return "Файл не найден\n"; }
                }
            }

            try
            {
                string[] s = Directory.GetFileSystemEntries(path);
                string k = "";
                foreach (string s2 in s) {
                    k += s2.Substring(path.Length, s2.Length - path.Length) + "\n";
                }
                return k;
            }
            catch { return "Каталог не найден\n"; }

        }

        public void Close()
        {
            socket.Close();
        }
    }
}


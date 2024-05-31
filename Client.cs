using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public class Client
    {
        Socket socket;
        IPEndPoint endPoint; 

        /// <summary>Присоединиться по ip</summary>
        public void Connect(string ip)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endPoint = new IPEndPoint(IPAddress.Parse(ip), 8080);
            socket.Connect(endPoint);
        }

        /// <summary>Отправить сообщение</summary>
        public string SendMessage(string message)
        {
            //Отправляем сообщение
            socket.Send(Encoding.UTF8.GetBytes(message));

            //Получаем ответ
            byte[] buffer = new byte[256]; //Буфер размером 0,25 Кб
            int size; //Размер процитанного сообщения
            StringBuilder data = new StringBuilder();

            do
            {
                size = socket.Receive(buffer);
                data.Append(Encoding.UTF8.GetString(buffer, 0, size)); //Раскодируем
            }
            while (socket.Available > 0); //Пока есть что читать

            return data.ToString();
        }

        /// <summary>Отключится</summary>
        public void Disconnect()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HomeServer
{
/*
    class Server
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public Server(int Port)
        {
            // Создаем "слушателя" для указанного порта
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start(); // Запускаем его

            // В бесконечном цикле
            while (true)
            {
                // Принимаем нового клиента
                var client = Listener.AcceptTcpClient();
                // Создаем поток
                var thread = new Thread(ClientThread);
                // И запускаем этот поток, передавая ему принятого клиента
                thread.Start(client);
            }
        }

/*
        static void ClientThread(object stateInfo)
        {
            new ClientWorker((TcpClient)stateInfo);
        }
#1#

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            // Остановим его
            Listener?.Stop();
        }
    }
*/
}

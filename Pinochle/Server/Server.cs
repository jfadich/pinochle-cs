using System;
using System.Collections.Generic;
using Fleck;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Pinochle.Server
{
    class Server
    {
        List<GameTable> tables = new List<GameTable>();

        public Server()
        {
            
        }
        public void Start()
        {
            FleckLog.Level = LogLevel.Debug;

            tables.Add(new GameTable(1));

            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://0.0.0.0:8181");
            
            server.Start(socket =>
            {
                String gameId = socket.ConnectionInfo.Path.Split("/", 2, StringSplitOptions.RemoveEmptyEntries)[0];

                GameTable table = tables.Find(table => table.ID.ToString().Equals(gameId));

                if(table == null)
                {
                    socket.OnOpen = () =>
                    {
                        socket.Send("Game not found");
                        socket.Close();
                    };
                } else
                {
                    table.AddSocket(socket);
                }
            });


            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(input);
                }
                input = Console.ReadLine();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pinochle.Server
{
    class GameTable
    {
        List<Connection> connections = new List<Connection>();

        public int ID { get; }

        protected Game Pinochle;

        protected string[] PlayerTokens = new string[4];
        public GameTable(int id)
        {
            ID = id;
            Pinochle = new Game();

            Pinochle.PhaseCompleted += BroadcastPhaseCompleted;
            Pinochle.TurnTaken += BroadcastTurnCompleted;
        }

        public void BroadcastPhaseCompleted(Pinochle.Events.Phases.PhaseCompleted phase)
        {
            Broadcast(new Messages.GameEvent(phase).ToString());
        }
        public void BroadcastTurnCompleted(Pinochle.Events.Turns.PlayerTurn turn)
        {
            var message = new Messages.GameEvent(turn);
            message.UserToken = PlayerTokens[turn.Player.Position];

            Broadcast(message.ToString());

        }

        public void AddSocket(IWebSocketConnection socket)
        {
            Connection connection = new Connection(socket);

            socket.OnOpen = () =>
            {
                Console.WriteLine("Open!");
                connections.Add(connection);
            };

            socket.OnClose = () =>
            {
                Console.WriteLine("Close!");
                connections.Remove(connection);
            };

            socket.OnMessage = (message) => {
                handleMessage(connection, message);
            };
        }

        protected void Join(Connection connection, Messages.PlayerJoined joined)
        {
            if (joined.Position >= 4 || joined.Position < 0)
            {
                connection.Reply("Invalid Position");
                return;
            }

            if(PlayerTokens.Any(token => token == connection.UserToken))
            {
                connection.Reply("You're already seated");
                return;
            }

            if (PlayerTokens[joined.Position] == default)
            {
                PlayerTokens[joined.Position] = connection.UserToken;

                Broadcast(joined.ToString());
            }
            else
            {
                connection.Reply("Seat Taken");
            }
        }

        protected void StartGame(Connection dealer)
        {
            Pinochle.StartGame();
        }

        public void Broadcast(string message, Connection except)
        {
            connections.ToList().ForEach(c => {
                if (except == null || c.Socket != except.Socket)
                {
                    c.Socket.Send(message);
                }
            });
        }

        public void Broadcast(string message)
        {
            Broadcast(message, null);
        }

        protected void handleMessage(Connection sender ,string json)
        {
            JObject request;

            try
            {
                request = JObject.Parse(json);
            } catch(JsonReaderException e)
            {
                sender.Reply("BAD JSON");
                return;
            }

            string type = request.GetValue("type").ToString();
            var data = request.GetValue("data");

            if(data == null && type != "Start")
            {
                sender.Reply("Missing Data");
            }

            if (type == "Chat")
            {
                var message = data.ToObject<Messages.Chat>();
                message.UserToken = sender.UserToken;

                if(message != null)
                {
                    Broadcast(message.ToString());
                }

            }

            if (type == "Join")
            {
                var joined = data.ToObject<Messages.PlayerJoined>();
                joined.UserToken = sender.UserToken;

                int position = joined.Position;

                Join(sender, joined);

            }

            if(type == "Start")
            {
                if(PlayerTokens.Any(token => token == default))
                {
                    sender.Reply("Not enough Players");
                }

                StartGame(sender);
            }
        }
    }
}

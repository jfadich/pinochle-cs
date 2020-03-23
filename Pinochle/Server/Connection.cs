using System;
using System.Collections.Generic;
using System.Text;
using Fleck;
using Newtonsoft;

namespace Pinochle.Server
{
    class Connection
    {
        public IWebSocketConnection Socket;

        public String UserToken { get; protected set; }

        public Connection(IWebSocketConnection socket)
        {
            Socket = socket;

            var uri = new Uri(socket.ConnectionInfo.Host + socket.ConnectionInfo.Path);
            var parameters = System.Web.HttpUtility.ParseQueryString(uri.Query);

            UserToken = parameters["token"];
        }

        public void Reply(string message)
        {
            Socket.Send((new Messages.BadRequest { Message = message }).ToString()); // @todo type: "response", data: {messasge: ""}
        }
    }
}

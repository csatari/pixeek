using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.BoardShapes;
using Pixeek.Game;
using Pixeek.ServerCommunicator.Objects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Pixeek.ServerCommunicator
{
    /// <summary>
    /// Singleton object for communicating with the server about multi player games
    /// </summary>
    public class FightGameCommunicator : ServerCommunicator
    {
        private static FightGameCommunicator instance;

        //singleton Instance
        public static FightGameCommunicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FightGameCommunicator();
                }
                return instance;
            }
        }

        public delegate void NewFightGameHandler(NewFightGameResponse response);
        public delegate void NotificationHandler(FightNotifyResponse response);

        public void NewFightGameListener(NewFightGameHandler handler)
        {
            SocketListener(delegate(string response)
            {
                if (handler != null)
                {
                    handler(fastJSON.JSON.ToObject<NewFightGameResponse>(response));
                }
            });
        }

        public void SendNotification(int row, int column, int score)
        {
            FightNotifyRequest request = new FightNotifyRequest()
            {
                col_index = column,
                row_index = row,
                score = score
            };
            fastJSON.JSON.Parameters.UseExtensions = false;

            SendSocket(fastJSON.JSON.ToJSON(request));
        }

        public void NotificationListener(NotificationHandler handler)
        {
            SocketListener(delegate(string response)
            {
                if (handler != null)
                {
                    handler(fastJSON.JSON.ToObject<FightNotifyResponse>(response));
                }
            });
        }
    }
}
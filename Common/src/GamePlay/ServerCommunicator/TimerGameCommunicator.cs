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
    public class TimerGameCommunicator : ServerCommunicator
    {
        private static TimerGameCommunicator instance;

        //singleton Instance
        public static TimerGameCommunicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimerGameCommunicator();
                }
                return instance;
            }
        }

        public delegate void NewTimerGameHandler(NewTimeGameResponse response);
        public delegate void NotificationHandler(TimeNotifyResponse response);


        public void NewTimeGameListener(NewTimerGameHandler handler)
        {
            SocketListener(delegate(string response)
            {
                if (handler != null)
                {
                    handler(fastJSON.JSON.ToObject<NewTimeGameResponse>(response));
                }
            });
        }

        public void SendNotification(int row, int column)
        {
            TimeNotifyRequest request = new TimeNotifyRequest()
            {
                col_index = column,
                row_index = row,
                purpose = "solved_task"
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
                    handler(fastJSON.JSON.ToObject<TimeNotifyResponse>(response));
                }
            });
        }
    }
}
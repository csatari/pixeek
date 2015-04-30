using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pixeek.ServerCommunicator
{
    public abstract class ServerCommunicator
    {
        private const string URL = "http://nipglab09.inf.elte.hu:8000/pixeek";

        public delegate void CommandResult(string result);

        /// <summary>
        /// Command is sent (parameters are in path), and the result will be in commandResult
        /// </summary>
        /// <param name="path"></param>
        /// <param name="commandResult"></param>
        protected void sendGetCommand(string path, CommandResult commandResult)
        {
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(URL + path).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }  
            }).Start();
            
        }

        /// <summary>
        /// Command is sent (parameters are in path), the body is the parameter Object, what will be serialized via json
        /// Result will be in commandResult.
        /// PUT method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="commandResult"></param>
        protected void sendPutCommand(string path, Object parameter, CommandResult commandResult)
        {
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(URL + path,content).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }
            }).Start();

        }

        /// <summary>
        /// Command is sent (parameters are in path), the body is the parameter Object, what will be serialized via json
        /// Result will be in commandResult
        /// POST method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="commandResult"></param>
        protected void sendPostCommand(string path, Object parameter, CommandResult commandResult)
        {
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(URL + path, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }
            }).Start();

        }
    }
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException() {}
        public InvalidParameterException(string message) {}
        public InvalidParameterException(string message, System.Exception inner) {}

        protected InvalidParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }
}
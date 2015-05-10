using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
#if WINDOWS
using System.Net.Http;
using System.Net.Http.Headers;
#endif
#if ANDROID
using Java.Net;
using Java.IO;
using Java.Lang;
#endif
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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
#if WINDOWS
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
            
#endif
#if ANDROID
            new System.Threading.Thread(() =>
            {
                Java.Net.URL obj = new Java.Net.URL(URL + path);
                HttpURLConnection con = (HttpURLConnection)obj.OpenConnection();
                con.RequestMethod = "GET";

                HttpStatus responseCode = con.ResponseCode;

                BufferedReader br = new BufferedReader(new InputStreamReader(con.InputStream));
                System.String inputLine;
		        System.String response = "";
 
                inputLine = br.ReadLine();
		        while (inputLine != null) {
                    response += inputLine;
                    inputLine = br.ReadLine();
		        }
                br.Close();

                commandResult(response);
            }).Start();
            
#endif
        }

        /// <summary>
        /// Command is sent (parameters are in path), the body is the parameter Object, what will be serialized via json
        /// Result will be in commandResult.
        /// PUT method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="commandResult"></param>
        protected void sendPutCommand(string path, System.Object parameter, CommandResult commandResult)
        {
#if WINDOWS
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(fastJSON.JSON.ToJSON(parameter), Encoding.UTF8, "application/json");
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
#endif
#if ANDROID
            new System.Threading.Thread(() =>
            {
                Java.Net.URL obj = new Java.Net.URL(URL + path);
                HttpURLConnection con = (HttpURLConnection)obj.OpenConnection();
                con.RequestMethod = "PUT";

                OutputStreamWriter outstream = new OutputStreamWriter(con.OutputStream);

                outstream.Write(fastJSON.JSON.ToJSON(parameter));
                outstream.Close();

                HttpStatus responseCode = con.ResponseCode;

                BufferedReader br = new BufferedReader(new InputStreamReader(con.InputStream));
                /*System.String inputLine;
		        System.String response = "";
 
                inputLine = br.ReadLine();
		        while (inputLine != null) {
                    response += inputLine;
                    inputLine = br.ReadLine();
		        }*/
                br.Close();

                commandResult(null);
                

                /*HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(URL + path).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }*/
            }).Start();
            
#endif

        }

        /// <summary>
        /// Command is sent (parameters are in path), the body is the parameter Object, what will be serialized via json
        /// Result will be in commandResult
        /// POST method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="commandResult"></param>
        protected void sendPostCommand(string path, System.Object parameter, CommandResult commandResult)
        {
#if WINDOWS
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(fastJSON.JSON.ToJSON(parameter), Encoding.UTF8, "application/json");
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
#endif
#if ANDROID
            new System.Threading.Thread(() =>
            {
                Java.Net.URL obj = new Java.Net.URL(URL + path);
                HttpURLConnection con = (HttpURLConnection)obj.OpenConnection();
                con.RequestMethod = "POST";

                HttpStatus responseCode = con.ResponseCode;

                OutputStreamWriter outstream = new OutputStreamWriter(con.OutputStream);

                outstream.Write(fastJSON.JSON.ToJSON(parameter));
                outstream.Close();

                BufferedReader br = new BufferedReader(new InputStreamReader(con.InputStream));
                System.String inputLine;
		        System.String response = "";
 
                inputLine = br.ReadLine();
		        while (inputLine != null) {
                    response += inputLine;
                    inputLine = br.ReadLine();
		        }
                br.Close();

                commandResult(response);
                

                /*HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(URL + path).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }*/
            }).Start();
            
#endif
        }
    }
    public class InvalidParameterException : System.Exception
    {
        public InvalidParameterException() {}
        public InvalidParameterException(string message) {}
        public InvalidParameterException(string message, System.Exception inner) {}

        /*protected InvalidParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}*/
    }
}
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Galaxy.HabboHotel.Rooms.Camera
{
    public class HabboCameraPictureRequest
    {
        public event ImageUrlReceived OnImageUrlReceive;
        
        public byte[] Data;
        public GameClient Session;

        private WebRequest WebRequest;

        private Thread thread;

        public int Index;

        public HabboCameraPictureRequest(int index, GameClient session, byte[] data, ImageUrlReceived callback)
        {
            Session = session;
            Data = data;
            Index = index;
            OnImageUrlReceive += callback;

            
            thread = new Thread(new ThreadStart(StartRequesting));
            thread.Name = "HABBOCAMERA_NUMBER_" + index;
            thread.Start();
            
        }

        private void StartRequesting()
        {
            
            WebRequest = HttpWebRequest.Create(HabboCameraManager.CAMERA_API_HTTP + "");
            // Set the Method property of the request to POST.
            WebRequest.Method = "PUT";

            WebRequest.Timeout = 36000000;

            // Create POST data and convert it to a byte array.

            //string postData = "test";
            //byte[] Data = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            WebRequest.ContentType = "multipart/form-data";

            // Set the ContentLength property of the WebRequest.
            WebRequest.ContentLength = Data.Length;

            // Get the request stream.
            WebRequest.BeginGetRequestStream(OnGetRequest, WebRequest);
            // Write the data to the request stream.
            

        }

        private void OnGetRequest(IAsyncResult e)
        {
            try
            {
                var request = (WebRequest)e.AsyncState;
                var dataStream = request.EndGetRequestStream(e);

                dataStream.Write(Data, 0, Data.Length);
                // Close the Stream object.
                dataStream.Close();

                WebRequest.BeginGetResponse(OnReceiveResponce, WebRequest);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }


        private void OnReceiveResponce(IAsyncResult e)
        {
            try
            {
                var request = (WebRequest)e.AsyncState;

                var response = request.EndGetResponse(e);


                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                // Display the content.
                //Console.WriteLine(responseFromServer);

                this.OnImageUrlReceive(responseFromServer, this);

                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                thread.Abort();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        private void Error(Exception e)
        {
            Session.SendMessage(new RoomNotificationComposer("", "Opss! Houve um erro ao obter a foto! desculpe o transtorno.", "error", "", "", true));
        }
    }
}

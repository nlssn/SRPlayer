using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SRPlayer
{
    /*
     * This file actually contains three classes. 
     * All of them are involved in getting data from the Sveriges Radio API.
     *  1. Api - the class that actually makes the call to the API
     *  2. Response - a class that exposes the result as an traversable array
     *  3. Channel - a class that exposes all the properties of a individual channel
     *
     * The code is dependant on the Newtonsoft.Json package.
     */

    class Api
    {
        // Properties
        private string Url; // The URL to the API
        private HttpClient Client;
        public List<Channel> Channels;

        // Constructor
        public Api(string url)
        {
            // Properties
            Url = url;
            Channels = new List<Channel>(); // The list that will hold all the channels
            Client = new HttpClient(); // The HTTP client used for making the request
        }

        public void Run()
        {
            string data = GetData();
            Convert(data);
        }

        private string GetData()
        {
            // Send a request to the API, and wait for the response.
            var response = Client.GetAsync(Url);
            response.Wait();

            // When the response is completed..
            if (response.IsCompleted)
            {
                // Grab the result, if it's successful
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Read the message, asyncronously
                    var message = result.Content.ReadAsStringAsync();
                    message.Wait();

                    // The raw json data
                    return message.Result;
                }
            }

            // If all else fails, return null
            return null;
        }

        // Converts the raw JSON data into JObjects
        private void Convert(string JsonData)
        {
            try
            {
                // Convert the JSON into a response object (that holds a JArray of all the channels)
                var allChannels = JsonConvert.DeserializeObject<Response>(JsonData);

                // Loop trough the JArray and..
                foreach (JObject channel in allChannels.Channels)
                {
                    // Create a new channel object, 
                    Channel newObj = new Channel();

                    // and assign the correct values
                    newObj.Id = (int)channel["id"];
                    newObj.Name = (string)channel["name"];
                    newObj.Tagline = (string)channel["tagline"];
                    newObj.Url = (string)channel["liveaudio"]["url"];

                    // Then add the new object to the list
                    Channels.Add(newObj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fel: " + e.Message);
            }
        }
    }

    // The response class
    public class Response
    {
        public JArray Channels { get; set; }
    }

    // The channel class
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Url { get; set; }
    }


}

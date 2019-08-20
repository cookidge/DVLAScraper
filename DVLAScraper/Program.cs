using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using JSON = Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DVLAScraper
{
    class Program
    {
        private static string requestUrl = "https://vehicleenquiry.service.gov.uk/ConfirmVehicle?Vrm=";
        private static string vehicleReg = "PJ05KYA";
       
        static void Main(string[] args)
        {
            /***************************
               ConfirmVehicle Request
            ***************************/

            // Variables to store returned informatio
            string vehicleMake, vehicleColour;
            string rxPattern = "value=\"\\w*\"";
   
            Console.Write(requestUrl + vehicleReg);
            Console.WriteLine();

            // Create request
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUrl + vehicleReg);
            webRequest.Method = "POST";

            // Write to request stream
            Stream reqStream = webRequest.GetRequestStream();

            // Retrieve response
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Stream respStream = response.GetResponseStream();

            // Check response
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Request: " + response.StatusDescription);
                Console.WriteLine();
                
                // Handle response
                using (StreamReader reader = new StreamReader(respStream))
                {
                    string[] elements = reader.ReadToEnd().Split(new char[] { '<' });

                    foreach (string element in elements)
                    {
                        // Vehicle Make
                        if (element.Contains("input id=\"Make\""))
                        {
                            // Retrieve vehicleMake input tag
                            vehicleMake = element.Replace("/>", "").Trim();

                            // Determine regex match on tag value
                            var makeRxMatch = Regex.Match(vehicleMake, rxPattern);

                            // Store make
                            if (makeRxMatch.Success)
                            {
                                vehicleMake = makeRxMatch.Value.Replace("value=\"","").Replace("\"", "");
                            }

                            Console.WriteLine("Vehicle Make:" + vehicleMake);

                        }

                        // Vehicle Colour
                        if (element.Contains("input id=\"Colour\""))
                        {
                            // Retrieve vehicleMake input tag
                            vehicleColour = element.Replace("/>", "").Trim();

                            // Determine regex match on tag value
                            var colourRxMatch = Regex.Match(vehicleColour, rxPattern);

                            // Store colour
                            if (colourRxMatch.Success)
                            {
                                vehicleColour = colourRxMatch.Value.Replace("value=\"", "").Replace("\"", "");
                            }

                            Console.WriteLine("Vehicle Colour:" + vehicleColour);
                        }
                    }
                }

            }
            else
            {
                Console.WriteLine("Request: " + response.StatusDescription);
                Console.WriteLine();
            }


           

        }
    }
}

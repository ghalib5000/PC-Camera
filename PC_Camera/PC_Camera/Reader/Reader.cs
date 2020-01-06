using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reader
{
    public class Reader
    {
        string pathnew = "";
        public Reader(string pathnew)
        {
            this.pathnew = pathnew;
        }
        public bool find(string value)
        {
            StreamReader sr = new StreamReader(pathnew);
            JsonTextReader reader = new JsonTextReader(new StringReader(sr.ReadToEnd()));
            while (reader.Read())
            {
                if (reader.TokenType.ToString().Contains("Property") && reader.Value.ToString().Contains(value))
                {
                    Console.WriteLine(reader.Value+" is the token");
                    reader.Read();
                    Console.WriteLine(reader.Value+" is the value");
                    sr.Close();
                    return true;
                }
                /*
                if (reader.Value != null)
                {
                    Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                }
                else
                {
                    Console.WriteLine("Value: {0}", reader.TokenType);
                }*/
            }
            return false;
        }
        public bool ReplaceValueOf(string name, string newvalue)
        {
            string jsonString = File.ReadAllText(pathnew);
            JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString) as JObject;
            // Select a nested property using a single string:
            JToken jToken = jObject.SelectToken("settings." + name);
            jToken.Replace(newvalue);
            string updatedJsonString = jObject.ToString();
            Console.WriteLine("replacing value of " + name+" to "+newvalue);
            File.WriteAllText(pathnew, updatedJsonString);
            return true;
        }
        public string ReturnValueOf(string name)
        {
            string jsonString = File.ReadAllText(pathnew);
            JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString) as JObject;
            // Select a nested property using a single string:
            JToken jToken = jObject.SelectToken("settings." + name);
            Console.WriteLine("returning value of "+name);
           return jToken.ToString();
        }
        public string view()
        {
            StreamReader sr = new StreamReader(pathnew);
            string res = sr.ReadToEnd();
            //Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            return res;
        }
    }
}

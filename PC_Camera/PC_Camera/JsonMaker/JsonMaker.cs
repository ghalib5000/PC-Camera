using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace JsonMaker
{
      public class JSONMaker
    {

        public  JSONMaker(int numberOfValues, string[] names, string[] values,string path)
        {
            Writer.JsonWriter jwr;
            jwr = new Writer.JsonWriter(numberOfValues, names, values);
            //jwr.AddNames(names);
            //jwr.AddValues(values);
            jwr.start();
            StreamWriter write = new StreamWriter(path);
            string arr = JsonConvert.SerializeObject(jwr);
            object obj = JsonConvert.DeserializeObject(arr);
            write.Write(obj);
            write.Close();
        }
    }
}

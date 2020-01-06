using System;
using System.Collections.Generic;
using System.Text;

namespace Writer
{
    class JsonWriter
    {
        System.ArgumentException argEx = new System.ArgumentException("insufficent values, Values do not match the array size");
        private int size;
        private string[] names;
        private string[] values;
        public Dictionary<string, string> settings = new Dictionary<string, string>();
        public JsonWriter(int size)
        {
            this.size = size;
            names = new string[size];
            values = new string[size];
        }
        public JsonWriter(int size, string[] names, string[] values)
        {
            this.size = size;
            this.names = new string[size];
            this.values = new string[size];
            AddNames(names);
            AddValues(values);
        }
        public void AddNames(string[] names)
        {
            if (names.Length == size)
            {
                for (int i = 0; i < size; i++)
                {
                    this.names[i] = names[i];
                }
            }
            else
            {
                throw argEx;
            }
        }
        public void AddValues(string[] values)
        {
            if (values.Length == size)
            {
                for (int i = 0; i < size; i++)
            {
                this.values[i] = values[i];
            }
            }
            else
            {
                 throw argEx;
            }
        }
        public void start()
        {
            for(int i =0;i<size;i++)
            {
                settings.Add(names[i],values[i]);
            }
        }

    }
}

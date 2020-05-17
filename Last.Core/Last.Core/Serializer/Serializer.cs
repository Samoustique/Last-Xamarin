using Last.Core.Models;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Last.Core.Helpers
{
    public sealed class Serializer
    {
        private static Serializer _instance = null;

        private Serializer()
        {
        }

        public static Serializer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Serializer();
                }
                return _instance;
            }
        }

        // TODO change Items by DataStore<Item>
        public void Deserialize(string path, out Data items)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));
            serializer.UnknownNode += new XmlNodeEventHandler(SerializerUnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(SerializerUnknownAttribute);
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlReader reader = new XmlTextReader(fs);
                if (serializer.CanDeserialize(reader))
                {
                    items = (Data)serializer.Deserialize(reader);
                }
                else
                {
                    items = new Data();
                    // TODO notif data format changed, data lossed. => needs to be fixed!
                }
            }
        }

        public void Serialize(string path, Data items)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));

            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, items);
                writer.Close();
            }
        }

        private void SerializerUnknownNode(object sender, XmlNodeEventArgs e)
        {
            // TODO notif?
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void SerializerUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            // TODO notif?
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
    }
}

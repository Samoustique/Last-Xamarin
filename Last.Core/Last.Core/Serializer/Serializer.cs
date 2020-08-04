using Last.Core.Models;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Last.Core.Serializer
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public void Deserialize(string path, out Data data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));
            serializer.UnknownNode += new XmlNodeEventHandler(SerializerUnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(SerializerUnknownAttribute);
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlReader reader = new XmlTextReader(fs);
                data = (Data) serializer.Deserialize(reader);
            }
        }

        public bool Serialize(string path, Data data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));

            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, data);
                writer.Close();
            }

            return true;
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

        public bool CanDeserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));
            serializer.UnknownNode += new XmlNodeEventHandler(SerializerUnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(SerializerUnknownAttribute);
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlReader reader = new XmlTextReader(fs);
                return serializer.CanDeserialize(reader);
            }
        }
    }
}

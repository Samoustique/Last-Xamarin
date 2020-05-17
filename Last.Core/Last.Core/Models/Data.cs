using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Last.Core.Models
{
    [XmlRoot(IsNullable = false)]
    public class Data
    {
        public List<Item> Items;

        public Data()
        {
            Items = new List<Item>();
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public void Update(Item item)
        {
            var oldItem = Items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            Items.Remove(oldItem);
            Items.Add(item);
        }

        public void Delete(string id)
        {
            var oldItem = Items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            Items.Remove(oldItem);
        }

        public Item GetItem(string id)
        {
            return Items.FirstOrDefault(s => s.Id == id);
        }
    }
}

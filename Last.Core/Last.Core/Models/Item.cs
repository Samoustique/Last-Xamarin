using System;

namespace Last.Core.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public DateTime LastModificationDate { get; set; }

        public Item()
        {
            LastModificationDate = DateTime.Now;
        }
    }
}
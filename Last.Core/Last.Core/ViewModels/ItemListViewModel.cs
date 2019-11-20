using System;
using Last.Core.Models;

namespace Last.Core.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        public Item Item { get; private set; }

        public string Text => Item.Text;
        public string Description => Item.Description;
        public int Count => Item.Count;

        public event Action CountChanged;

        public ItemListViewModel(Item item)
        {
            Item = item;
        }

        internal void IncrementAsync()
        {
            Item.Count++;
            Item.LastModificationDate = DateTime.Now;
            CountChanged?.Invoke();
        }
    }
}

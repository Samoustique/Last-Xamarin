using Last.Core.Helpers;
using Last.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Last.Core.Services
{
    public class ItemsDataStore : IDataStore<Item>
    {
        private Data _data;

        public ItemsDataStore()
        {
            _data = new Data();
        }

        public string BbdFilename => "bbd.xml";

        public string BbdPath { get; set; }

        public void Serialize()
        {
            Serializer.Instance.Serialize(BbdPath, _data);
        }

        public void Deserialize()
        {
            Serializer.Instance.Deserialize(BbdPath, out _data);
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            return await Task.Run(() =>
            {
                _data.Add(item);
                Serializer.Instance.Serialize(BbdPath, _data);
                return true;
            });
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            return await Task.Run(() =>
            {
                _data.Update(item);
                Serializer.Instance.Serialize(BbdPath, _data);
                return true;
            });
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return await Task.Run(() =>
            {
                _data.Delete(id);
                Serializer.Instance.Serialize(BbdPath, _data);
                return true;
            });
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.Run(() =>
            {
                return _data.GetItem(id);
            });
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.Run(() =>
            {
                return _data.Items;
            });
        }
    }
}

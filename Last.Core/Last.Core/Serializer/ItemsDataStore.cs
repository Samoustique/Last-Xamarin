using Last.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Last.Core.Services
{
    public class ItemsDataStore : IDataStore<Item>
    {
        private Version _currentVersion;
        private Data _data;
        private Serializer.Serializer _serializer;

        public ItemsDataStore(Version currentVersion)
        {
            _currentVersion = currentVersion;
            _serializer = new Serializer.Serializer();
        }

        public string BbdFilename => "bbd.xml";

        public string BbdPath { get; set; }

        public bool Serialize()
        {
            return _serializer.Serialize(BbdPath, _data);
        }

        public bool Deserialize()
        {
            if (_serializer.CanDeserialize(BbdPath))
            {
                _serializer.Deserialize(BbdPath, out _data);
                TryToUpgrade();
                return true;
            }
            else
            {
                throw new System.NotSupportedException($"Version is not supported");
            }
        }

        private void TryToUpgrade()
        {
            Version dataVersion = _data.Version ?? new Version();

            // TODO
            //while (dataVersion.CompareTo(_currentVersion) < 0)
            //{
            //    //upgrade
            //    // dataVersion = xxx
            //}

            _data.Version = _currentVersion;
        }

        // TODO
        //private bool NeedUpgrade(int version)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> AddItemAsync(Item item)
        {
            return await Task.Run(() =>
            {
                _data.Add(item);
                _serializer.Serialize(BbdPath, _data);
                return true;
            });
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            return await Task.Run(() =>
            {
                _data.Update(item);
                _serializer.Serialize(BbdPath, _data);
                return true;
            });
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return await Task.Run(() =>
            {
                _data.Delete(id);
                _serializer.Serialize(BbdPath, _data);
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

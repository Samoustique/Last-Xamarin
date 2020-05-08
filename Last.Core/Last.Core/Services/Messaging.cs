using System;
using Xamarin.Forms;

namespace Last.Core.Services
{
    public class Messaging
    {
        private const string _msgSave = "Save";
        private const string _msgUpdate = "Update";
        private const string _msgDelete = "Delete";

        public void SubscribeSave<TSender, TArgs>(object subscriber, Action<TSender, TArgs> callback) where TSender : class
        {
            MessagingCenter.Subscribe(subscriber, _msgSave, callback);
        }

        public void SendSave<TSender, TArgs>(TSender sender, TArgs args) where TSender : class
        {
            MessagingCenter.Send(sender, _msgSave, args);
        }

        public void SubscribeUpdate<TSender, TArgs>(object subscriber, Action<TSender, TArgs> callback) where TSender : class
        {
            MessagingCenter.Subscribe(subscriber, _msgUpdate, callback);
        }

        public void SendUpdate<TSender, TArgs>(TSender sender, TArgs args) where TSender : class
        {
            MessagingCenter.Send(sender, _msgUpdate, args);
        }

        public void SubscribeDelete<TSender, TArgs>(object subscriber, Action<TSender, TArgs> callback) where TSender : class
        {
            MessagingCenter.Subscribe(subscriber, _msgDelete, callback);
        }

        public void SendDelete<TSender, TArgs>(TSender sender, TArgs args) where TSender : class
        {
            MessagingCenter.Send(sender, _msgDelete, args);
        }
    }
}

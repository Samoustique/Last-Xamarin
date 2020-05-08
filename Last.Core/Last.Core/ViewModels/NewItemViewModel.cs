using Last.Core.Models;
using Last.Core.Services;
using System;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class NewItemViewModel : ItemDetailViewModel
    {
        public NewItemViewModel(Messaging messaging) : base(messaging)
        {
            MainCommand = new Command(SaveExecute);
            ButtonTitle = "Save";
        }

        private async void SaveExecute()
        {
            await Navigation.PopAsync();
            Messaging.SendSave(this, new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Text = this.Text,
                Count = this.Count,
                Description = this.Description,
                ImagePath = ImagePath,
                LastModificationDate = DateTime.Now
            });
        }
    }
}

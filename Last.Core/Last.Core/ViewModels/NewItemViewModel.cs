using Last.Core.Models;
using System;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class NewItemViewModel : ItemDetailViewModel
    {
        public NewItemViewModel()
        {
            MainCommand = new Command(SaveExecute);
            ButtonTitle = "Save";
        }

        private async void SaveExecute()
        {
            await Navigation.PopAsync();
            MessagingCenter.Send(this, "Save", new Item()
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

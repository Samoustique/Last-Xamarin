using Last.Core.Models;
using Last.Core.Services;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class UpdateItemViewModel : ItemDetailViewModel
    {
        public Item Item { get; }

        public UpdateItemViewModel(Item item, Messaging messaging) : base(messaging)
        {
            MainCommand = new Command(UpdateExecute);
            ButtonTitle = "Update";
            Item = item;
            Text = Item.Text;
            Count = Item.Count;
            Description = Item.Description;
            ImagePath = Item.ImagePath;
            Image = string.IsNullOrEmpty(Item.ImagePath) ? null : ImageSource.FromFile(Item.ImagePath);
        }

        private async void UpdateExecute()
        {
            Item.Text = Text;
            Item.Count = Count;
            Item.Description = Description;
            Item.ImagePath = ImagePath;
            await Navigation.PopAsync();
            Messaging.SendUpdate(this, Item);
        }
    }
}

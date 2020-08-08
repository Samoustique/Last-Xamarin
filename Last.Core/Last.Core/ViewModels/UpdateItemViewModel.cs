using Last.Core.Message;
using Last.Core.Models;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    interface IItemUpdater
    {
    }

    public class UpdateItemViewModel : ItemDetailViewModel, IItemUpdater
    {
        public Item Item { get; }

        public UpdateItemViewModel(Item item)
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
            MessagingCenter.Send(this as IItemUpdater, string.Empty, new UpdateItemMessage() { Item = Item });
        }
    }
}

using Last.Core.Models;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class UpdateItemViewModel : ItemDetailViewModel
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
        }

        private async void UpdateExecute()
        {
            Item.Text = Text;
            Item.Count = Count;
            Item.Description = Description;
            await Navigation.PopAsync();
            MessagingCenter.Send(this, "Update", Item);
        }
    }
}

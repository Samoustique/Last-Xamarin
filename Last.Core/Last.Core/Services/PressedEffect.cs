using Last.Core.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;

namespace Last.Core.Services
{
    /// <summary>
    /// Long pressed effect. Used for invoking commands on long press detection cross platform
    /// </summary>
    public class PressedEffect : RoutingEffect
    {
        public PressedEffect() : base("Last.Core.Services.PressedEffect")
        {
        }

        public static readonly BindableProperty LongClickCommandProperty = BindableProperty.CreateAttached("LongClickCommand", typeof(ICommand), typeof(PressedEffect), (object)null);
        public static ICommand GetLongClickCommand(BindableObject view)
        {
            return (ICommand) view.GetValue(LongClickCommandProperty);
        }

        public static void SetLongClickCommand(BindableObject view, ICommand value)
        {
            view.SetValue(LongClickCommandProperty, value);
        }

        public static readonly BindableProperty ClickCommandProperty = BindableProperty.CreateAttached("ClickCommand", typeof(ICommand), typeof(PressedEffect), (object)null);
        public static ICommand GetClickCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(ClickCommandProperty);
        }

        public static void SetClickCommand(BindableObject view, ICommand value)
        {
            view.SetValue(ClickCommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(PressedEffect), (object)null);
        public static object GetCommandParameter(BindableObject view)
        {
            return view.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject view, ItemListViewModel value)
        {
            view.SetValue(CommandParameterProperty, value);
        }
    }
}

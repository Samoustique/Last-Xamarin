using System;
using Last.Core.Droid.Services;
using Last.Core.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Last.Core.Services")]
[assembly: ExportEffect(typeof(AndroidPressedEffect), "PressedEffect")]
namespace Last.Core.Droid.Services
{
    /// <summary>
    /// Android long pressed effect.
    /// </summary>
    public class AndroidPressedEffect : PlatformEffect
    {
        private bool _attached;

        /// <summary>
        /// Initializer to avoid linking out
        /// </summary>
        public static void Initialize() { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Yukon.Application.AndroidComponents.Effects.AndroidPressedEffect"/> class.
        /// Empty constructor required for the odd Xamarin.Forms reflection constructor search
        /// </summary>
        public AndroidPressedEffect()
        {
        }

        /// <summary>
        /// Apply the handler
        /// </summary>
        protected override void OnAttached()
        {
            //because an effect can be detached immediately after attached (happens in listview), only attach the handler one time.
            if (!_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick += Control_LongClick;
                    Control.Click += Control_Click;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += Control_LongClick;
                    Container.Click += Control_Click;
                }
                _attached = true;
            }
        }

        /// <summary>
        /// Invoke the command if there is one
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Control_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            Console.WriteLine("Invoking long click command");
            var command = PressedEffect.GetLongClickCommand(Element);
            command?.Execute(PressedEffect.GetCommandParameter(Element));
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Invoking click command");
            var command = PressedEffect.GetClickCommand(Element);
            command?.Execute(PressedEffect.GetCommandParameter(Element));
        }

        /// <summary>
        /// Clean the event handler on detach
        /// </summary>
        protected override void OnDetached()
        {
            if (_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= Control_LongClick;
                    Control.Click -= Control_Click;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= Control_LongClick;
                    Container.Click -= Control_Click;
                }
                _attached = false;
            }
        }
    }
}
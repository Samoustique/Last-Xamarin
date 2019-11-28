﻿using Last.Core.iOS.Services;
using Last.Core.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Last.Core.Services")]
[assembly: ExportEffect(typeof(iOSPressedEffect), "PressedEffect")]
namespace Last.Core.iOS.Services
{
    /// <summary>
    /// iOS long pressed effect
    /// </summary>
    public class iOSPressedEffect : PlatformEffect
    {
        private bool _attached;
        private readonly UILongPressGestureRecognizer _longPressRecognizer;
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Yukon.Application.iOSComponents.Effects.iOSPressedEffect"/> class.
        /// </summary>
        public iOSPressedEffect()
        {
            _longPressRecognizer = new UILongPressGestureRecognizer(HandleLongClick);
        }

        /// <summary>
        /// Apply the handler
        /// </summary>
        protected override void OnAttached()
        {
            //because an effect can be detached immediately after attached (happens in listview), only attach the handler one time
            if (!_attached)
            {
                Container.AddGestureRecognizer(_longPressRecognizer);
                _attached = true;
            }
        }

        /// <summary>
        /// Invoke the command if there is one
        /// </summary>
        private void HandleLongClick()
        {
            var command = PressedEffect.GetLongClickCommand(Element);
            command?.Execute(PressedEffect.GetCommandParameter(Element));
        }

        /// <summary>
        /// Clean the event handler on detach
        /// </summary>
        protected override void OnDetached()
        {
            if (_attached)
            {
                Container.RemoveGestureRecognizer(_longPressRecognizer);
                _attached = false;
            }
        }
    }
}
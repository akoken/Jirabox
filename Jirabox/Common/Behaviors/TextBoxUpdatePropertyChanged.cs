using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Jirabox.Common.Behaviors
{  
    public class TextBoxUpdatePropertyChanged : Behavior<TextBox>
    {
        /// <summary>
        /// Binding expression this behavior is attached to.
        /// </summary>
        private BindingExpression expression;

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            // Hook events to change behavior
            expression = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            AssociatedObject.TextChanged += OnTextChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            // Un-hook events
            AssociatedObject.TextChanged -= OnTextChanged;
            expression = null;
        }

        /// <summary>
        /// Updates the source property when the text is changed.
        /// </summary>
        private void OnTextChanged(object sender, EventArgs args)
        {
            expression.UpdateSource();
        }
    }
}



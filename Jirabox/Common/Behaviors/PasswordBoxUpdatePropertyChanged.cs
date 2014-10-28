using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Jirabox.Common.Behaviors
{
    public class PasswordBoxUpdatePropertyChanged : Behavior<PasswordBox>
    {
        private BindingExpression expression;
        protected override void OnAttached()
        {
            base.OnAttached();
            expression = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            AssociatedObject.PasswordChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnTextChanged;
            expression = null;
        }

        private void OnTextChanged(object sender, EventArgs args)
        {
            expression.UpdateSource();
        }
    }
}

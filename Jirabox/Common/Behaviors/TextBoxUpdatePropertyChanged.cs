using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Jirabox.Common.Behaviors
{  
    public class TextBoxUpdatePropertyChanged : Behavior<TextBox>
    {       
        private BindingExpression expression;
        protected override void OnAttached()
        {
            base.OnAttached();         
            expression = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged -= OnTextChanged;
            expression = null;
        }
      
        private void OnTextChanged(object sender, EventArgs args)
        {
            expression.UpdateSource();
        }
    }
}



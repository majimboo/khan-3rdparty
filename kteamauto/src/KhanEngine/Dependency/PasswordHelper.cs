using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KhanBlaze.Dependency
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.RegisterAttached("Password",
        typeof(string), typeof(PasswordHelper),
        new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordHelper));

        public static DependencyProperty OnLostFocusProperty = DependencyProperty.RegisterAttached(
            "OnLostFocus",
            typeof(ICommand),
            typeof(PasswordHelper),
            new UIPropertyMetadata(PasswordHelper.OnLostFocus));

        public static void SetOnLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(PasswordHelper.OnLostFocusProperty, value);
        }

        /// <summary>
        /// PropertyChanged callback for OnDoubleClickProperty.  Hooks up an event handler with the
        /// actual target.
        /// </summary>
        private static void OnLostFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is PasswordBox element))
            {
                throw new InvalidOperationException("This behavior can be attached to a PasswordBox item only.");
            }

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                element.LostFocus += OnPreviewLostFocus;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                element.LostFocus -= OnPreviewLostFocus;
            }
        }

        private static void OnPreviewLostFocus(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(PasswordHelper.OnLostFocusProperty);
            if (command != null)
            {
                command.Execute(e);
            }
        }

        /// <summary>
        /// End Lost Focus
        /// </summary>
        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
using System;
using System.Windows;

namespace TypeCode.Wpf.Jab.Helper.MessageBoxes;

public class InteractionBox
{
    public static MessageBoxResult Show(string message,string title, MessageBoxButton buttons = MessageBoxButton.YesNo, MessageBoxImage image = MessageBoxImage.Question)
    {
        var messageBoxResult = MessageBox.Show(message, title, buttons, image);

        switch (messageBoxResult)
        {
            case MessageBoxResult.Yes:
                return MessageBoxResult.Yes;
            case MessageBoxResult.No:
                return MessageBoxResult.No;
            case MessageBoxResult.None:
                return MessageBoxResult.None;
            case MessageBoxResult.OK:
                return MessageBoxResult.OK;
            case MessageBoxResult.Cancel:
                return MessageBoxResult.Cancel;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
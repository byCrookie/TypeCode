using System;
using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Modal.Service;

public class ModalParameter
{
    public string Title { get; set; }
    public string Text { get; set; }
    public Func<Task> OnCloseAsync { get; set; }
}
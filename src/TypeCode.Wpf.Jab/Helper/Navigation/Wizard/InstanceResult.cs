using System;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard;

public class InstanceResult
{
    public Type ViewType { get; set; }
    public object ViewInstance { get; set; }
    public Type ViewModelType { get; set; }
    public object ViewModelInstance { get; set; }
}
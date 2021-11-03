using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public class Wizard
    {
        public Wizard()
        {
            StepConfigurations = new List<WizardStepConfiguration>();
            CompletedAction = _ => Task.CompletedTask;
        }
        
        public List<WizardStepConfiguration> StepConfigurations { get; set; }
        public WizardStepConfiguration CurrentStepConfiguration { get; set; }
        public Frame NavigationFrame { get; set; }
        public UIElement Content { get; set; }
        public UIElement WizardOverlay { get; set; }
        public InstanceResult WizardInstances { get; set; }
        public NavigationContext NavigationContext { get; set; }
        public Func<NavigationContext,Task> CompletedAction { get; set; }
    }
}
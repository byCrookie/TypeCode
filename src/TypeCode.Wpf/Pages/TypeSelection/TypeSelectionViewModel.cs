using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Framework.Extensions.List;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.TypeSelection
{
    public class TypeSelectionViewModel : Reactive, IAsyncNavigatedTo
    {
        public TypeSelectionViewModel()
        {
            Types = new ObservableCollection<TypeItemViewModel>();
        }
        
        public Task OnNavigatedToAsync(NavigationContext context)
        {
            var parameter = context.GetParameter<TypeSelectionParameter>();
            parameter.Types.ForEach(t => Types.Add(new TypeItemViewModel(t)));
            SelectionMode = parameter.AllowMultiSelection ? SelectionMode.Multiple : SelectionMode.Single;
            return Task.CompletedTask;
        }

        public SelectionMode SelectionMode { get; set; }

        public ObservableCollection<TypeItemViewModel> Types
        {
            get => Get<ObservableCollection<TypeItemViewModel>>();
            set => Set(value);
        }

        public IEnumerable<Type> SelectedTypes
        {
            get
            {
                return Types
                    .Where(t => t.IsSelected)
                    .Select(t => t.Type);
            }
        }
    }
}
using System.Collections;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Helper.ViewModels;

public static class ErrorEvaluator
{
    public static bool HasErrors(object? viewModel)
    {
        switch (viewModel)
        {
            case null:
                throw new ArgumentNullException(nameof(viewModel));
            case ObservableValidator observableValidator:
            {
                var fields = viewModel.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<ChildViewModelAttribute>() is not null)
                    {
                        var fieldValue = field.GetValue(viewModel) ?? throw new Exception($"Can not get value of field {field.Name} of type {viewModel.GetType().FullName}");
                        if (fieldValue is IEnumerable values)
                        {
                            var hasErrors = false;

                            foreach (var value in values)
                            {
                                if (HasErrors(value))
                                {
                                    hasErrors = true;
                                }
                            }

                            return observableValidator.HasErrors || hasErrors;
                        }

                        return observableValidator.HasErrors || HasErrors(fieldValue);
                    }
                }

                return observableValidator.HasErrors;
            }
        }

        return false;
    }
}
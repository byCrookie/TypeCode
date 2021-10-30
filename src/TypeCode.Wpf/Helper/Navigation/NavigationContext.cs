using System;
using System.Collections.Generic;

namespace TypeCode.Wpf.Helper.Navigation
{
    public class NavigationContext
    {
        private readonly IDictionary<Type, object> _parameters;

        public NavigationContext()
        {
            _parameters = new Dictionary<Type, object>();
        }

        public void AddParameter(object parameter)
        {
            _parameters.Add(parameter.GetType(), parameter);
        }
        
        public T GetParameter<T>()
        {
            if (_parameters.TryGetValue(typeof(T), out var parameter))
            {
                return (T)parameter;
            }

            return default;
        }
    }
}
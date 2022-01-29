using System;
using System.Collections.Generic;

namespace TypeCode.Wpf.Helper.Navigation.Service;

public class NavigationContext
{
    private readonly IDictionary<Type, object> _parameters;
    private readonly IDictionary<string, object> _parametersKey;

    public NavigationContext()
    {
        _parameters = new Dictionary<Type, object>();
        _parametersKey = new Dictionary<string, object>();
    }
        
    public void AddParameter(string key, object parameter)
    {
        _parametersKey.Add(key, parameter);
    }
        
    public void AddOrUpdateParameter(string key, object parameter)
    {
        if (_parametersKey.ContainsKey(key))
        {
            _parametersKey.Remove(key);
        }
        else
        {
            _parametersKey.Add(key, parameter);
        }
    }

    public void AddParameter(object parameter)
    {
        _parameters.Add(parameter.GetType(), parameter);
    }
        
    public void AddOrUpdateParameter(object parameter)
    {
        if (_parameters.ContainsKey(parameter.GetType()))
        {
            _parameters.Remove(parameter.GetType());
        }
        else
        {
            _parameters.Add(parameter.GetType(), parameter);
        }
    }
        
    public T GetParameter<T>()
    {
        if (_parameters.TryGetValue(typeof(T), out var parameter))
        {
            return (T)parameter;
        }

        return default;
    }
        
    public T GetParameter<T>(string key)
    {
        if (_parametersKey.TryGetValue(key, out var parameter))
        {
            return (T)parameter;
        }

        return default;
    }
}
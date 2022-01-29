using System;
using Autofac;
using TypeCode.Business.Format;

namespace TypeCode.Wpf.Helper.Autofac
{
    public static class ViewModelExtensions
    {
        public static Type GetViewType(this Type viewModelType)
        {
            var viewName = NameBuilder.GetNameWithoutGeneric(viewModelType)[..^"Model".Length];
            return Type.GetType($"{viewModelType.Namespace}.{viewName}");
        }
        
        public static void AddViewModelAndViewAsSingleInstance<TViewModel, TView>(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TViewModel>().AsSelf().SingleInstance();
            containerBuilder.RegisterType<TView>().AsSelf().SingleInstance();
        }

        public static void AddViewModelAndView<TViewModel, TView>(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TViewModel>().AsSelf();
            containerBuilder.RegisterType<TView>().AsSelf();
        }

        public static void AddGenericViewModelAndView(this ContainerBuilder containerBuilder, Type viewModelType, Type viewType)
        {
            containerBuilder.RegisterGeneric(viewModelType).AsSelf();
            containerBuilder.RegisterType(viewType).AsSelf();
        }
    }
}
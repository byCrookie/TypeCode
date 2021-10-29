using Autofac;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Specflow;

namespace TypeCode.Wpf
{
    public class TypeCodeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SpecflowModule>();
            builder.RegisterModule<HelperModule>();
            
            base.Load(builder);
        }
    }
}
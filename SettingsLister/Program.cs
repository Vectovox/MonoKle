using MoreLinq;
using System;
using System.Linq;
using System.Reflection;

namespace SettingsLister
{
    class Program
    {
        static void Main()
        {
            var types = typeof(MonoKle.MVector2).Assembly.GetTypes().Concat(typeof(MonoKle.Engine.MonoKleSettings).Assembly.GetTypes());
            var properties = types.SelectMany(t => t.GetProperties());
            var pairs = properties.Select(property => ( property, attribute:  property.GetCustomAttribute<MonoKle.Configuration.CVarAttribute>()))
                                  .Where(pair => pair.attribute != null);

            pairs.OrderBy(p => p.attribute.Identifier)
                 .ForEach(pair => Console.WriteLine($"{pair.attribute.Identifier} : {pair.property.DeclaringType.Name}"));
        }
    }
}

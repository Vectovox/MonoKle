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
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<MonoKle.Configuration.CVarAttribute>();
                if (attribute != null)
                {
                    Console.WriteLine($"{attribute.Identifier} : {property.DeclaringType.Name}");
                }
            }
        }
    }
}

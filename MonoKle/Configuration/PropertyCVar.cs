using System;
using System.Reflection;

namespace MonoKle.Configuration
{
    /// <summary>
    /// Class for a property-based variable, binding to a specific property.
    /// </summary>
    public class PropertyCVar : ICVar
    {
        private readonly object? _owner;
        private readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCVar"/> class for instanced properties.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="owner">The owner instance of the property.</param>
        public PropertyCVar(string propertyName, object owner) :
            this(owner.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCVar"/> class for static properties.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="staticType">The static type containing the property.</param>
        public PropertyCVar(string propertyName, Type staticType) :
            this(staticType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCVar"/> class from a <see cref="PropertyInfo"/> and eventual owner.
        /// </summary>
        /// <param name="property">The property to use.</param>
        /// <param name="owner">The owner. Must be null for static properties.</param>
        public PropertyCVar(PropertyInfo property, object? owner)
        {
            if (property == null)
            {
                throw new ArgumentException("Not a valid property.");
            }
            if (owner != null && property.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException("Static properties must have null owner.");
            }
            if (owner == null && property.GetGetMethod(true).IsStatic == false)
            {
                throw new ArgumentNullException("Non-static properties must have non-null owner.");
            }

            _property = property;
            _owner = owner;
        }

        public Type Type => _property.PropertyType;

        public bool CanSet() => _property.SetMethod != null;

        public object GetValue() => _property.GetValue(_owner);

        public bool SetValue(object value)
        {
            if (CanSet())
            {
                Type valueType = value.GetType();
                if (_property.PropertyType.IsAssignableFrom(valueType))
                {
                    _property.SetValue(_owner, value);
                    return true;
                }
                else if (_property.PropertyType.IsValueType && valueType.IsValueType)
                {
                    try
                    {
                        object newObj = Convert.ChangeType(value, _property.PropertyType);
                        _property.SetValue(_owner, newObj);
                        return true;
                    }
                    catch (InvalidCastException)
                    {
                    }
                }
            }
            return false;
        }
    }
}
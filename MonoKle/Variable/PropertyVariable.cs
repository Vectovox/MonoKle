namespace MonoKle.Variable {
    using System;
    using System.Reflection;

    /// <summary>
    /// Class for a property-based variable.
    /// </summary>
    public class PropertyVariable : IVariable {
        private object owner;
        private PropertyInfo property;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyVariable"/> class for instanced properties.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="owner">The owner instance of the property.</param>
        public PropertyVariable(string propertyName, object owner) :
            this(owner.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), owner) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyVariable"/> class for static properties.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="staticType">The static type containing the property.</param>
        public PropertyVariable(string propertyName, Type staticType) :
            this(staticType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyVariable"/> class from a <see cref="PropertyInfo"/> and eventual owner.
        /// </summary>
        /// <param name="property">The property to use.</param>
        /// <param name="owner">The owner. Must be null for static properties.</param>
        public PropertyVariable(PropertyInfo property, object owner) {
            if (property == null) {
                throw new ArgumentException("Not a valid property.");
            }
            if (property.GetGetMethod().IsStatic && owner != null) {
                throw new ArgumentException("Static properties must have null owner.");
            }
            if (property.GetGetMethod().IsStatic == false && owner == null) {
                throw new ArgumentNullException("Non-static properties must have non-null owner.");
            }

            this.property = property;
            this.owner = owner;
        }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type => CanSet() ? property.PropertyType : null;

        /// <summary>
        /// Determines whether this instance can set.
        /// </summary>
        /// <returns></returns>
        public bool CanSet() => property.SetMethod != null;

        /// <summary>
        /// Gets the variable value.
        /// </summary>
        /// <returns></returns>
        public object GetValue() => property.GetValue(owner);

        /// <summary>
        /// Sets the variable to the provided value if possible.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>True if value could be set; otherwise false.</returns>
        public bool SetValue(object value) {
            if (CanSet()) {
                Type valueType = value.GetType();
                if (property.PropertyType.IsAssignableFrom(valueType)) {
                    property.SetValue(owner, value);
                    return true;
                } else if (property.PropertyType.IsValueType && valueType.IsValueType) {
                    try {
                        object newObj = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(owner, newObj);
                        return true;
                    } catch (InvalidCastException e) {
                    }
                }
            }
            return false;
        }
    }
}
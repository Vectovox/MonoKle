namespace MonoKle.Setting
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Class for a property-based setting instance.
    /// </summary>
    public class PropertySetting : ISettingInstance
    {
        private object owner;
        private PropertyInfo property;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetting"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="owner">The owner instance of the property.</param>
        public PropertySetting(string name, object owner)
        {
            this.property = owner.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            this.owner = owner;

            if (this.property == null)
            {
                throw new ArgumentException("No such property for the provided instance: " + name);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetting"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="staticType">The static type containing the property.</param>
        public PropertySetting(string name, Type staticType)
        {
            this.property = staticType.GetProperty(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (this.property == null)
            {
                throw new ArgumentException("No such property for the provided class: " + name);
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return this.property.GetValue(owner);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(object value)
        {
            this.property.SetValue(owner, value);
        }
    }
}
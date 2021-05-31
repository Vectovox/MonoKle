﻿using MonoKle.Logging;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoKle.Configuration
{
    /// <summary>
    /// Class binding and storing variables.
    /// </summary>
    public class CVarSystem : ILogged
    {
        private readonly Dictionary<string, ICVar> _variables = new Dictionary<string, ICVar>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CVarSystem"/> class.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public CVarSystem(Logger logger) => Logger = logger;

        /// <summary>
        /// Gets the variable identifiers.
        /// </summary>
        /// <value>
        /// The variable identifiers.
        /// </value>
        public ICollection<string> Identifiers => _variables.Keys;

        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public Logger Logger { get; set; }

        /// <summary>
        /// Binds the specified instance to the specified variable identifier. Any existing values will be assigned the instance.
        /// </summary>
        /// <param name="instance">The instance to bind.</param>
        /// <param name="identifier">The identifier to register for.</param>
        public void Bind(ICVar instance, string identifier) => Bind(instance, identifier, true);

        /// <summary>
        /// Binds the specified instance to the specified variable identifier.
        /// </summary>
        /// <param name="instance">The instance to bind.</param>
        /// <param name="identifier">The identifier to register for.</param>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound instance.</param>
        public void Bind(ICVar instance, string identifier, bool assignOld)
        {
            if (_variables.ContainsKey(identifier))
            {
                object oldValue = GetValue(identifier);
                _variables[identifier] = instance;

                if (assignOld)
                {
                    SetValue(identifier, oldValue);
                }
            }
            else
            {
                _variables.Add(identifier, instance);
            }
            Log($"Bound variable instance to identifier: {identifier}", LogLevel.Debug);
        }

        /// <summary>
        /// Binds the properties declared with <see cref="CVarAttribute"/> of the provided object.
        /// </summary>
        /// <remarks>
        /// Assigns any existing value to the bound instance. 
        /// </remarks>
        /// <param name="instance">The object instance to bind.</param>
        public void BindProperties(object instance) => BindProperties(instance, true);

        /// <summary>
        /// Binds the properties declared with <see cref="CVarAttribute"/> of the provided object.
        /// </summary>
        /// <param name="instance">The object instance to bind.</param>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound instance.</param>
        public void BindProperties(object instance, bool assignOld)
        {
            GetInstanceProperties(instance).ForEach(t => Bind(new PropertyCVar(t.Property, instance), t.Attribute.Identifier, assignOld));
            BindProperties(instance.GetType());
        }

        /// <summary>
        /// Binds the static properties declared with <see cref="CVarAttribute"/> of the provided type argument.
        /// </summary>
        /// <remarks>
        /// Assigns any existing value to the bound type. 
        /// </remarks>
        public void BindProperties<T>() => BindProperties(typeof(T));

        /// <summary>
        /// Binds the static properties declared with <see cref="CVarAttribute"/> of the provided type argument.
        /// </summary>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound type.</param>
        public void BindProperties<T>(bool assignOld) => BindProperties(typeof(T), assignOld);

        /// <summary>
        /// Binds the static properties declared with <see cref="CVarAttribute"/> of the provided type.
        /// </summary>
        /// <remarks>
        /// Assigns any existing value to the bound type. 
        /// </remarks>
        /// <param name="type">The type to bind static properties to.</param>
        public void BindProperties(Type type) => BindProperties(type, true);

        /// <summary>
        /// Binds the static properties declared with <see cref="CVarAttribute"/> of the provided type.
        /// </summary>
        /// <param name="type">The type to bind static properties to.</param>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound type.</param>
        public void BindProperties(Type type, bool assignOld) =>
            GetClassProperties(type).ForEach(t => Bind(new PropertyCVar(t.Property, null), t.Attribute.Identifier, assignOld));

        /// <summary>
        /// Determines whether the specified variable can be set or is read-only.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <returns>True if it can be set; otherwise false.</returns>
        public bool CanSet(string identifier) => TryGetVariable(identifier, out var variable) && variable.CanSet();

        /// <summary>
        /// Clears all variables.
        /// </summary>
        public void Clear()
        {
            _variables.Clear();
            Log("Cleared variables", LogLevel.Debug);
        }

        /// <summary>
        /// Determines whether this instance contains a variable with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>True if a variable with the specified identifier exists; otherwise false.</returns>
        public bool Contains(string identifier) => TryGetVariable(identifier, out _);

        /// <summary>
        /// Gets the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier variable to get.</param>
        public object GetValue(string identifier)
        {
            if (TryGetVariable(identifier, out var variable))
            {
                return variable.GetValue();
            }
            throw new ArgumentException($"Variable with identifier '{identifier}' does not exist.");
        }

        /// <summary>
        /// Unbinds and removes the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier of the variable to remove.</param>
        /// <returns>True if a variable was removed; otherwise false.</returns>
        public bool Unbind(string identifier)
        {
            Log($"Removed occurences of variable: {identifier}", LogLevel.Debug);
            return _variables.Remove(identifier);
        }

        /// <summary>
        /// Unbinds the properties declared with <see cref="CVarAttribute"/> of the provided object.
        /// </summary>
        /// <param name="instance">The object instance to find attributes on.</param>
        /// <returns>The amount of variables unbound.</returns>
        public int UnbindProperties(object instance) =>
            GetInstanceProperties(instance).Sum(t => Unbind(t.Attribute.Identifier) ? 1 : 0)
                + UnbindProperties(instance.GetType());

        /// <summary>
        /// Unbinds the static properties declared with <see cref="CVarAttribute"/> of the provided type argument.
        /// </summary>
        /// <returns>The amount of variables unbound.</returns>
        public int UnbindProperties<T>() => UnbindProperties(typeof(T));

        /// <summary>
        /// Unbinds the static properties declared with <see cref="CVarAttribute"/> of the provided type.
        /// </summary>
        /// <param name="type">The type to unbind static properties from.</param>
        /// <returns>The amount of variables unbound.</returns>
        public int UnbindProperties(Type type) => GetClassProperties(type).Sum(t => Unbind(t.Attribute.Identifier) ? 1 : 0);

        /// <summary>
        /// Sets the value of the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <param name="value">The value to assign.</param>
        public bool SetValue(string identifier, object value)
        {
            if (_variables.TryGetValue(identifier, out var variable))
            {
                // Existing variable so set the value
                if (!variable.SetValue(value))
                {
                    Log($"Could not set variable '{identifier}' to '{value}'", LogLevel.Error);
                    return false;
                }
            }
            else
            {
                // No existing variable so add it
                _variables.Add(identifier, new ValueCVar(value));
                Log($"Added new variable '{identifier}'", LogLevel.Trace);
            }
            Log($"Set value of variable '{identifier}' to '{value}'", LogLevel.Trace);
            return true;
        }

        /// <summary>
        /// Logs the given message with the provided logging level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The logging level.</param>
        protected void Log(string message, LogLevel level) => Logger.Log(message, level);

        /// <summary>
        /// Gets the variable associated with the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier to lookup.</param>
        /// <param name="variable">Contains the associated variable if one exists for the given identifier; otherwise a void variable.</param>
        /// <returns>True if a variable with the associated identifier exists; otherwise false.</returns>
        public bool TryGetVariable(string identifier, out ICVar variable)
        {
            if (_variables.ContainsKey(identifier))
            {
                variable = _variables[identifier];
                return true;
            }
            variable = VoidVariable;
            return false;
        }

        private static ICVar VoidVariable { get; } = new ValueCVar(new object());

        private IEnumerable<(PropertyInfo Property, CVarAttribute Attribute)> GetInstanceProperties(object instance)
        {
            foreach (PropertyInfo property in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (CVarAttribute attribute in property.GetCustomAttributes(typeof(CVarAttribute)))
                {
                    yield return (property, attribute);
                }
            }
        }

        private IEnumerable<(PropertyInfo Property, CVarAttribute Attribute)> GetClassProperties(Type type)
        {
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic))
            {
                foreach (CVarAttribute attribute in property.GetCustomAttributes(typeof(CVarAttribute)))
                {
                    yield return (property, attribute);
                }
            }
        }
    }
}
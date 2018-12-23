namespace MonoKle.Variable {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Logging;

    /// <summary>
    /// Class binding and storing variables.
    /// </summary>
    public class VariableSystem : ILogged {
        private Dictionary<string, IVariable> variables = new Dictionary<string, IVariable>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSystem"/> class.
        /// </summary>
        public VariableSystem() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSystem"/> class.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public VariableSystem(Logger logger) {
            Logger = logger;
        }

        /// <summary>
        /// Gets the variable identifiers.
        /// </summary>
        /// <value>
        /// The variable identifiers.
        /// </value>
        public ICollection<string> Identifiers => variables.Keys;

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
        public void Bind(IVariable instance, string identifier) => Bind(instance, identifier, true);

        /// <summary>
        /// Binds the specified instance to the specified variable identifier.
        /// </summary>
        /// <param name="instance">The instance to bind.</param>
        /// <param name="identifier">The identifier to register for.</param>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound instance.</param>
        public void Bind(IVariable instance, string identifier, bool assignOld) {
            identifier = identifier.ToLower();
            if (variables.ContainsKey(identifier)) {
                object oldValue = GetValue(identifier);
                variables[identifier] = instance;

                if (assignOld) {
                    SetValue(identifier, oldValue);
                }
            } else {
                variables.Add(identifier, instance);
            }
            Log("Bound variable instance to identifier: " + identifier, LogLevel.Debug);
        }

        /// <summary>
        /// Binds the properties of the provided object to variables. Only properties declared with <see cref="PropertyVariableAttribute"/> are bound.
        /// </summary>
        /// <param name="o">The object.</param>
        public void BindProperties(object o) {
            Type type = o.GetType();
            foreach (PropertyInfo p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                foreach (object a in p.GetCustomAttributes(true)) {
                    var attribute = a as PropertyVariableAttribute;
                    if (attribute != null) {
                        Bind(new PropertyVariable(p, o), attribute.Identifier);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified variable can be set or is read-only.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <returns>True if it can be set; otherwise false.</returns>
        public bool CanSet(string identifier) {
            IVariable variable = GetVariable(identifier, false);
            if (variable != null) {
                return variable.CanSet();
            }
            return false;
        }

        /// <summary>
        /// Clears all variables.
        /// </summary>
        public void Clear() {
            variables.Clear();
            Log("Cleared variables", LogLevel.Debug);
        }

        /// <summary>
        /// Determines whether this instance contains a variable with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>True if a variable with the specified identifier exists; otherwise false.</returns>
        public bool Contains(string identifier) => GetVariable(identifier, false) != null;

        /// <summary>
        /// Gets the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier variable to get.</param>
        /// <returns>Setting value.</returns>
        public object GetValue(string identifier) {
            IVariable variable = GetVariable(identifier);
            if (variable != null) {
                return variable.GetValue();
            }
            return null;
        }

        /// <summary>
        /// Removes the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier of the variable to remove.</param>
        /// <returns>True if a variable was removed; otherwise false.</returns>
        public bool Remove(string identifier) {
            identifier = identifier.ToLower();
            Log("Removed occurences of variable: " + identifier, LogLevel.Debug);
            return variables.Remove(identifier);
        }

        /// <summary>
        /// Sets the value of the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <param name="value">The value to assign.</param>
        public bool SetValue(string identifier, object value) {
            identifier = identifier.ToLower();
            if (variables.ContainsKey(identifier)) {
                if (variables[identifier].SetValue(value) == false) {
                    Log("Could not set variable '" + identifier + "' to " + value, LogLevel.Error);
                    return false;
                }
            } else {
                Log("Added new variable: " + identifier, LogLevel.Trace);
                variables.Add(identifier, new ValueVariable(value));
            }
            Log("Set value of variable '" + identifier + "' to " + value, LogLevel.Trace);
            return true;
        }

        /// <summary>
        /// Logs the given message with the provided logging level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The logging level.</param>
        protected void Log(string message, LogLevel level) {
            if (Logger != null) {
                Logger.Log(message, level);
            }
        }

        /// <summary>
        /// Gets the variable with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public IVariable GetVariable(string identifier) => GetVariable(identifier, true);

        private IVariable GetVariable(string identifier, bool generateWarning) {
            identifier = identifier.ToLower();
            if (variables.ContainsKey(identifier)) {
                return variables[identifier];
            } else if (generateWarning) {
                Log("Accessed variable does not exist: " + identifier, LogLevel.Warning);
            }
            return null;
        }
    }
}
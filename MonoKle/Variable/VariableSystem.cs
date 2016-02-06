namespace MonoKle.Variable
{
    using Attributes;
    using Logging;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Class binding and storing variables.
    /// </summary>
    public class VariableSystem : ILoggable
    {
        private Dictionary<string, IVariable> variables = new Dictionary<string, IVariable>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSystem"/> class.
        /// </summary>
        public VariableSystem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSystem"/> class.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public VariableSystem(Logger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the variable identifiers.
        /// </summary>
        /// <value>
        /// The variable identifiers.
        /// </value>
        public ICollection<string> Identifiers
        {
            get { return variables.Keys; }
        }

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
        public void Bind(IVariable instance, string identifier)
        {
            this.Bind(instance, identifier, true);
        }

        /// <summary>
        /// Binds the specified instance to the specified variable identifier.
        /// </summary>
        /// <param name="instance">The instance to bind.</param>
        /// <param name="identifier">The identifier to register for.</param>
        /// <param name="assignOld">If set to true, assigns any existing value to the bound instance.</param>
        public void Bind(IVariable instance, string identifier, bool assignOld)
        {
            if (variables.ContainsKey(identifier))
            {
                object oldValue = this.GetValue(identifier);
                this.variables[identifier] = instance;

                if (assignOld)
                {
                    this.SetValue(identifier, oldValue);
                }
            }
            else
            {
                this.variables.Add(identifier, instance);
            }
            this.Log("Bound variable instance to identifier: " + identifier, LogLevel.Debug);
        }

        /// <summary>
        /// Binds the properties of the provided object to variables. Only properties declared with <see cref="PropertyVariableAttribute"/> are bound.
        /// </summary>
        /// <param name="o">The object.</param>
        public void BindProperties(object o)
        {
            Type type = o.GetType();
            foreach (PropertyInfo p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (object a in p.GetCustomAttributes(true))
                {
                    PropertyVariableAttribute attribute = a as PropertyVariableAttribute;
                    if (attribute != null)
                    {
                        this.Bind(new PropertyVariable(p, o), attribute.Identifier);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified variable can be set or is read-only.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <returns>True if it can be set; otherwise false.</returns>
        public bool CanSet(string identifier)
        {
            IVariable variable = this.GetVariable(identifier);
            if (variable != null)
            {
                return variable.CanSet();
            }
            return false;
        }

        /// <summary>
        /// Clears all variables.
        /// </summary>
        public void Clear()
        {
            this.variables.Clear();
            this.Log("Cleared variables", LogLevel.Debug);
        }

        /// <summary>
        /// Determines whether this instance contains a variable with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>True if a variable with the specified identifier exists; otherwise false.</returns>
        public bool Contains(string identifier) => this.GetVariable(identifier) != null;

        /// <summary>
        /// Gets the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier variable to get.</param>
        /// <returns>Setting value.</returns>
        public object GetValue(string identifier)
        {
            IVariable variable = this.GetVariable(identifier);
            if (variable != null)
            {
                return variable.GetValue();
            }
            return null;
        }

        /// <summary>
        /// Removes the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier of the variable to remove.</param>
        /// <returns>True if a variable was removed; otherwise false.</returns>
        public bool Remove(string identifier)
        {
            this.Log("Removed occurences of variable: " + identifier, LogLevel.Debug);
            return this.variables.Remove(identifier);
        }

        /// <summary>
        /// Sets the value of the specified variable.
        /// </summary>
        /// <param name="identifier">The identifier of the variable.</param>
        /// <param name="value">The value to assign.</param>
        public bool SetValue(string identifier, object value)
        {
            if (variables.ContainsKey(identifier))
            {
                if (this.variables[identifier].SetValue(value) == false)
                {
                    this.Log("Could not set variable '" + identifier + "' to " + value, LogLevel.Error);
                    return false;
                }
            }
            else
            {
                this.Log("Added new variable: " + identifier, LogLevel.Trace);
                this.variables.Add(identifier, new ValueVariable(value));
            }
            this.Log("Set value of variable '" + identifier + "' to " + value, LogLevel.Trace);
            return true;
        }

        /// <summary>
        /// Logs the given message with the provided logging level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The logging level.</param>
        protected void Log(string message, LogLevel level)
        {
            if (this.Logger != null)
            {
                this.Logger.Log(message, level);
            }
        }

        private IVariable GetVariable(string identifier)
        {
            if (variables.ContainsKey(identifier))
            {
                return this.variables[identifier];
            }

            this.Log("Accessed variable does not exist: " + identifier, LogLevel.Warning);
            return null;
        }
    }
}
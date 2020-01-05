﻿using System;

namespace MonoKle.Scripting
{
    /// <summary>
    /// Class for a static script source that does not change over time.
    /// </summary>
    /// <seealso cref="IScriptSource" />
    public class StaticScriptSource : IScriptSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticScriptSource"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public StaticScriptSource(string source)
        {
            Code = source;
        }

        /// <summary>
        /// Gets the source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        public string Code { get { return code; } set { code = value; Date = DateTime.UtcNow; } }
        private string code;

        /// <summary>
        /// Gets the source date in UTC.
        /// </summary>
        /// <value>
        /// The source date in UTC.
        /// </value>
        public DateTime Date { get; private set; }
    }
}
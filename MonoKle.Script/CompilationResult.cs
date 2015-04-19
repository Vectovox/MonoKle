namespace MonoKle.Script
{
    using System.Collections.Generic;

    /// <summary>
    /// Compilation result for MonoKle scripts.
    /// </summary>
    public class CompilationResult
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="CompilationResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; private set; }
        
        /// <summary>
        /// Gets the errors associated with the compilation.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationResult"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public CompilationResult(List<string> errors)
        {
            this.Errors = errors;
            this.Success = this.Errors == null || this.Errors.Count == 0;
        }
    }
}

namespace MonoKle.Script
{
    using System.Collections.Generic;

    public class CompilationResult
    {
        public bool Success { get; private set; }

        public List<string> Errors { get; private set; }

        public CompilationResult(List<string> errors)
        {
            this.Errors = errors;
            this.Success = this.Errors == null || this.Errors.Count == 0;
        }
    }
}

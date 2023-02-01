using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MonoKle.Console
{
    public class MonoKleConsoleLogger : ILogger
    {
        private readonly string _name;
        private readonly GameConsoleLogData _logData;

        public MonoKleConsoleLogger(string name, GameConsoleLogData logData)
        {
            var startIndex = name.LastIndexOf('.') + 1;
            _name = startIndex <= 0 ? name : name[startIndex..name.Length];
            _logData = logData;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logData.LogLevel <= logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var text = $"{DateTime.Now.ToString("HH:mm:ss")} : {_name} : {formatter(state, exception)}";

            if (logLevel == LogLevel.Error || logLevel == LogLevel.Critical)
            {
                _logData.AddError(text);
            }
            else if (logLevel == LogLevel.Warning)
            {
                _logData.AddWarning(text);
            }
            else
            {
                _logData.AddLine(text);
            }
        }
    }

    public class MonoKleConsoleLoggerProvider : ILoggerProvider
    {
        private readonly GameConsoleLogData _logData;

        public MonoKleConsoleLoggerProvider(GameConsoleLogData logData)
        {
            _logData = logData;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MonoKleConsoleLogger(categoryName, _logData);
        }

        public void Dispose()
        {
        }
    }

    public static class MonoKleConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddMonoKleConsoleLogger(this ILoggingBuilder builder, GameConsoleLogData logData)
        {
            builder.Services.AddSingleton<ILoggerProvider, MonoKleConsoleLoggerProvider>(x => new MonoKleConsoleLoggerProvider(logData));
            return builder;
        }
    }
}

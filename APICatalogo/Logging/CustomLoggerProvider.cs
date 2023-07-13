using System.Collections.Concurrent;

namespace APICatalogo.Logging
{
    // Criará a instância do logger customizado criado pelo user
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfig _loggerConfig;
        readonly ConcurrentDictionary<string, CustomerLogger> _loggers =
            new ConcurrentDictionary<string, CustomerLogger>(); // armazena as infos do log
        
        public CustomLoggerProvider(CustomLoggerProviderConfig loggerConfig)
        {
            _loggerConfig = loggerConfig;
        }
                
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new CustomerLogger(categoryName, _loggerConfig));
        }

        // Libera a instância do logger
        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}

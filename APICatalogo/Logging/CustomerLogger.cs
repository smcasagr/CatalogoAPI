namespace APICatalogo.Logging
{
    // Classe que cria as instâncias do logger por categoria e provedor
    public class CustomerLogger : ILogger
    {
        readonly string _loggerName;
        readonly CustomLoggerProviderConfig _loggerConfig;

        public CustomerLogger(string loggerName, CustomLoggerProviderConfig loggerConfig)
        {
            _loggerName = loggerName;
            _loggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string msg = $"{ logLevel.ToString() }: { eventId.Id } - { formatter(state, exception)}";
            EscreverLogNoArquivo(msg);
        }

        private void EscreverLogNoArquivo(string msg)
        {
            string caminhoArquivoLog = @"E:\OneDrive\Cursos\Udemy\Web API ASP .NET Core Essencial - Macoratti\log\log.txt";
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(msg);
                    streamWriter.Close();
                }
                catch 
                {
                    throw;
                }
            }
        }
    }
}

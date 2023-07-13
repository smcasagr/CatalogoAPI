namespace APICatalogo.Logging
{
    public class CustomLoggerProviderConfig
    {
        // Informações que irão constar no log personalizado
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
    }
}

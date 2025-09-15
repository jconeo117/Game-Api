
namespace DungeonCrawlerAPI.Services
{
    public class AuctionCleanService : BackgroundService
    {

        private readonly ILogger<AuctionCleanService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AuctionCleanService(ILogger<AuctionCleanService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("El servicio de limpieza de subastas está iniciando.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("El servicio de limpieza de subastas se está ejecutando.");

                // Creamos un "scope" para poder usar nuestros servicios Scoped aquí
                using (var scope = _serviceProvider.CreateScope())
                {
                    var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();
                    await auctionService.ProcessExpiredAuctionsAsync();
                }

                // Espera 1 minuto antes de volver a ejecutarse
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("El servicio de limpieza de subastas se está deteniendo.");

        }
    }
}

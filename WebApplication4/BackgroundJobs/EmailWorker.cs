using System.Threading.Channels;

namespace WebApplication4.BackgroundJobs;

public class EmailWorker:BackgroundService
{
    private readonly Channel<string> _queue;

    private readonly ILogger<EmailWorker> _logger;

    public EmailWorker(Channel<string> queue, ILogger<EmailWorker> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EmailWorker запущен");

        await foreach (var email in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            _logger.LogInformation($"Начинаем отправку  email {email}");
            await Task.Delay(3000, stoppingToken);
            _logger.LogInformation($"Email отправелн  {email}");
        }
    }
}

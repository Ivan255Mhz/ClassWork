using System.Threading.Channels;

namespace WebApplication4.BackgroundJobs;

/// <summary>
/// Читает очередь email-сообщений и «отправляет» их (учебная имитация).
/// </summary>
public class EmailWorker : BackgroundService
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
            _logger.LogInformation("Начинаем отправку email: {Email}", email);

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);

            _logger.LogInformation("Email отправлен: {Email}", email);
        }
    }
}

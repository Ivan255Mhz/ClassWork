namespace WebApplication4.BackgroundJobs;

/// <summary>
/// Состояние фоновых задач: если обновлений не было дольше 5 минут — считаем, что задачи зависли.
/// </summary>
public class WorkerHealthState
{
    public DateTime LastHeartbeat { get; set; } = DateTime.UtcNow;

    public bool IsAlive => (DateTime.UtcNow - LastHeartbeat).TotalMinutes < 5;
}

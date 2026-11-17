namespace WebApplication4.BackgroundJobs;

public class WorkerHealthState
{
    public DateTime LastHeartbeat { get; set; } = DateTime.UtcNow;

    public bool IsAlive => (DateTime.UtcNow - LastHeartbeat).TotalMinutes < 5;
}

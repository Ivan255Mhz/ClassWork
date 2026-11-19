namespace WebApplication4.Data.Dtos;

/// <summary>
/// Базовый DTO с названием сущности.
/// </summary>
public abstract class BaseDto
{
    public virtual string Name { get; set; } = string.Empty;
}

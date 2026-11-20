using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyApi.Entities;

/// <summary>
/// Игрушка. Таблица и колонки совпадают с существующей БД проекта WebApplication4.
/// </summary>
[Table("Toys")]
public class Toy
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    // Имя колонки в БД — Prise. Не переименовываем, иначе EF её не найдёт.
    public decimal Prise { get; set; }

    public int CategoryId { get; set; }

    public int ManufactureId { get; set; }

    [MaxLength(300)]
    public string? ImageUrl { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    [ForeignKey(nameof(ManufactureId))]
    public Manufacture? Manufacture { get; set; }
}

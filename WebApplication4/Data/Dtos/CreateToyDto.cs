using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Data.Dtos;

/// <summary>
/// Данные для создания игрушки.
/// </summary>
public class CreateToyDto : BaseDto
{
    [Required(ErrorMessage = "Название обязательно")]
    [MaxLength(100, ErrorMessage = "Название не более 100 символов")]
    public override string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Цена обязательна")]
    public decimal Prise { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Id категории должен быть больше 0")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Id производителя должен быть больше 0")]
    public int ManufacturerId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Data.Dtos;

public class CreateToyDto:BaseDto
{
    [Required(ErrorMessage ="Название обезательное поле")]
    [MaxLength(100 , ErrorMessage ="Имя не более 50 символов")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Цена обезательное поле")]
    public decimal Prise { get; set; }
    [Range(1,int.MaxValue, ErrorMessage =" Id категории должен быть больше 0")]
    public int CategoryId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = " Id производителя  должен быть больше 0")]
    public int ManufacturerId { get; set; }
}

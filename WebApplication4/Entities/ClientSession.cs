using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Entities;

[Table("ClientSesions")]
public class ClientSession
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [MaxLength(64)]
    public string? IpHash { get; set; }
    [MaxLength(100)]
    public string? Country { get; set; }
    [MaxLength(100)]
    public string? Citi {  get; set; }

    public string? TimeZone { get; set; }

    public string? Language {  get; set; }

    public string? Os {  get; set; }

    public string? Browser { get; set; }

    public string? DeviceType { get; set; }

    public int? Scrnwidth { get; set; }

    public int? Scrheigth { get; set; }


 
}

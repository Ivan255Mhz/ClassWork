using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Pages;

public class ToysModel : PageModel
{
    private readonly IRepository<Toy> _repository;

    [BindProperty]
    public string NewToyName { get; set; } = string.Empty;

    [BindProperty]
    public decimal NewToyPrise { get; set; }

    public List<Toy> Toys { get; set; } = new();

    public ToysModel(IRepository<Toy> repository)
    {
        _repository = repository;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        Toys = await _repository.GetAllAsync(ct);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(NewToyName))
        {
            return Page();
        }

        var toy = new Toy
        {
            Name = NewToyName,
            Prise = NewToyPrise,
            CategoryId = 1,
            ManufactureId = 1
        };

        await _repository.AddAsync(toy, ct);
        await _repository.SaveChangesAsync(ct);

        return RedirectToPage();
    }
}

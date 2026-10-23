using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Pages
{
    public class ToysModel : PageModel
    {

        private readonly IRepository<Toy>? _repository;

        [BindProperty]
        public string NewToyName { get; set; } = string.Empty;
        [BindProperty]
        public string NewToyPrise { get; set; } = string.Empty;


        public List<Toy> Toys { get; set; } = new ();

        public ToysModel(IRepository<Toy> toys)
        {
            _repository = toys;
        }

        public async Task OnGetAsync(CancellationToken ct)
        {
            Toys = await _repository.GetAllAsync(ct);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if(!(string.IsNullOrWhiteSpace(NewToyName) && string.IsNullOrWhiteSpace(NewToyPrise)))
            {
                var temp = decimal.Parse(NewToyPrise);

                var toy = new Toy
                {
                    Id = 1,
                    Name = NewToyName,
                    Prise = temp,
                    CategoryId = 1,
                    ManufactureId = 1,
                };

                await _repository.AddAsync(toy,ct);
                await _repository.SaveChagesAsync(ct);

                return RedirectToPage();
            }

            return Page();
        }

    }
}

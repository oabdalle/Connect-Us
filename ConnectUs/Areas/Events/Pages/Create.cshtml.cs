using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConnectUs.Areas.Events.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly ApplicationDbContext _db;

        [BindProperty] //this assumes then onPost you will be getting this event here that the user modified so no need for onPost(Event eventObj)
        public Event Event { get; set; }

    
        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGet()
        {
       

        }


        public async Task<IActionResult> OnPost() //we use <IActionResult> to direct to new page
        {
            if (ModelState.IsValid)
            {
                await _db.Event.AddAsync(Event);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index", new { area = "Home" });

            }
            else
            {
                return Page();
            }

        }
    }
}

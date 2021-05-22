using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ConnectUs.Areas.Events.Pages
{

    public class CancelModel : PageModel
    {

        private readonly ILogger<CancelModel> _logger;

        public CancelModel(ILogger<CancelModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
 
         
        }
    }
}

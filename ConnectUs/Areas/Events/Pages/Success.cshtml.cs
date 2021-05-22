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

    public class SuccessModel : PageModel
    {

        private readonly ILogger<SuccessModel> _logger;

        public SuccessModel(ILogger<SuccessModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
 
         
        }
    }
}

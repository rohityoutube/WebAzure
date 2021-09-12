using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAzure.Models;

namespace WebAzure.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ITokenServce _tokenServce;
        public HomeController(ILogger<HomeController> logger, ITokenServce tokenServce )
        {
            _logger = logger;
            _tokenServce = tokenServce;
        }

        public async Task<ActionResult> Index()
        {
            var view = await _tokenServce.Get();

            IList<string> obj = new List<string>();

            obj.Add(view);

            return View(obj as IEnumerable<string>);
        }
 
    }
}

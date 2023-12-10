using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Awesome.AI.Web.Extensions;
using Awesome.AI.Web.Models.copenhagenai;
using Awesome.AI.Web.Models;

namespace Awesome.AI.Web.Pages
{
    public class DataModel : PageModel
    {
        private readonly CopenhagenaiContext _context;

        public DataModel(CopenhagenaiContext context)
        {
            _context = context;
        }

        public Section Data { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Sections != null)
            {
                IList<Section> Sections = _context.Sections
                    .OrderBy(x => x.Sort)
                    .Select(x => new Section()
                    {
                        Header = x.Header,
                        Teaser = x.Teaser,
                        Subheader = x.Subheader,
                        Text = x.Text.LineBreakToBreak().TapToSpan()
                    }).ToList();

                Data = Sections[1];
            }
        }
    }
}

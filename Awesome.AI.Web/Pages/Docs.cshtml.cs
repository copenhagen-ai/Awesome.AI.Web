using Awesome.AI.Web.copenhagenai;
using Awesome.AI.Web.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Awesome.AI.Web.Pages
{
    public class DocsModel : PageModel
    {
        private readonly CopenhagenaiContext _context;

        public DocsModel(CopenhagenaiContext context)
        {
            _context = context;
        }

        public IList<Doc> Doc { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Docs != null)
            {
                Doc = _context.Docs
                    .OrderBy(x=>x.Sort)
                    .Select(x=>new Doc() { 
                        Header = x.Header,
                        Text = x.Text.LineBreakToBreak()
                    }).ToList();
            }
        }
    }
}

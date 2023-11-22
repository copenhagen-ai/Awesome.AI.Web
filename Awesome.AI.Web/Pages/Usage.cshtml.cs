using Awesome.AI.Web.copenhagenai;
using Awesome.AI.Web.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Awesome.AI.Web.Pages
{
    public class UsageModel : PageModel
    {
        private readonly Awesome.AI.Web.copenhagenai.CopenhagenaiContext _context;

        public UsageModel(Awesome.AI.Web.copenhagenai.CopenhagenaiContext context)
        {
            _context = context;
        }

        public Section Section { get;set; } = default!;

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

                Section = Sections[0];
            }
        }
    }
}

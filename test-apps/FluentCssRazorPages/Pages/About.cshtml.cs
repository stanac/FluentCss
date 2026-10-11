using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FluentCssRazorPages.Pages;

public class AboutModel : PageModel
{
    public void OnGet()
    {
    }

    // Rendered over AJAX from the About page. It registers classes the page's
    // stylesheet link did not know about, which the observer script should pick up.
    public IActionResult OnGetLoadPartial() => Partial("_Partial");
}

﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OperationServer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        this.logger = logger;
    }

    public void OnGet()
    {
    }
}

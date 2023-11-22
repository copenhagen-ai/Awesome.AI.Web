using System;
using System.Collections.Generic;

namespace Awesome.AI.Web.copenhagenai;

public partial class Doc
{
    public int Id { get; set; }

    public string Header { get; set; } = null!;

    public string Text { get; set; } = null!;

    public int Sort { get; set; }
}

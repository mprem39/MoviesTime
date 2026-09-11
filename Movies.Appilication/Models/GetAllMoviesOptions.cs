using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Models;

public class GetAllMoviesOptions
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public Guid? UserId { get; set; }
}

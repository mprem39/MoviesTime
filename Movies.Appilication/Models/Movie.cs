using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Movies.Appilication.Models
{
    public partial class Movie
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public string Slug => GenerateSlug();
                public required int YearOfRelease { get; init; }
        public required List<string> Genres { get; init; } = new();

        private string GenerateSlug()
        {
           var slugedTitle = SlugRegex().Replace(Title, string.Empty)
                .ToLower().Replace(" ", "-");
            return $"{slugedTitle}-{YearOfRelease}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking,10)]
        private static partial Regex SlugRegex();
    }
}

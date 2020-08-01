using Newtonsoft.Json;
using System;
namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorBookDto
    {
        [JsonProperty(nameof(Id))]
        public int? Id { get; set; }
    }
}

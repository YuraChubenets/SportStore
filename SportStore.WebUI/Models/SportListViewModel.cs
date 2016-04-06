using SportStore.Domain.Entities;
using System.Collections.Generic;


namespace SportStore.WebUI.Models
{
    public class SportListViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
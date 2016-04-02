using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportStore.WebUI.Models
{
    public class SportListViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
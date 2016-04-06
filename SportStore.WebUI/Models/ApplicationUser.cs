using Microsoft.AspNet.Identity.EntityFramework;


namespace SportStore.WebUI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Year { get; set; }
        public ApplicationUser()
        {
        }
    }
}
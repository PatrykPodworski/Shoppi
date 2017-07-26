using Microsoft.AspNet.Identity.EntityFramework;

namespace Shoppi.Data.Models
{
    public class ShoppiUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
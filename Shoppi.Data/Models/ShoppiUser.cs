using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class ShoppiUser : IdentityUser
    {
        public virtual List<Address> Addresses { get; set; }

        public int? DefaultAddressId { get; set; }
        public Address DefaultAddress { get; set; }
    }
}
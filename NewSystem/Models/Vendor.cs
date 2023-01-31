using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NewSystem.Models
{
    public class Vendor
    {
		[Key]
        public int Id { get; set; }

        [Display(Name = "Vendor ID")]
		[Required]
        public string VendorId{ get; set; }

		[Display(Name = "Vendor Name")]
		[Required]
        public string VendorName { get; set; }

		[Display(Name = "Vendor Email")]
		[Required]
        public string VendorEmail{ get; set; }

		[Display(Name = "Contact")]
		[Required]
        public string VendorPhone { get; set;}

		[Display(Name = "Address")]
		[ValidateNever]
		public string? VendorAddress{ get; set; }

		[Display(Name = "City")]
		[ValidateNever]
		public string? VendorCity { get; set;}

		[Display(Name = "Country")]
		[ValidateNever]
		public string? VendorCountry { get; set;}

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Shop Name")]
		[ValidateNever]
		public string? VendorShopName{ get; set; }

		[Display(Name = "Shop Address")]
		[ValidateNever]
		public string? VendorShopAddress{ get; set; }

		[Display(Name = "City")]
		[ValidateNever]
		public string? VendorShopCity{ get; set; }

		[Display(Name = "Country")]
		[ValidateNever]
		public string? VendorShopCountry { get; set; }

        public int CreatedBy { get; set; }

        public bool IsActive{ get; set; }
    }
}

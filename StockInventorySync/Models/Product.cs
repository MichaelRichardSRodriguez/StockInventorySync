using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockInventorySync.Models
{
	public class Product
	{
		public int Product_Id { get; set; }

		[MinLength(4, ErrorMessage = "Must have a atleast 4 characters as valid name.")]
		[MaxLength(30, ErrorMessage = "Cannot exceed 30 characters.")]
		public string Name { get; set; }
		[MinLength(10, ErrorMessage = "Must have a atleast 10 characters as valid description.")]
		[MaxLength(300, ErrorMessage = "Cannot exceed 300 characters.")]
		public string Description { get; set; }

		[DisplayName("Category")]
		public int Category_Id { get; set; }

		//[ForeignKey("Category_Id")]
		[ValidateNever]
		public Category Category { get; set; }	

		[DisplayName("Date Created")]
		public DateTime? DateCreated { get; set; }
		[DisplayName("Created By")]
		public string? CreatedBy { get; set; }
		[DisplayName("Date Updated")]
		public DateTime? DateUpdated { get; set; }
		[DisplayName("Updated By")]
		public string? UpdatedBy { get; set; }

		public string? Status { get; set; }
		public DateTime? DateActivatedDeactivated { get; set; }
		public string? ActivatedDeactivatedBy { get; set; }
	}
}

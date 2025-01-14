using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static StockInventorySync.Utilities.StaticDetails;

namespace StockInventorySync.Models
{
	public class Category
	{
		public int Category_Id { get; set; }

		[MinLength(4, ErrorMessage = "Must have a atleast 4 characters as valid name.")]
		[MaxLength(30, ErrorMessage = "Cannot exceed 30 characters.")]
		public string Name { get; set; }
		[MinLength(10,ErrorMessage ="Must have a atleast 10 characters as valid description.")]
		[MaxLength(300, ErrorMessage = "Cannot exceed 300 characters.")]
		public string Description { get; set; }
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

		public IEnumerable<Product> Products { get; set; }

	}
}

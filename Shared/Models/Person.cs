using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCrud.Shared.Models
{
	public class Person
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Field {0} is required")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "{0} number is required")]
		public string? Phone { get; set; }

		[Required(ErrorMessage = "{0} is required")]
		[EmailAddress(ErrorMessage = "Invalid email, please try again")]
		public string? Email { get; set; }
	}
}


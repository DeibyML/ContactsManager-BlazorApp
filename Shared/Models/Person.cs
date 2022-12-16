using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCrud.Shared.Models
{
	public class Person
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Field {0} is required")]
		public string? Name { get; set; }
	}
}


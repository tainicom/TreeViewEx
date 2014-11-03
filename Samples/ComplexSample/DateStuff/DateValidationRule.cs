using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TreeViewEx.ComplexSample.DateStuff
{
	class DateValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			DateTime dt;
			if (DateTime.TryParse(value.ToString(), out dt)) return new ValidationResult(true, null);
			return new ValidationResult(false, "Invalid Date");
		}
	}
}

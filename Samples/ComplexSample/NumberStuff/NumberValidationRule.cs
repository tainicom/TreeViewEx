using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TreeViewEx.ComplexSample.NumberStuff
{
	class NumberValidation : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			int i;
			if (int.TryParse(value.ToString(), out i)) return new ValidationResult(true, null);
			return new ValidationResult(false, "Invalid Number");
		}
	}
}

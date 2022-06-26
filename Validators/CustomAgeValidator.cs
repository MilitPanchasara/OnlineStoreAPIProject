using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Validators
{
    public class CustomAgeValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //return base.IsValid(value); 
            var age = Convert.ToInt32(value);
            if(age < 1 || age > 125)
            {
                return false;
            }
            return true;
        }
    }
}

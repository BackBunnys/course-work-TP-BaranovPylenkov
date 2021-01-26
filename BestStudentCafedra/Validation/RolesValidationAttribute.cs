using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BestStudentCafedra.Validation
{
    public class RolesValidationAttribute : ValidationAttribute
    {
        string exceptRole;

        public RolesValidationAttribute(string exceptRole)
        {
            this.exceptRole = exceptRole;
        }
        public override bool IsValid(object value)
        {
            List<string> roles = value as List<string>;
            if (roles == null) return true;
            return !(roles.Contains(exceptRole) && roles.Count > 1);
        }
    }
}

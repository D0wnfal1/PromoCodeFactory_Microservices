using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.WebHost.Models
{
    /// <example>
    /// {
    ///    "firstName": "Otto",
    ///    "lastName": "Adams",
    ///     "email": "Otto_Addams@gmail.com",
    ///        "preferenceIds": [
    ///            "c4bda62e-fc74-4256-a956-4760b3858cbd"
    ///        ]
    /// }
    /// </example>>
    public class CreateOrEditCustomerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Guid> PreferenceIds { get; set; }
    }
}
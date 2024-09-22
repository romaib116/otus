using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class CustomerPreference
    {
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public Guid PreferenceId { get; set; }

        public virtual Preference Preference { get; set; }
    }
}
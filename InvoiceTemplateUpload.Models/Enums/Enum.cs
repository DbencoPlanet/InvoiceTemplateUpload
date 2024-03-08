using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceTemplateUpload.Models.Enums
{
    public enum PaymentStatus
    {

        [Description("Paid")]
        [Display(Name = "Paid")]
        Paid = 1,
        [Description("UnPaid")]
        [Display(Name = "UnPaid")]
        UnPaid = 2


    }
}

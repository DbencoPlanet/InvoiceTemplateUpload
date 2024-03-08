using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceTemplateUpload.Models.OperationReturns
{
    public enum OperationStatus
    {
        ConcurrencyMismatch,
        Created,
        Deleted,
        Exists,
        NotFound,
        Unknown,
        Updated,
        Failed
    }
}

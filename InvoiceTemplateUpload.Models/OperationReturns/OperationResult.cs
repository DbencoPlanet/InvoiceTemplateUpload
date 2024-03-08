using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceTemplateUpload.Models.OperationReturns
{
    public struct OperationResult
    {
        public bool Succeeded { get; set; }

        public OperationStatus Status { get; set; }

        public object ReturnObject { get; set; }

        public string Message { get; set; }
    }
}

using InvoiceTemplateUpload.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceTemplateUpload.Models.Invoicing
{
    public class Invoice
    {
        public Invoice()
        {

            this.VatRate = 0;
            this.Discount = 0;
            this.ServicePrice = 0;
            this.Quantity = 0;
            //this.InvoiceNumber = DateTime.UtcNow.Date.Year.ToString() +
            //  DateTime.UtcNow.Date.Month.ToString() +
            //  DateTime.UtcNow.Date.Day.ToString() + Guid.NewGuid().ToString().Substring(0, 4).ToUpper() + "INV";
        }
        public long Id { get; set; }

        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Display(Name = "Company Logo")]
        public string Logo { get; set; }

        [Display(Name = "Is Entertainment")]
        public bool IsEntertainment { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Account No")]
        public string AccountNo { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Invoice Template Type")]
        public string InvoiceTemplateType { get; set; }

        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Service Price")]
        public decimal? ServicePrice { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Vat Rate")]
        public decimal? VatRate { get; set; }

        [Display(Name = "Discount")]
        public decimal? Discount { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus Status { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Currency Prefix")]
        public string CurrencyPrefix { get; set; }

      

        [Display(Name = "Vat")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal? Vat
        {
            get
            {
                return (VatRate / 100) * SubTotal;
            }
        }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal? Total
        {
            get
            {
                return Quantity * ServicePrice;
            }
        }

        [Display(Name = "Sub Total")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal? SubTotal
        {
            get
            {
                return Total - DiscountValue;
            }
        }


        [Display(Name = "Discount Value")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal? DiscountValue
        {
            get
            {
                return ((Discount / 100) * Total);
            }
        }

        [Display(Name = "Grand Total")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal? GrandTotal
        {
            get
            {
                return (SubTotal + Vat);
            }
        }

        [Display(Name = "Due")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal? Due
        {
            get
            {
                return GrandTotal;
            }
        }

    }
}

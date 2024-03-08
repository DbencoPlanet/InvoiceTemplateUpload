using InvoiceTemplateUpload.Application.Invoicing.DTO;
using InvoiceTemplateUpload.Application.Invoicing.Interface;
using InvoiceTemplateUpload.Main;
using InvoiceTemplateUpload.Models.Invoicing;
using InvoiceTemplateUpload.Models.OperationReturns;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceTemplateUpload.Application.Invoicing.Concrete
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Instatiates a new object for the Category.
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="repositoryUnitOfWork"></param>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        public InvoiceAppService(
             IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public bool Any()
        {
            return _context.Invoices.Any();
        }

        public async Task<OperationResult> Delete(InvoiceDto entity)
        {
            var data = new Invoice
            {
                Id = entity.Id,
                AccountNo = entity.AccountNo,
                Address = entity.Address,
                BankName = entity.BankName,
                InvoiceTemplateType = entity.InvoiceTemplateType,
                IsEntertainment = entity.IsEntertainment,
                Logo = entity.Logo,
                Name = entity.Name,
                ServiceName = entity.ServiceName,
                ServicePrice = entity.ServicePrice,
                Discount = entity.Discount,
                Status = entity.Status,
                VatRate = entity.VatRate,
                EndDate = entity.EndDate,
                InvoiceNumber = entity.InvoiceNumber,
                StartDate = entity.StartDate,
                AccountName = entity.AccountName,
                DateCreated = entity.DateCreated,
                CurrencyPrefix = entity.CurrencyPrefix,
                Quantity = entity.Quantity
                
                

            };
            if (Any())
            {
                if (Exists(data))
                {

                    var data1 = await _context.Invoices.FindAsync(entity.Id);

                    string webRootPath = _env.WebRootPath;
                    string removeString = data.Logo;
                    var fullPath = webRootPath + removeString;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    _context.Invoices.Remove(data1);
                    await _context.SaveChangesAsync();
                    return new OperationResult()
                    {
                        Message = "Invoice deleted successfully!",
                        Status = OperationStatus.Deleted,
                        Succeeded = true,
                    };
                }
                return new OperationResult()
                {
                    Message = "The record you are seeking to delete might have been deleted by another" +
                   "user after you last retrieved the id. Re-fetch the record and try again.",
                    Status = OperationStatus.NotFound,
                    Succeeded = false
                };
            }
            return new OperationResult()
            {
                Message = "The collection does not have any element yet. Add an item into it and then " +
               "perform the operation again.",
                Status = OperationStatus.NotFound,
                Succeeded = false
            };
        }

        public bool Exists(Invoice entity)
        {
            return _context.Invoices.Find(entity.Id) == null ? false : true;
        }

        public async Task<InvoiceDto> GetById(long? id)
        {
            var data = await _context.Invoices
               .FirstOrDefaultAsync(x => x.Id == id);
            var data1 = new InvoiceDto
            {
                Id = data.Id,
                AccountNo = data.AccountNo,
                Address = data.Address,
                BankName = data.BankName,
                InvoiceTemplateType = data.InvoiceTemplateType,
                IsEntertainment = data.IsEntertainment,
                Logo = data.Logo,
                Name = data.Name,
                ServiceName = data.ServiceName,
                ServicePrice = data.ServicePrice,
                Discount = data.Discount,
                Due = data.Due,
                Status = data.Status,
                VatRate = data.VatRate,
                Vat = data.Vat,
                SubTotal = data.SubTotal,
                GrandTotal = data.GrandTotal,
                EndDate = data.EndDate,
                InvoiceNumber = data.InvoiceNumber,
                StartDate = data.StartDate,
                AccountName = data.AccountName,
                DateCreated = data.DateCreated,
                CurrencyPrefix = data.CurrencyPrefix,
                Quantity = data.Quantity,
                Total = data.Total,
                DiscountValue = data.DiscountValue
            };
            if (data1 == null)
                return null;
            else
                return data1;
        }

        public async Task<IQueryable<InvoiceDto>> GetInvoiceAsync()
        {

            try
            {
                List<InvoiceDto> data = new List<InvoiceDto>();
                IQueryable<Invoice> item = from s in _context.Invoices
                     .OrderByDescending(x=>x.Id) select s;
                data.AddRange(item.Select(invoice => new InvoiceDto()
                {
                    Id = invoice.Id,
                    AccountNo = invoice.AccountNo,
                    Address = invoice.Address,
                    BankName = invoice.BankName,
                    InvoiceTemplateType = invoice.InvoiceTemplateType,
                    IsEntertainment = invoice.IsEntertainment,
                    Logo = invoice.Logo,
                    Name = invoice.Name,
                    ServiceName = invoice.ServiceName,
                    ServicePrice = invoice.ServicePrice,
                    Discount = invoice.Discount,
                    Due = invoice.Due,
                    Status = invoice.Status,
                    VatRate = invoice.VatRate,
                    Vat = invoice.Vat,
                    SubTotal = invoice.SubTotal,
                    GrandTotal = invoice.GrandTotal,
                    EndDate = invoice.EndDate,
                    InvoiceNumber = invoice.InvoiceNumber,
                    StartDate = invoice.StartDate,
                    AccountName = invoice.AccountName,
                    DateCreated = invoice.DateCreated,
                    CurrencyPrefix = invoice.CurrencyPrefix,
                    Quantity = invoice.Quantity,
                    Total = invoice.Total,
                    DiscountValue = invoice.DiscountValue
                }));
                return data.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IQueryable<InvoiceDto>> GetInvoiceByCompanyAsync(string companyName)
        {

            try
            {
                List<InvoiceDto> data = new List<InvoiceDto>();
                IQueryable<Invoice> item = from s in _context.Invoices
                     .OrderBy(a => a.Name).Where(x => x.Name == companyName)
                                           select s;
                data.AddRange(item.Select(invoice => new InvoiceDto()
                {
                    Id = invoice.Id,
                    AccountNo = invoice.AccountNo,
                    Address = invoice.Address,
                    BankName = invoice.BankName,
                    InvoiceTemplateType = invoice.InvoiceTemplateType,
                    IsEntertainment = invoice.IsEntertainment,
                    Logo = invoice.Logo,
                    Name = invoice.Name,
                    ServiceName = invoice.ServiceName,
                    ServicePrice = invoice.ServicePrice,
                    Discount = invoice.Discount,
                    Due = invoice.Due,
                    Status = invoice.Status,
                    VatRate = invoice.VatRate,
                    Vat = invoice.Vat,
                    SubTotal = invoice.SubTotal,
                    GrandTotal = invoice.GrandTotal,
                    EndDate = invoice.EndDate,
                    InvoiceNumber = invoice.InvoiceNumber,
                    StartDate = invoice.StartDate,
                    AccountName = invoice.AccountName,
                    DateCreated = invoice.DateCreated,
                    CurrencyPrefix = invoice.CurrencyPrefix,
                    Quantity = invoice.Quantity,
                    Total = invoice.Total,
                    DiscountValue = invoice.DiscountValue

                }));
                return data.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OperationResult> Insert(InvoiceDto entity, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var results = "";

                    var webRoot = _env.WebRootPath.Trim();
                    var webUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                    var uploads = Path.Combine(webRoot, "Uploads");
                    var extension = "";
                    var filePath = "";
                    var fileName = "";
                    var uniqueFileName = "";

                    extension = Path.GetExtension(file.FileName);
                    uniqueFileName = Guid.NewGuid().ToString();

                    uniqueFileName = uniqueFileName.Replace(",", " ")
                        .Replace(".", " ").Replace("+", " ").Replace("-", " ")
                        .Replace(";", " ").Replace(":", " ").Replace("/", " ")
                        .Replace(">", " ").Replace("<", " ").Replace("?", " ")
                        .Replace("\"", " ").Replace("'", " ").Replace("|", " ")
                        .Replace("%", " ");
                    uniqueFileName = uniqueFileName.Replace(" ", "-").Replace("--", "-").Replace("-", "-");
                    uniqueFileName = uniqueFileName.Replace(" ", "-").Replace("--", "-").Replace("-", "-");
                    fileName = uniqueFileName + extension;
                    filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    results = fileName;
                    entity.Logo = webUrl + "/Uploads/" + fileName;


                }
                else
                {
                    entity.Logo = entity.Logo;
                }
                var data = new Invoice
                {
                    AccountNo = entity.AccountNo,
                    Address = entity.Address,
                    BankName = entity.BankName,
                    InvoiceTemplateType = entity.InvoiceTemplateType,
                    IsEntertainment = entity.IsEntertainment,
                    Logo = entity.Logo,
                    Name = entity.Name,
                    ServiceName = entity.ServiceName,
                    ServicePrice = entity.ServicePrice,
                    Discount = entity.Discount,
                    Status = entity.Status,
                    VatRate = entity.VatRate,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    InvoiceNumber = entity.InvoiceNumber,
                    AccountName = entity.AccountName,
                    DateCreated = entity.DateCreated,
                    CurrencyPrefix = entity.CurrencyPrefix,
                    Quantity = entity.Quantity
                };

                _context.Invoices.Add(data);
                await _context.SaveChangesAsync();
                return new OperationResult()
                {
                    Message = "Invoice Added successfully!",
                    Status = OperationStatus.Created,
                    Succeeded = true,
                    ReturnObject = new Invoice()
                    {
                        Id = entity.Id,
                        AccountNo = entity.AccountNo,
                        Address = entity.Address,
                        BankName = entity.BankName,
                        InvoiceTemplateType = entity.InvoiceTemplateType,
                        IsEntertainment = entity.IsEntertainment,
                        Logo = entity.Logo,
                        Name = entity.Name,
                        ServiceName = entity.ServiceName,
                        ServicePrice = entity.ServicePrice,
                        Discount = entity.Discount,
                        Status = entity.Status,
                        VatRate = entity.VatRate,
                        StartDate = entity.StartDate,
                        EndDate = entity.EndDate,
                        InvoiceNumber = entity.InvoiceNumber,
                        AccountName = entity.AccountName,
                        DateCreated = entity.DateCreated,
                        CurrencyPrefix = entity.CurrencyPrefix,
                        Quantity = entity.Quantity

                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    Message = "Invoice Failed to Save!",
                    Status = OperationStatus.Failed,
                    Succeeded = true,
                    ReturnObject = new Invoice()
                    {
                        Id = entity.Id,
                        AccountNo = entity.AccountNo,
                        Address = entity.Address,
                        BankName = entity.BankName,
                        InvoiceTemplateType = entity.InvoiceTemplateType,
                        IsEntertainment = entity.IsEntertainment,
                        Logo = entity.Logo,
                        Name = entity.Name,
                        ServiceName = entity.ServiceName,
                        ServicePrice = entity.ServicePrice,
                        Discount = entity.Discount,
                        Status = entity.Status,
                        VatRate = entity.VatRate,
                        StartDate = entity.StartDate,
                        EndDate = entity.EndDate,
                        InvoiceNumber = entity.InvoiceNumber,
                        AccountName = entity.AccountName,
                        DateCreated = entity.DateCreated,
                        CurrencyPrefix = entity.CurrencyPrefix,
                        Quantity = entity.Quantity

                    }
                };
            }

        }

        public async Task<OperationResult> Update(InvoiceDto entity, IFormFile file)
        {
            var data = await _context.Invoices
            .FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (Any())
            {
                if (Exists(data))
                {
                    try
                    {
                        if (file != null)
                        {
                            var results = "";

                            var webRoot = _env.WebRootPath.Trim();
                            var webUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                            var uploads = Path.Combine(webRoot, "Uploads");
                            var extension = "";
                            var filePath = "";
                            var fileName = "";
                            var uniqueFileName = "";

                            extension = Path.GetExtension(file.FileName);
                            uniqueFileName = Guid.NewGuid().ToString();

                            uniqueFileName = uniqueFileName.Replace(",", " ")
                                .Replace(".", " ").Replace("+", " ").Replace("-", " ")
                                .Replace(";", " ").Replace(":", " ").Replace("/", " ")
                                .Replace(">", " ").Replace("<", " ").Replace("?", " ")
                                .Replace("\"", " ").Replace("'", " ").Replace("|", " ")
                                .Replace("%", " ");
                            uniqueFileName = uniqueFileName.Replace(" ", "-").Replace("--", "-").Replace("-", "-");
                            uniqueFileName = uniqueFileName.Replace(" ", "-").Replace("--", "-").Replace("-", "-");
                            fileName = uniqueFileName + extension;
                            filePath = Path.Combine(uploads, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            results = fileName;
                            entity.Logo = webUrl + "/Uploads/" + fileName;


                        }
                        else
                        {
                            entity.Logo = data.Logo;
                        }

                        data.AccountNo = entity.AccountNo;
                        data.Address = entity.Address;
                        data.BankName = entity.BankName;
                        data.InvoiceTemplateType = entity.InvoiceTemplateType;
                        data.IsEntertainment = entity.IsEntertainment;
                        data.Logo = entity.Logo;
                        data.Name = entity.Name;
                        data.ServiceName = entity.ServiceName;
                        data.ServicePrice = entity.ServicePrice;
                        data.Discount = entity.Discount;
                        data.Status = entity.Status;
                        data.VatRate = entity.VatRate;
                        data.StartDate = entity.StartDate;
                        data.EndDate = entity.EndDate;
                        data.InvoiceNumber = entity.InvoiceNumber;
                        data.AccountName = entity.AccountName;
                        data.DateCreated = entity.DateCreated;
                        data.Quantity = entity.Quantity;
                        _context.Update(data);
                        await _context.SaveChangesAsync();
                        return new OperationResult()
                        {
                            Message = "Invoice Added successfully!",
                            Status = OperationStatus.Created,
                            Succeeded = true,
                            ReturnObject = new Invoice()
                            {
                                Id = entity.Id,
                                AccountNo = entity.AccountNo,
                                Address = entity.Address,
                                BankName = entity.BankName,
                                InvoiceTemplateType = entity.InvoiceTemplateType,
                                IsEntertainment = entity.IsEntertainment,
                                Logo = entity.Logo,
                                Name = entity.Name,
                                ServiceName = entity.ServiceName,
                                ServicePrice = entity.ServicePrice,
                                Discount = entity.Discount,
                                Status = entity.Status,
                                VatRate = entity.VatRate,
                                StartDate = entity.StartDate,
                                EndDate = entity.EndDate,
                                InvoiceNumber = entity.InvoiceNumber,
                                AccountName = entity.AccountName,
                                DateCreated = entity.DateCreated,
                                CurrencyPrefix = entity.CurrencyPrefix,
                                Quantity = entity.Quantity


                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        return new OperationResult()
                        {
                            Message = "Invoice Failed to Save!",
                            Status = OperationStatus.Failed,
                            Succeeded = false,
                            ReturnObject = new Invoice()
                            {
                                Id = entity.Id,
                                AccountNo = entity.AccountNo,
                                Address = entity.Address,
                                BankName = entity.BankName,
                                InvoiceTemplateType = entity.InvoiceTemplateType,
                                IsEntertainment = entity.IsEntertainment,
                                Logo = entity.Logo,
                                Name = entity.Name,
                                ServiceName = entity.ServiceName,
                                ServicePrice = entity.ServicePrice,
                                Discount = entity.Discount,
                                Status = entity.Status,
                                VatRate = entity.VatRate,
                                StartDate = entity.StartDate,
                                EndDate = entity.EndDate,
                                InvoiceNumber = entity.InvoiceNumber,
                                AccountName = entity.AccountName,
                                DateCreated = entity.DateCreated,
                                CurrencyPrefix = entity.CurrencyPrefix,
                                Quantity = entity.Quantity

                            }
                        };
                    }
                }

            }
            return new OperationResult()
            {
                Message = "Invalid invoice!",
                Status = OperationStatus.NotFound,
                Succeeded = false,
                ReturnObject = new Invoice()
                {
                    Id = entity.Id,
                    AccountNo = entity.AccountNo,
                    Address = entity.Address,
                    BankName = entity.BankName,
                    InvoiceTemplateType = entity.InvoiceTemplateType,
                    IsEntertainment = entity.IsEntertainment,
                    Logo = entity.Logo,
                    Name = entity.Name,
                    ServiceName = entity.ServiceName,
                    ServicePrice = entity.ServicePrice,
                    Discount = entity.Discount,
                    Status = entity.Status,
                    VatRate = entity.VatRate,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    InvoiceNumber = entity.InvoiceNumber,
                    AccountName = entity.AccountName,
                    DateCreated = entity.DateCreated,
                    CurrencyPrefix = entity.CurrencyPrefix,
                    Quantity = entity.Quantity

                }
            };
        }
    }
}

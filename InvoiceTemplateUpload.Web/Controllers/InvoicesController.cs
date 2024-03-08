using InvoiceTemplateUpload.Application.Invoicing.DTO;
using InvoiceTemplateUpload.Application.Invoicing.Interface;
using InvoiceTemplateUpload.Main;
using InvoiceTemplateUpload.Models.Invoicing;
using InvoiceTemplateUpload.Models.OperationReturns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using OfficeOpenXml.Drawing;
using Microsoft.AspNetCore.Hosting;

namespace InvoiceTemplateUpload.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoicesController(
            IInvoiceAppService invoiceAppService,
            ApplicationDbContext context,
            IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {

            _invoiceAppService = invoiceAppService;
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }


        [BindProperty]
        public IList<InvoiceDto> InvoiceList { get; set; }

        [BindProperty]
        public InvoiceDto Invoice { get; set; }

        public IFormFile file { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: InvoicesController
        public async Task<ActionResult> Index()
        {
            try
            {
                var query = await _invoiceAppService.GetInvoiceAsync();
                InvoiceList = query.ToList();
                return View(InvoiceList);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Import(IFormFile file)
        {
            try
            {
                var webRoot = _env.WebRootPath.Trim();
                var webUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                var path = webUrl + "/Uploads/" + "midrasolution.png";
                //var list = new List<Invoice>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows; //get Row Count
                        int colCount = worksheet.Dimension.End.Column;  //get Column Count
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var data = new Invoice
                            {
                                Name = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                                IsEntertainment = bool.Parse(worksheet.Cells[row, 2].Value?.ToString().Trim()),
                                Address = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                                AccountName = worksheet.Cells[row, 4].Value?.ToString().Trim(),
                                AccountNo = worksheet.Cells[row, 5].Value?.ToString().Trim(),
                                BankName = worksheet.Cells[row, 6].Value?.ToString().Trim(),
                                InvoiceTemplateType = worksheet.Cells[row, 7].Value?.ToString().Trim(),
                                ServiceName = worksheet.Cells[row, 8].Value?.ToString().Trim(),
                                ServicePrice = Decimal.Parse(worksheet.Cells[row, 9].Value?.ToString().Trim()),
                                CurrencyPrefix = worksheet.Cells[row, 10].Value?.ToString().Trim(),
                                VatRate = Decimal.Parse(worksheet.Cells[row, 11].Value?.ToString().Trim()),
                                Discount = Decimal.Parse(worksheet.Cells[row, 12].Value?.ToString().Trim()),
                                Quantity = int.Parse(worksheet.Cells[row, 13].Value?.ToString().Trim()),
                                StartDate = worksheet.Cells[row, 14].Value?.ToString().Trim(),
                                EndDate = worksheet.Cells[row, 15].Value?.ToString().Trim(),
                                Status = InvoiceTemplateUpload.Models.Enums.PaymentStatus.UnPaid,
                                Logo = path,
                                DateCreated = DateTime.Now
                            };

                            _context.Invoices.Add(data);
                            await _context.SaveChangesAsync();

                            var update = await _context.Invoices.FirstOrDefaultAsync(x=>x.Id == data.Id);
                            update.InvoiceNumber = update.Id.ToString("D6") + "-INV";
                            _context.Update(update);
                            await _context.SaveChangesAsync();
                           
                        }
                    }

                }
                return RedirectToAction(nameof(Index));
                //return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        // GET: InvoicesController/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            try
            {
                var query = await _invoiceAppService.GetById(id);

                Invoice = query;
                return View(Invoice);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> Detail2(long? id)
        {
            try
            {
                var query = await _invoiceAppService.GetById(id);

                Invoice = query;
                return View(Invoice);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: InvoicesController/SaveInvoice
        public async Task<ActionResult> SaveInvoice(long? id)
        {
            if (id != null)
            {
                var data = await _invoiceAppService.GetById(id);
                Invoice = data;
                return View(Invoice);
            }
            else
            {
                return View(Invoice);
            }

        }

        // POST: InvoicesController/SaveInvoice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveInvoice()
        {
            try
            {
                var getInvoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == Invoice.Id);
                if (getInvoice != null)
                {
                    OperationResult result = await _invoiceAppService.Update(Invoice, file);
                    if (result.Succeeded)
                    {
                        result.ReturnObject = result.ReturnObject;
                        result.Status = result.Status;
                        StatusMessage = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else if (!result.Succeeded)
                    {
                        StatusMessage = "Error!" + " " + result.Message;
                    }
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(Invoice);
                }
                else
                {
                    OperationResult result = await _invoiceAppService.Insert(Invoice, file);
                    if (result.Succeeded)
                    {
                        result.ReturnObject = result.ReturnObject;
                        result.Status = result.Status;
                        StatusMessage = result.Message;
                        return RedirectToAction(nameof(Index));

                    }
                    else if (!result.Succeeded)
                    {
                        StatusMessage = "Error!" + " " + result.Message;
                    }
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(Invoice);

                }

            }
            catch (Exception ex)
            {

                return View(Invoice);
            }
        }


        #region Rotativa
        /// <summary>
        /// Print Invoice details
        /// </summary>
        /// <returns></returns>
        public IActionResult PrintAllInvoice()
        {
            return new ViewAsPdf("Index");
        }

        /// <summary>
        /// Print employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> PrintDetails(long? id)
        {

            var query = await _invoiceAppService.GetById(id);
            Invoice = query;
            return new ViewAsPdf(Invoice);
        }


        /// <summary>
        /// Print employee details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> PrintDetail2(long? id)
        {
            var query = await _invoiceAppService.GetById(id);
            Invoice = query;
            return new ViewAsPdf(Invoice);
        }



        #endregion


        // GET: InvoicesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InvoicesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

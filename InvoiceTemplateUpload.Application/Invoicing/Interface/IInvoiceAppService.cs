using InvoiceTemplateUpload.Application.Invoicing.DTO;
using InvoiceTemplateUpload.Models.Invoicing;
using InvoiceTemplateUpload.Models.OperationReturns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceTemplateUpload.Application.Invoicing.Interface
{
    public interface IInvoiceAppService
    {



        /// <summary>
        /// Returns true if the Invoice presented exists.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns
        bool Exists(Invoice entity);

        /// <summary>
        /// Returns true if the tracking  collection contains any element of Category entity
        /// </summary>
        /// <returns></returns>
        bool Any();
        /// <summary>
        /// Adds an Invoice related field to the tracking entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        

        Task<OperationResult> Insert(InvoiceDto entity, IFormFile file);

        /// <summary>
        /// Returns true if the tracking  collection contains any element of Invoice entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InvoiceDto> GetById(long? id);

        /// <summary>
        /// Deletes a Invoice entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(InvoiceDto entity);

        /// <summary>
        /// Updtaes an Invoice entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Update(InvoiceDto entity, IFormFile file);

        /// <summary>
        /// Returns true if the tracking  collection contains any element of Invoice entity
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<InvoiceDto>> GetInvoiceAsync();

        /// <summary>
        ///  Returns true if the tracking  collection contains any element of Invoice by Company Name entity
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        Task<IQueryable<InvoiceDto>> GetInvoiceByCompanyAsync(string companyName);
    }
}

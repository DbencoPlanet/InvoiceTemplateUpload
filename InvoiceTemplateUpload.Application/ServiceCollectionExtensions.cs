using InvoiceTemplateUpload.Application.Invoicing.Concrete;
using InvoiceTemplateUpload.Application.Invoicing.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceTemplateUpload.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IInvoiceAppService, InvoiceAppService>();
            return services;

        }
    }
}

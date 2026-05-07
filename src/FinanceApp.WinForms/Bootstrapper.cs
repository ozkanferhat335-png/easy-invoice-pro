using FinanceApp.Application;
using FinanceApp.BankAdapters;
using FinanceApp.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;

namespace FinanceApp.WinForms
{
    public static class Bootstrapper
    {
        public static IServiceProvider Build()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            var services = new ServiceCollection();
            services.AddSingleton(Log.Logger);
            services.AddTransient<IDbConnection>(_ => new SqlConnection(ConfigurationManager.ConnectionStrings["FinanceDb"].ConnectionString));
            services.AddTransient<ITransferOrderRepository, TransferOrderRepository>();
            services.AddTransient<TransferWorkflowService>();
            services.AddSingleton(_ => new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["GarantiApiBaseUrl"]) });
            services.AddTransient<IBankAdapter>(sp => new GarantiBankAdapter(
                sp.GetRequiredService<HttpClient>(),
                ConfigurationManager.AppSettings["GarantiClientId"],
                ConfigurationManager.AppSettings["GarantiClientSecret"]));
            return services.BuildServiceProvider();
        }
    }
}

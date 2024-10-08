using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Create a builder for the web application
        var builder = WebApplication.CreateBuilder(args);

        // Register services to the container
        builder.Services.AddRazorPages();

        // Register the AzureService for DI
        builder.Services.AddSingleton<AzureService>();

        // Build the application
        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.RunAsync();


        var azureService = app.Services.GetRequiredService<AzureService>();
        await azureService.PerformAzureOperations();

        await app.WaitForShutdownAsync();
    }
}

public class AzureService
{
 
    public async Task PerformAzureOperations()
    {
        try
        {

            await AzureTableStorageFunction.StoreDataInAzureTable("CustomerProducts", "Product001", "NasiphiVelelo", "BlueJeans");
            Console.WriteLine("Stored data in Azure Table Storage.");

            await AzureQueueFunction.WriteToQueue("transactionqueue", "Transaction #001: BlueJeans Order");
            Console.WriteLine("Message added to Azure Queue.");

            string transactionMessage = await AzureQueueFunction.ReadFromQueue("transactionqueue");
            Console.WriteLine("Transaction Info: " + transactionMessage);


            await AzureBlobStorageFunction.WriteToBlobStorage("product-images", "blueJeans.jpeg", @"C:\Users\User\Pictures\blueJeans.jpeg");
            Console.WriteLine("Blob written to Azure Blob Storage.");
            
            await AzureFileFunction.WriteToFileStorage("contracts", "contract001.txt", @"C:\Users\User\Documents\contract001.txt");
            Console.WriteLine("File written to Azure File Storage.");

            Console.WriteLine("All Azure operations completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred during Azure operations: " + ex.Message);
        }
    }
}

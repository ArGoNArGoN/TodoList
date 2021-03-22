using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace TodoList
{
	public class Program
	{
		public static IList<TodoItem> TodoItems { get; set; }
		public static TasksResource Tasks { get; set; }
		public static String IdTasks { get; set; }


		// If modifying these scopes, delete your previously saved credentials
		// at ~/.credentials/tasks-dotnet-quickstart.json
		static string[] Scopes = { TasksService.Scope.Tasks };
		static string ApplicationName = "Google Tasks API .NET Quickstart";


		public static void Main(string[] args)
		{
			UserCredential credential;

			using (var stream =
				new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				// The file token.json stores the user's access and refresh tokens, and is created
				// automatically when the authorization flow completes for the first time.
				string credPath = "token.json";
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None).Result;
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Google Tasks API service.
			var service = new TasksService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			// Define parameters of request.
			TasklistsResource.ListRequest listRequest = service.Tasklists.List();
			listRequest.MaxResults = 10;

			// List task lists.
			var list = listRequest.Execute().Items;
			Console.WriteLine("Task Lists:");

			if (list != null && list.Count > 0)
			{
				var taskList = list[0];

				Console.WriteLine("{0} ({1})", taskList.Title, taskList.Id);
				
				Tasks = service.Tasks;
				IdTasks = taskList.Id;
				TodoItems = service.Tasks.List(taskList.Id).Execute().Items.Select(x => new TodoItem() { Title = x.Title, ID = x.Id }).ToList();
			}
			else
			{
				Console.WriteLine("No task lists found.");
			}


			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}

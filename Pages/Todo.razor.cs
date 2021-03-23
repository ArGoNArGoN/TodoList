using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace TodoList.Pages
{
    public class ToDoModel 
        : ComponentBase
    {
		protected override void OnInitialized()
        {
			Service = CreateService();
			Todos = Service.GetTodoItems;
        }

		private ToDoGoogleService CreateService()
		{
			UserCredential credential;

			string[] Scopes = { TasksService.Scope.Tasks };
			string ApplicationName = "Google Tasks API .NET Quickstart";

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

			return new ToDoGoogleService(service);
		}

		public ToDoGoogleService Service { get; private set; }
		public IList<TodoItem> Todos { get; set; }
		public String newTodo { get; set; } = String.Empty;
		public String IdDel { get; set; } = String.Empty;
		public String IdEd { get; set; } = String.Empty;
		public String editToDo { get; set; } = String.Empty;

		public void EditToDo(TodoItem item)
			=> Service.EditTask(item);

		public void DelToDo(TodoItem item)
			=> Service.DeleteTask(item);

		public void AddTodo()
		{
			if (!string.IsNullOrWhiteSpace(newTodo))
			{
				Service.AddTask(new TodoItem() { Title = newTodo });
				newTodo = String.Empty;
			}
		}
	}
}

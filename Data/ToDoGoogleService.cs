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
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace TodoList.Data
{
	public class ToDoGoogleService
	{
		private String id;
		public String Title { get; set; }
		private TasksService Service { get; }
		private TasksResource Tasks { get; }
		public List<TodoItem> TodoItems { get; private set; }

		public ToDoGoogleService(TasksService service)
		{
			Service = service;
			
			var Ltask = service.Tasklists.List().Execute().Items[0];

			id = Ltask.Id;
			Title = Ltask.Title;
			Tasks = Service.Tasks;
			TodoItems = GetData();
		}

		public void AddTask(Task task)
		{
			Tasks.Insert(task, id).Execute();
			TodoItems = GetData();
		}
		public void AddTask(TodoItem task)
		{
			var ntask = new Google.Apis.Tasks.v1.Data.Task() { Title = task.Title };
			this.AddTask(ntask);
		}

		public void EditTask(Task task)
		{
			Tasks.Update(task, id, task.Id).Execute();
			TodoItems = GetData();
		}
		public void EditTask(TodoItem task)
		{
			var ntask = new Google.Apis.Tasks.v1.Data.Task() { Title = task.Title, Id = task.ID };
			this.EditTask(ntask);
		}

		public void DeleteTask(Task task)
		{
			Tasks.Delete(id, task.Id).Execute();
			TodoItems = GetData();
		}
		public void DeleteTask(TodoItem task)
		{
			var ntask = new Google.Apis.Tasks.v1.Data.Task() { Title = task.Title, Id = task.ID };
			this.DeleteTask(ntask);
		}

		private List<TodoItem> GetData()
			=> Tasks.List(id).Execute().Items.Select(x => new TodoItem() { Title = x.Title, ID = x.Id }).ToList();
	}
}

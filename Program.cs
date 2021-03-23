using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace TodoList
{
	public class Program
	{
        public int MyProperty { get; private set; }

        public static void Main(string[] args)
		{
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

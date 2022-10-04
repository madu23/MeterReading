using ENSEK.Tasks.StartupTasks;

namespace ENSEK.API.Extensions
{
    public static class StartupTaskHostExtension
    {

        public static async Task RunWithTasksAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            // Load all tasks from DI
            var startupTasks = host.Services.GetServices<IStartupTask>();

            // Execute all the tasks
            foreach (var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }
            //await host.RunAsync(cancellationToken);
        }
    }
}

using Hangfire;
using Service.Implementations.ScheduleJobs;

namespace InventorSoftTestTask.Hangfire
{
    public class HangfireJobScheduler : IHangfireJobScheduler
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public HangfireJobScheduler(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }

        //background job is working every 2 minutes
        public void ScheduleRecurringJobs()
        {
            _recurringJobManager.AddOrUpdate<ReassigneTasksJob>(
                nameof(ReassigneTasksJob),
                job => job.ExecuteAsync(),
                "*/2 * * * *",
                new RecurringJobOptions());
        }
    }
}

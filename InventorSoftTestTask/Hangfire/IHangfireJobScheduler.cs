namespace InventorSoftTestTask.Hangfire
{
    public interface IHangfireJobScheduler
    {
        public void ScheduleRecurringJobs();
    }
}

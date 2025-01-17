using Prometheus;

namespace OnlineCourses.API.Metrics
{
    public static class CustomMetrics
    {
        // Определение кастомной метрики
        public static readonly Counter UserRegistrationsCounter =
            Prometheus.Metrics.CreateCounter(
                "user_registrations_total",
                "Total number of user registrations"
            );
    }
}

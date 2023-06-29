using Polly;
using Polly.Retry;

namespace Polly_eg.Policies
{
    public class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; }

        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }

        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }

        public ClientPolicy()
        {
            // executes 5 times continuously 
            ImmediateHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode ).RetryAsync(5);

            // executes 5 times with interval of 3 seconds for each request 
            LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(5,RetryAttempt => TimeSpan.FromSeconds(3));

            //executes 5 times with diff inteval for each request
            ExponentialHttpRetry= Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, RetryAttempt => TimeSpan.FromSeconds(Math.Pow(2,RetryAttempt)));



        }
    }
}

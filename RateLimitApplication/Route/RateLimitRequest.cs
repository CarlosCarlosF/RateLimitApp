using ServiceStack;
namespace RateLimitApplication
{
    
    [Route("/greet/{Name}", Verbs = "GET")]
    public class RateLimitRequest
    {
        public string Name { get; set; }
    }
}

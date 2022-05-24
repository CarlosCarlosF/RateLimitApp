using ServiceStack;
using ServiceStack.Web;
using System.Linq;
using System.Threading.Tasks;

namespace RateLimitApplication
{
    public class CustomAsyncResponseFilterAttribute : ResponseFilterAsyncAttribute
    {
        public override async Task ExecuteAsync(IRequest req, IResponse res, object responseDto)
        {
            //This code is executed after the service
            var response = (HttpResult)responseDto;          
            res.AddHeader("X-RateLimit-Limit", response.Headers.FirstOrDefault().Value);
            res.AddHeader("X-RateLimit-Remaining", response.Headers.LastOrDefault().Value);
        }
    }
}

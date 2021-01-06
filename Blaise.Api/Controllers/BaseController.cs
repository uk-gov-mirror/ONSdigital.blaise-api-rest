using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace Blaise.Api.Controllers
{
    public class BaseController : ApiController
    {
        internal StatusCodeResult NoContent()
        {
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

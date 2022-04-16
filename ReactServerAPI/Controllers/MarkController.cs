
namespace ReactServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MarkController : ControllerBase
    {
        private readonly IService<Mark, int> markService;

        public MarkController(IService<Mark, int> markService)
        {
            this.markService = markService;
        }

        [HttpGet("/getall")]
        public IActionResult Get()
        {
            var res = markService.GetAsync().Result;
            return Ok(res);
        }
        [HttpGet("/get/{id}")]
        public IActionResult Get(int id)
        {
            var res = markService.GetAsync(id).Result;
            return Ok(res);
        }

        [HttpPost("/create")]
        //[Authorize(Roles="User")]
        public IActionResult Post(string email, Mark mark)
        {
            if (ModelState.IsValid)
            {
                //Need to set ID
                //mark.UserId = HttpContext.Session.GetString("UserID");
                mark.UserId = email;
                var res = markService.CreateAsync(mark).Result;
                return Ok(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

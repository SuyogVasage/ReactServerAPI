
namespace ReactServerAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly IService<Mark, int> markService;
        private readonly UserManager<IdentityUser> userManager;
        public MarkController(IService<Mark, int> markService, UserManager<IdentityUser> userManager)
        {
            this.markService = markService;
            this.userManager = userManager;
        }

        [HttpGet("{EmailID}")]
        //[ActionName("getmarks")]
        public async Task<IActionResult> Get(string EmailID)
        {
            var user = await userManager.FindByEmailAsync(EmailID);
            var res = markService.GetAsync().Result.Where(e => e.UserId == user.Id).ToList();
            var clientOutput = from r in res
                               select new
                               {
                                   MathsScore = r.Mathematics,
                                   ScienceScore = r.Science,
                                   HistoryScore = r.History,
                                   GeograpgyScore = r.Geography,
                                   Date = r.Date,
                               };
            return Ok(clientOutput);
        }

        [HttpPost("create")]
        //[ActionName("postmarks")]
        public async Task<IActionResult> Post(StudentMarks mark)
        {
            if (ModelState.IsValid)
            {
                var res = await userManager.FindByEmailAsync(mark.MailID);
                var student = new Mark()
                {
                    UserId = res.Id,
                    Mathematics = mark.MathsScore,
                    Science = mark.SciScore,
                    History = mark.HistScore,
                    Geography = mark.GeoScore,
                    Date = DateTime.Now
                };

                var result = markService.CreateAsync(student);
                return Ok(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

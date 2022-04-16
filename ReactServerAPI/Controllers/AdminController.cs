
namespace ReactServerAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public AdminController(AuthService _authService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this._authService = _authService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost("/register")]
        [ActionName("create")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RegisterNewUserAsync(registerUser);

                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("/login")]
        [ActionName("login")]
        public async Task<IActionResult> Login(LoginUser inputModel)
        {
            if (ModelState.IsValid)
            {
                var msg = await _authService.AuthenticateUserAsync1(inputModel);
                //HttpContext.Session.SetString(msg.Id, "UserID");
                if (msg.Id == null)
                {
                    return Unauthorized("The Authentication Failed");
                }
                var ResponseData = new ResponseData()
                {
                    Message = "Login Succesfull"
                };

                return Ok(ResponseData);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("/masterlogin")]
        [ActionName("masterlogin")]
        public async Task<IActionResult> MasterLogin(LoginUser inputModel)
        {
            if (ModelState.IsValid)
            {
                var msg = await _authService.AuthenticateUserAsync(inputModel);
                //HttpContext.Session.SetString(msg.Id, "UserID");
                if (msg == null)
                {
                    return Unauthorized("The Authentication Failed");
                }
                var ResponseData = new ResponseData()
                {
                    Message = msg
                };

                return Ok(ResponseData);
            }
            return BadRequest(ModelState);
        }
    }
}

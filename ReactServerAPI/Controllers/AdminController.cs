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

        [HttpPost]
        [ActionName("create")]
        public async Task<IActionResult> Create(RegisterUser registerUser)
        {
            if(registerUser.Email == null || registerUser.UserName == null || registerUser.Password == null)
            {
                return BadRequest("Data shi se enter kro bhai");
            }
            if (ModelState.IsValid)
            {
                var response = await _authService.RegisterNewUserAsync(registerUser);

                return Ok(response.Message);
            }
            return BadRequest(ModelState);
        }


        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login(LoginUser inputModel)
        {
            if(inputModel.Email == null || inputModel.Password == null)
            {
                return BadRequest("Data shi se enter kro bhai");
            }
            if (ModelState.IsValid)
            {
                var msg = await _authService.AuthenticateUserAsync(inputModel);
                if (msg.Message == null)
                {
                    return Unauthorized("The Authentication Failed");
                }

                return Ok(msg);
            }
            return BadRequest(ModelState);
        }

    }
}

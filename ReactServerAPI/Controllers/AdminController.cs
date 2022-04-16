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
            if (ModelState.IsValid)
            {
                var msg = await _authService.AuthenticateUserAsync(inputModel);
                if (msg == null)
                {
                    return Unauthorized("The Authentication Failed");
                }
                var ResponseData = new ResponseData()
                {
                    Message = "Login Succesfull",
                };

                return Ok(ResponseData);
            }
            return BadRequest(ModelState);
        }

    }
}

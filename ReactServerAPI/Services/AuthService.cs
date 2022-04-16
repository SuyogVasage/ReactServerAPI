
namespace ReactServerAPI.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        // used to read settings from appsettings.json
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._config = config;
            this.roleManager = roleManager;
        }

        public async Task<ResponseData> RegisterNewUserAsync(RegisterUser user)
        {
            var response = new ResponseData();
            // Store the received data in IdentityUser class
            var registerUser = new IdentityUser() { UserName = user.Email, Email = user.Email };
            // Create User
            var result = await _userManager.CreateAsync(registerUser, user.Password);
            // If Successful returnSuccess
            if (result.Succeeded)
            {
                var student = await _userManager.FindByEmailAsync(registerUser.Email);
                //string student = registerUser.Email;
                var role = new IdentityRole() { Name = "User" };
                //var result1 = await roleManager.CreateAsync(role);
                var addRole = await _userManager.AddToRoleAsync(student, role.Name);

                //Logic for mail

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("suyog.vasage@coditas.com");
                mail.To.Add(user.Email);
                mail.Subject = "User Credentials";
                mail.Body = $"Email:- {user.Email}\n" +
                    $"Password:-{user.Password}";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("suyog.vasage@coditas.com", "Hitman45$");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                response.Message = $"User {user.Email} is registered Successfully";

            }
            else
            {
                // else return fail
                response.Message = $"User {user.Email} Registration Failed";
            }
            return response;
        }

        public async Task<string> AuthenticateUserAsync(LoginUser inputModel)
        {
            string jwtToken = "";
            // SIgnin the user
            var result = await _signInManager.PasswordSignInAsync(inputModel.Email, inputModel.Password, false, lockoutOnFailure: true);
            var student = await _userManager.FindByEmailAsync(inputModel.Email);

            if (result.Succeeded)
            {
                // Read Secret Key
                var secretKey = Convert.FromBase64String(_config["JWTSettings:SecretKey"]);
                // Read Expiry
                var expiryTimeSpan = Convert.ToInt32(_config["JWTSettings:ExpiryInMinuts"]);

                // CReate an IdenttyUSer object
                // this will be used fot creating Claim
                IdentityUser user = new IdentityUser(inputModel.Email);

                // SecurityTokenDescriptor: Define the Information for
                // Generating Token
                var securityTokenDescription = new SecurityTokenDescriptor()
                {
                    Issuer = null,
                    Audience = null,
                    // SUrrently Only USer NAme is addded in Claim
                    Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("username",user.Id,ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(expiryTimeSpan),
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    // Setting an Algorithm for Encryption
                    // and The Signeture
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };
                // Actually GeneratenJSON Web Token
                var jwtHandler = new JwtSecurityTokenHandler();
                // CReate Token based on Description
                var jwToken = jwtHandler.CreateJwtSecurityToken(securityTokenDescription);
                // Write Token in Response
                jwtToken = jwtHandler.WriteToken(jwToken);
            }
            else
            {
                jwtToken = "Login failed";
            }

            return jwtToken;

        }
    }
}



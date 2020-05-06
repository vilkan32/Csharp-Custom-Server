using SharedTrip.Services;
using SharedTrip.ViewModels;
using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService usersService;
        private ILogger logger;

        public UsersController(IUsersService usersService, ILogger logger)
        {
            this.usersService = usersService;
            this.logger = logger;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            var userId = this.usersService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userId);
            this.logger.Log("User logged in: " + username);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {

            Console.WriteLine();
            if (input.Password != input.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            if (input.Username?.Length < 5 || input.Username?.Length > 20)
            {
                return this.Redirect("/Users/Register");
            }

            if (input.Password?.Length < 6 || input.Password?.Length > 20)
            {
                return this.Redirect("/Users/Register");
            }

            if (!IsValid(input.Email))
            {
                return this.Redirect("/Users/Register");
            }

            if (this.usersService.IsUsernameUsed(input.Username))
            {
                return this.Redirect("/Users/Register");
            }

            if (this.usersService.IsEmailUsed(input.Email))
            {
                return this.Redirect("/Users/Register");
            }

            this.usersService.CreateUser(input.Username, input.Email, input.Password);
            this.logger.Log("New user: " + input.Username);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.SignOut();
            return this.Redirect("/");
        }

        private bool IsValid(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

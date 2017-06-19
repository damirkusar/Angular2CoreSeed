﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using WebApp.DataAccessLayer.Models;

namespace WebApp.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly Logger logger;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [Authorize(ActiveAuthenticationSchemes = "Cookie")]
        [HttpGet]
        [Route("UserInfo")]
        public IActionResult GetUserInfo()
        {
            try
            {
                this.logger.Trace($"GetUserInfo called");
                var userInfo = this.CreateUserInfo();
                return this.Ok(userInfo);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception, $"Error in GetUserInfo");
                return this.BadRequest(exception);
            }
        }

        private ApplicationUser CreateUserInfo()
        {
            try
            {
                this.logger.Trace($"CreateUserInfo called");
                var user = this.userManager.GetUserAsync(this.User).Result;
                user.AssignedRoles = this.userManager.GetRolesAsync(user).Result;

                return user;
            }
            catch (Exception exception)
            {
                this.logger.Error(exception, $"Error in CreateUserInfo");
                throw;
            }
        }
    }
}
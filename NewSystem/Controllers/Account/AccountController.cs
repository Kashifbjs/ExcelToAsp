using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NewSystem.Data;
using NewSystem.Models.Account;
using NewSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace NewSystem.Controllers.Account
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var CheckUserName = _context.Users.Where(e => e.UserName == model.UserName).SingleOrDefault();
                if (CheckUserName != null)
                {
                    if (CheckUserName.IsActive == false)
                    {
                        TempData["ErrorMsg"] = "User not active, Kindly contact to admin!";
                        return View(model);
                    }
                    bool isValid = (CheckUserName.UserName == model.UserName && CheckUserName.UserPassword == model.UserPassword);
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity();
                        if (CheckUserName.RoleId == 2)
                        {
                            int UserRole = CheckUserName.RoleId;
                            identity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, model.UserName), new Claim(ClaimTypes.Role, "2") },
                                 CookieAuthenticationDefaults.AuthenticationScheme);
                        }
                        else
                        {
                            int UserRole = CheckUserName.RoleId;
                            identity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, model.UserName), new Claim(ClaimTypes.Role, "1") },
                                 CookieAuthenticationDefaults.AuthenticationScheme);
                        }
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("UserName", model.UserName);
                        if (CheckUserName.RoleId == 1)
                        {
                            HttpContext.Session.SetString("Role", "1");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Role", "2");
                            return RedirectToAction("Index", "Home2");
                        }
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Invalid username or password";
                        return View(model);
                    }
                }
                return RedirectToAction("Signup");
            }
            else
            {
                TempData["ErrorMsg"] = "Username not found";
                return View(model);
            }

        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var StoredCookies = Request.Cookies.Keys;
            foreach (var Cookies in StoredCookies)
            {
                Response.Cookies.Delete(Cookies);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserPassword != model.ConfirmPassword)
                {
                    TempData["ErrorMsg"] = "Both passwords are not same!";
                    return View(model);
                }
                var CheckUserName = _context.Users.Where(e => e.UserName == model.UserName).SingleOrDefault();
                if (CheckUserName != null)
                {
                    TempData["ErrorMsg"] = "Username already exist!";
                    return View(model);
                }
                var CheckUserEmail = _context.Users.Where(e => e.UserEmail == model.UserEmail).SingleOrDefault();
                if (CheckUserEmail != null)
                {
                    TempData["ErrorMsg"] = "Email already exist!";
                    return View(model);
                }
                var CheckNumber = _context.Users.Where(e => e.UserPhone == model.UserPhone).SingleOrDefault();
                if (CheckNumber != null)
                {
                    TempData["ErrorMsg"] = "Mobile number already exist!";
                    return View(model);
                }
                var data = new User
                {
                    UserName = model.UserName,
                    UserEmail = model.UserEmail,
                    UserPassword = model.UserPassword,
                    UserPhone = model.UserPhone,
                    RoleId = model.RoleId,
                    IsActive = model.IsActive
                };
				var getLastNumber = _context.Vendors.OrderByDescending(p => p.VendorId).FirstOrDefault();
				int NewVendorId = Convert.ToInt32(getLastNumber.VendorId.Replace("V", "")) + 1;
				var NewVendorId2 = "V" + NewVendorId.ToString().PadLeft(4, '0');
                var data2 = new Vendor
                {
                    VendorId = NewVendorId2,
                    VendorName = model.UserName,
                    VendorEmail = model.UserEmail,
                    VendorPhone = model.UserPhone,
                    VendorShopName = model.UserShopName,
                    CreatedBy = 0,
                    IsActive = false
                };
                _context.Users.Add(data);
				_context.Vendors.Add(data2);
				_context.SaveChanges();
                TempData["SuccessMsg"] = "User registered successfully!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["SuccessMsg"] = "User not registered!";
                return View(model);
            }
        }

		[HttpGet]
		public IActionResult ChangePassword()
		{
			var CheckUserName = _context.Users.Where(e => e.UserName == User.Identity.Name).SingleOrDefault();
			ViewBag.Userid = CheckUserName.UserId;
            ViewBag.UserName = CheckUserName.UserName;
            ViewBag.UserEmail = CheckUserName.UserEmail;
			return View();
		}
		[HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePassword changePassword)
        {
			if (changePassword == null)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
            {
                var CheckUserName = _context.Users.Where(e => e.UserName == User.Identity.Name).SingleOrDefault();
                if (CheckUserName.UserPassword != changePassword.OldPassword)
                {
                    TempData["ErrorMsg"] = "Old Password not matched!";
                    return View(changePassword);
                }

				if (changePassword.OldPassword == changePassword.NewPassword)
				{
					TempData["ErrorMsg"] = "Old and New password cannot be same!";
					return View(changePassword);
				}

				if (changePassword.NewPassword != changePassword.ConfirmPassword)
                {
                    TempData["ErrorMsg"] = "Both passwords are not same!";
                    return View(changePassword);
                }

                CheckUserName.UserPassword = changePassword.NewPassword;
                CheckUserName.DateUpdated = DateTime.Now;
                _context.Users.Update(CheckUserName);
				_context.SaveChanges();
                if(CheckUserName.RoleId == 1)
                {
					return RedirectToAction("Index", "Home");
				}
                else if(CheckUserName.RoleId == 2)
                {
                    return RedirectToAction("Index", "Home2");
                }
			}
            return View(changePassword);
        }
    }
}

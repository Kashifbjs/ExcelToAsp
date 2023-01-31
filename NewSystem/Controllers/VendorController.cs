using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using NewSystem.Data;
using NewSystem.Models;
using Microsoft.EntityFrameworkCore;
using NewSystem.Models.Account;

namespace NewSystem.Controllers
{
    [Authorize(Roles = "1")]
    public class VendorController : Controller
	{

		private ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
            IEnumerable<Vendor> vendors = _context.Vendors.Where(e => e.CreatedBy != 0);
            return View(vendors);
        }


        [HttpGet]
        public IActionResult Create()
        {
			var getLastNumber = _context.Vendors.OrderByDescending(p => p.VendorId).FirstOrDefault();
			int NewVendorId = Convert.ToInt32(getLastNumber.VendorId.Replace("V", "")) + 1;
			var NewVendorId2 = "V" + NewVendorId.ToString().PadLeft(4, '0');
			ViewBag.NewVendorId = NewVendorId2;
			return View();
        }

        [HttpPost]
		public IActionResult Create(Vendor vendor)
        {
			if (ModelState.IsValid)
			{
				var CheckUserName = _context.Vendors.Where(e => e.VendorId == vendor.VendorId).SingleOrDefault();
				if (CheckUserName != null)
				{
					TempData["ErrorMsg"] = "Vendor ID already exist!";
					return View(vendor);
				}
				var CheckUserEmail = _context.Vendors.Where(e => e.VendorEmail == vendor.VendorEmail).SingleOrDefault();
				if (CheckUserEmail != null)
				{
					TempData["ErrorMsg"] = "Email already exist!";
					return View(vendor);
				}
				var CheckNumber = _context.Vendors.Where(e => e.VendorPhone == vendor.VendorPhone).SingleOrDefault();
				if (CheckNumber != null)
				{
					TempData["ErrorMsg"] = "Contact number already exist!";
					return View(vendor);
				}
				var UserId = _context.Users.Where(e => e.UserName == User.Identity.Name).SingleOrDefault();
				var data = new Vendor
				{
					VendorId = vendor.VendorId,
					VendorName = vendor.VendorName,
					VendorEmail = vendor.VendorEmail,
					VendorPhone = vendor.VendorPhone,
					VendorAddress = vendor.VendorAddress,
					VendorCity = vendor.VendorCity,
					VendorCountry = vendor.VendorCountry,
					CreatedDate = vendor.CreatedDate,
					UpdatedDate = vendor.UpdatedDate,
					VendorShopName = vendor.VendorShopName,
					VendorShopAddress = vendor.VendorShopAddress,
					VendorShopCity = vendor.VendorShopCity,
					VendorShopCountry = vendor.VendorShopCountry,
					CreatedBy = UserId.UserId,
					IsActive = vendor.IsActive
				};
				_context.Vendors.Add(data);
				_context.SaveChanges();
				TempData["SuccessMsg"] = "Vendor created successfully!";
                var data2 = new User
                {
                    UserName = vendor.VendorName,
                    UserEmail = vendor.VendorEmail,
                    UserPassword = "12345",
                    UserPhone = vendor.VendorPhone,
                    RoleId = 2,
                    IsActive = true
                };
                _context.Users.Add(data2);
                _context.SaveChanges();
                return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var vendor = _context.Vendors.Find(id);
			if (vendor == null)
			{
				return NotFound();
			}
			return View(vendor);
		}

		[HttpPost]
		public IActionResult Edit(Vendor vendor)
		{
			if (ModelState.IsValid)
			{
				_context.Vendors.Update(vendor);
				var CheckVendor = _context.Users.Where(e => e.UserName == vendor.VendorName).SingleOrDefault();
				if (CheckVendor != null)
				{
					CheckVendor.DateUpdated = DateTime.Now;
					CheckVendor.UserPhone = vendor.VendorPhone;
					CheckVendor.IsActive = vendor.IsActive;
					_context.Users.Update(CheckVendor);
				}
				TempData["SuccessMsg"] = "Vendor updated successfully!";
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}

        [HttpGet, ActionName("Delete")]
        public IActionResult Index(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var vendor = _context.Vendors.Find(id);
			
			if (vendor == null)
			{
				return NotFound();
			}
            var CheckVendor = _context.Users.Where(e => e.UserName == vendor.VendorName).SingleOrDefault();
            vendor.UpdatedDate = DateTime.Now;
            CheckVendor.DateUpdated = DateTime.Now;
			if(vendor.IsActive == true)
			{
				vendor.IsActive = false;
                CheckVendor.IsActive = false;
                TempData["SuccessMsg"] = "Vendor disabled successfully!";
            }
			else
			{
                vendor.IsActive = true;
                CheckVendor.IsActive = true;
                TempData["SuccessMsg"] = "Vendor enabled successfully!";
            }
            _context.Vendors.Update(vendor);
            _context.Users.Update(CheckVendor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Requests()
        {
            IEnumerable<Vendor> vendors = _context.Vendors.Where(e => e.CreatedBy == 0 && e.IsActive == false);
            return View(vendors);
        }

        public IActionResult ViewVendor(int? id)
        {
            var vendor = _context.Vendors.Where(e => e.Id == id).SingleOrDefault();
			ViewBag.VendorName = vendor?.VendorName;
            IEnumerable<Vendor> vendors = _context.Vendors.Where(e => e.Id == id);
            return View(vendors);
        }

        [HttpGet]
        public IActionResult Approve(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var vendor = _context.Vendors.Find(id);

            if (vendor == null)
            {
                return NotFound();
            }
            var user = _context.Users.Where(e => e.UserName == vendor.VendorName).SingleOrDefault();
            vendor.UpdatedDate = DateTime.Now;
			var AdminUserId = _context.Users.Where(e => e.UserName == User.Identity.Name).SingleOrDefault();
			vendor.CreatedBy = AdminUserId.UserId;
			vendor.IsActive = true;
            user.DateUpdated = DateTime.Now;
			user.IsActive = true;
            _context.Vendors.Update(vendor);
            _context.Users.Update(user);
            _context.SaveChanges();
			TempData["SuccessMsg"] = "Vendor approved successfully";
            return RedirectToAction("Index");
        }

    }
}

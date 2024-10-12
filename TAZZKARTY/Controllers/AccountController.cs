using Azure.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TAZZKARTY.Models;
using TAZZKARTY.Requests;
using TAZZKARTY.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using TAZZKARTY.Migrations;
namespace TAZZKARTY.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public AccountController(IWebHostEnvironment environment)
        {
            this.environment=environment;
        }
        AppDbContext _Db = new AppDbContext() ;
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AdminInputs request)
        {
            if (ModelState.IsValid is false)
            {
                return View(request);
            }

            var foundEmployee1 = _Db.Users.FirstOrDefault(x => x.Phone == request.Phone);
            if (foundEmployee1 != null)
            {
                ModelState.AddModelError("Phone", "There is already an employee with the same phone number");
                return View(request);
            }
            var foundEmployee2 = _Db.Users.FirstOrDefault(x => x.Email == request.Email);
            if (foundEmployee2 != null)
            {
                ModelState.AddModelError("Email", "There is already an employee with the same Email");
                return View(request);
            }
            string Newfilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Newfilename += Path.GetExtension(request.Image!.FileName);
            string imagefullPath = environment.WebRootPath + "/assets/img/" + Newfilename;



            using (var stream = System.IO.File.Create(imagefullPath))
            {
                request.Image.CopyTo(stream);
            }
            var user = new User
            {
                NationalId = request.NationalId,
                Phone = request.Phone,
                FullName = request.FullName,
                Password = request.Password,
                Role = Role.User ,
                Email = request.Email,
                Image = Newfilename,

            };

            _Db.Users.Add(user);
            _Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterAdmin(AdminInputs request)
        {
            if (ModelState.IsValid is false)
            {
                return View(request);
            }

            var foundEmployee1 = _Db.Users.FirstOrDefault(x => x.Phone == request.Phone);
            if (foundEmployee1 != null)
            {
                ModelState.AddModelError("Phone", "There is already an employee with the same phone number");
                return View(request);
            }
            var foundEmployee2 = _Db.Users.FirstOrDefault(x => x.Email == request.Email);
            if (foundEmployee2 != null)
            {
                ModelState.AddModelError("Email", "There is already an employee with the same Email");
                return View(request);
            }
            string Newfilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Newfilename += Path.GetExtension(request.Image!.FileName);
            string imagefullPath = environment.WebRootPath + "/assets/img/" + Newfilename;



            using (var stream = System.IO.File.Create(imagefullPath))
            {
                request.Image.CopyTo(stream);
            }
            var user = new User
            {
                NationalId = request.NationalId,
                Phone = request.Phone,
                FullName = request.FullName,
                Password = request.Password,
                Role = Role.Admin,
                Email = request.Email,
                Image = Newfilename,

            };

            _Db.Users.Add(user);
            _Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputs Reqest)
        {

            if (ModelState.IsValid is false)
            {
                return View(Reqest);
            }
            var user = _Db.Users.FirstOrDefault(x => x.Password == Reqest.Password && x.Email == Reqest.Email && x.Role == Reqest.Role);

            if (user is null)
            {
                ModelState.AddModelError("", "Email or Password  or role is Wrong. Please Try Again!");
                return View(Reqest);
            }

            List<Claim> claims = [
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone,user.Phone),
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.Email,user.Email),

                ];

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var princiable = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princiable);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Get the current logged-in user's ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(); // If the user is not authenticated, redirect or show an error
            }

            int userId = int.Parse(userIdClaim);

            // Find the user and include the purchased matches
            var user = await _Db.Users
                .Include(u => u.Matches) // Load the matches the user has purchased
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user); // Pass the user and their matches to the view
        }
        [Authorize(Roles = $"{nameof(Role.User)}")]

        public async Task<IActionResult> Buy(int matchId)
        {
            // Get the current logged-in user's ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim);

            // Find the user and the match
            var user = await _Db.Users
                .Include(u => u.Matches)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var match = await _Db.Matches.FindAsync(matchId);

            if (user == null || match == null)
            {
                return NotFound();
            }

            // Add the match to the user's collection of matches
            if (!user.Matches.Contains(match))
            {
                user.Matches.Add(match);
                await _Db.SaveChangesAsync();
            }

            return RedirectToAction("Profile");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


    }



}


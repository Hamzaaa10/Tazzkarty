using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using TAZZKARTY.Models;
using TAZZKARTY.Requests;
using TAZZKARTY.Services;
namespace TAZZKARTY.Controllers
{
    public class MatchController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public MatchController(IWebHostEnvironment environment)
        {
            this.environment=environment;
        }

        AppDbContext _Db = new AppDbContext();
        public IActionResult Index()
        {
            var matchs = _Db.Matches.ToList();
            return View(matchs);
        }
      

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MatchRequest input)
        {
            if (input.Image == null)
            {
                ModelState.AddModelError("Image", "the image file is required");
                // return View();
            }
            if (ModelState.IsValid == false)
            {
                return View(input);
            }

            string Newfilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Newfilename += Path.GetExtension(input.Image!.FileName);
            string imagefullPath = environment.WebRootPath + "/assets/img/" + Newfilename;



            using (var stream = System.IO.File.Create(imagefullPath))
            {
                input.Image.CopyTo(stream);
            }
            var match = new Match
            {
                NameS = input.NameS,
                Namev=input.Namev,
                Location=input.Location,
                MatchTime = input.MatchTime,
                Tournament = input.Tournament,
                Image=Newfilename,
                Price=input.Price,

            };
            _Db.Matches.Add(match);
            _Db.SaveChanges();
            return RedirectToAction("Index");

        }
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public IActionResult Delete(int id)
        {
            var match = _Db.Matches.FirstOrDefault(p => p.Id == id);
            _Db.Matches.Remove(match);
            _Db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost] 
        public async Task<IActionResult> Buy(int matchId)
        {
            // Get the current user's identity (e.g., email or username)
            var userId = User.Identity.Name;

            // Find the user by their email or username
            var user = await _Db.Users
                .Include(u => u.Matches)  // Include the Matches collection
                .FirstOrDefaultAsync(u => u.Email == userId);  // Adjust if using something else as the identifier

            if (user == null)
            {
                // Handle user not found
                return NotFound("User not found.");
            }

            // Find the match by its ID
            var match = await _Db.Matches.FindAsync(matchId);

            if (match == null)
            {
                // Handle match not found
                return NotFound("Match not found.");
            }

            // Add the match to the user's collection of matches
            user.Matches.Add(match);

            // Save changes to the database
            await _Db.SaveChangesAsync();

            // Redirect to a confirmation or profile page
            return RedirectToAction("index");
        }
    }


    /* public IActionResult Buy(int matchId)
     {

         var employeeId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)!.Value;
         var userId = int.Parse(employeeId);

         var user = _Db.Users.FirstOrDefault(u => u.Id == userId)  ;// Assuming Email as the identifier

         if (user == null)
         {
             // Handle user not found
             return NotFound("User not found.");
         }

         // Find the match
         var match =  _Db.Matches.FirstOrDefault(m => m.Id == matchId);

         if (match == null)
         {
             // Handle match not found
             return NotFound("Match not found.");
         }

         // Add the match to the user's matches
         user.Matches.Add(match);

         // Save changes to the database
          _Db.SaveChangesAsync();


        var matchss = _Db.Users.Include(x => x.Matches).Where(x => x.Id == userId).ToList();

         return View(matchss);
     }*/


}



    


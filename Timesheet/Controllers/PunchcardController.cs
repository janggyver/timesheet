using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timesheet.Data;
using Timesheet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Timesheet.Controllers
{

    public class PunchcardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        


        public PunchcardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        //public IList<Punchcard> Punchcards { get; set; }

        // GET: Punchcard
        [Authorize]
        public ActionResult _Index()
        {
            var PunchcardLists = from ts in _context.Punchcards
                                 select ts;
            var userId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.FindByIdAsync(userId.ToString()).Result;
            ViewBag.userEmail = user.Email;
            PunchcardLists = PunchcardLists.Where(e => e.UserId == userId).OrderBy(p => p.WorkDate);
            return View(PunchcardLists);
        }

        [Authorize]
        public async Task<IActionResult> Index(string actionString, string searchString1, string searchString2)
        {
            
            var userId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
            ApplicationUser user = _userManager.FindByIdAsync(userId.ToString()).Result;
            ViewBag.userEmail = user.Email;

            var punchcardLists = from pc in _context.Punchcards
                                 select pc;
            punchcardLists =  punchcardLists.Where(e => e.UserId.Equals(userId)).OrderBy(p => p.WorkDate);

            if(ModelState.IsValid && !String.IsNullOrEmpty(actionString))
            {
                switch (actionString)
                {
                    case "ExactDate":
                        if (!string.IsNullOrEmpty(searchString1))
                        {
                            if (DateTime.Parse(searchString1).GetType() == typeof(DateTime))
                            {
                                punchcardLists = punchcardLists
                                    .Where(p => p.WorkDate.Equals(DateTime.Parse(searchString1))).OrderBy(p => p.WorkDate);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Data Type should be \"DateTime\"";
                            }
                        }
                        return View(punchcardLists);
                    case "StartDate":
                        if (!string.IsNullOrEmpty(searchString1))
                        {
                            if (DateTime.Parse(searchString1).GetType() == typeof(DateTime))
                            {
                                punchcardLists = punchcardLists
                                    .Where(p => p.WorkDate >= DateTime.Parse(searchString1)).OrderBy(p => p.WorkDate);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Data Type should be \"DateTime\"";
                            }
                            return View(punchcardLists);
                        }
                        break;

                    case "EndDate":
                        if (!string.IsNullOrEmpty(searchString1))
                        {
                            if (DateTime.Parse(searchString1).GetType() == typeof(DateTime))
                            {
                                punchcardLists = punchcardLists
                                    .Where(p => p.WorkDate <= DateTime.Parse(searchString1)).OrderBy(p => p.WorkDate);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Data Type should be \"DateTime\"";
                            }
                        }
                        return View(punchcardLists);
                    case "RangeDates":
                        if(!string.IsNullOrEmpty(searchString1) && !string.IsNullOrEmpty(searchString2))
                        {
                            
                            if((DateTime.Parse(searchString1).GetType() == typeof(DateTime)) && 
                                (DateTime.Parse(searchString2).GetType() == typeof(DateTime)))
                            {
                                var firstDate = DateTime.Parse(searchString1);
                                var secondDate = DateTime.Parse(searchString2);
                                if(firstDate <= secondDate)
                                {
                                    punchcardLists = punchcardLists.
                                    Where(e => e.WorkDate >= firstDate && e.WorkDate <= secondDate).OrderBy(p => p.WorkDate);
                                }
                                else
                                {
                                    punchcardLists = punchcardLists.
                                    Where(e => e.WorkDate >= secondDate && e.WorkDate <= firstDate).OrderBy(p => p.WorkDate);
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Data Type should be \"DateTime\"";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Please choose two Dates.";
                        }
                        return View(punchcardLists);

                    case "CertainWeekDate":
                        if (!string.IsNullOrEmpty(searchString1) && string.IsNullOrEmpty(searchString2)) 
                        {
                            if (DateTime.Parse(searchString1).GetType() == typeof(DateTime)) 
                            {
                                var inputDate = DateTime.Parse(searchString1).Date;
                                var dateTosearch = HelperMethods.TimeHelper.FindStartDayOfWeek(inputDate);
                                punchcardLists = punchcardLists.
                                    Where(e => e.WorkDate >= dateTosearch && e.WorkDate <= dateTosearch.AddDays(6))
                                    .OrderBy(p => p.WorkDate);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Data Type should be \"DateTime\"";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Please input in a first textbox with a chosend date.";
                        }
                        return View(punchcardLists);

                    case "CurrentWeek":
                        var dateTosearch2 = HelperMethods.TimeHelper.FindStartDayOfWeek(DateTime.Today);
                        punchcardLists = punchcardLists.
                                    Where(e => e.WorkDate >= dateTosearch2 && e.WorkDate <= dateTosearch2.AddDays(6))
                                    .OrderBy(p => p.WorkDate);
                        return View(punchcardLists);

                    case "CurrentMonth":
                        punchcardLists = punchcardLists
                            .Where(p => p.WorkDate >= HelperMethods.TimeHelper.StartDateOfMonth(DateTime.Today) &&
                            p.WorkDate <= HelperMethods.TimeHelper.LastDateOfMonth(DateTime.Today))
                            .OrderBy(pc => pc.WorkDate);
                        ViewBag.ThisYear = HelperMethods.TimeHelper.StartDateOfMonth(DateTime.Now).ToString("Y");
                        return View(punchcardLists);


                    default:
                        ViewBag.ErrorMessage = "Please select option to find!";
                        break;
                }
            }

            return View(punchcardLists);
        }
        

        [Authorize]
        // GET: Punchcard/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create New Punchcard
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Punchcard punchCard)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if(userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    try
                    {
                        punchCard.UserId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
                        _context.Add(punchCard);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));

                    }
                    catch (DbUpdateException)
                    {
                        return NotFound();
                    }
                }
            }
            return View(punchCard);
        }

        public async Task<IActionResult> Details (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Punchcard = await _context.Punchcards.SingleOrDefaultAsync(p => p.PunchcardId.Equals(id));
            if(Punchcard == null)
            {
                return NotFound();
            }
            return View(Punchcard);
        }

        // GET: Punchcard/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PunchcardLists = await _context.Punchcards.SingleOrDefaultAsync(m => m.PunchcardId.Equals(id));
            if(PunchcardLists == null)
            {
                return NotFound();
            }

            return View(PunchcardLists);
        }

        // POST: Punchcard/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Punchcard punchCard)
        {
            var userId = Int32.Parse(_userManager.GetUserId(HttpContext.User));

            if (id != punchCard.PunchcardId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    punchCard.UserId = Int32.Parse(_userManager.GetUserId(HttpContext.User));

                    _context.Update(punchCard);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Punchcard/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
            if(id == null)
            {
                return NotFound();
            }
            var punchCard = await _context.Punchcards
                .SingleOrDefaultAsync(m => m.PunchcardId.Equals(id) && m.UserId.Equals(userId));
            if (punchCard == null)
            {
                return NotFound();
            }

            return View(punchCard);
        }

        // POST: Punchcard/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
                
                var punchCardToRemove = await _context.Punchcards
                    .SingleOrDefaultAsync(m => m.PunchcardId.Equals(id) && m.UserId.Equals(userId));
                if (punchCardToRemove == null)
                {
                    return NotFound();
                }
                _context.Punchcards.Remove(punchCardToRemove);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
    }
}
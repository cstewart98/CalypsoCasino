using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CalypsoCasino.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CalypsoCasino.Controllers;

public class CasinoController : Controller
{
    private MyContext db;
    public CasinoController(MyContext context)
    {
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        HttpContext.Session.Clear();
        return View("Index");
    }

    // After a user attempts to enter a 'Username'
    [HttpPost]
    public IActionResult Register(Player newPlayer)
    {
        try
        {

            // If the Username is null / the field is blank
            if (newPlayer.Username == null)
            {
                Console.WriteLine("Username is required!");
                ModelState.AddModelError("Username", "Username is required");
                return View("Index");
            }

            // The Username is less than 3 characters long
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Oops something went wrong!");
                return View("Index");
            }

        }
        catch (Exception ex)
        {

            // Handle exception for Method 
            Console.WriteLine($"Error message: {ex.Message}");
        }
            // User enters a valid username
            HttpContext.Session.SetInt32("UUID", newPlayer.PlayerId);
            HttpContext.Session.SetString("Username", newPlayer.Username);

            // Presenting your Bank
            int startingBank = 100;
            HttpContext.Session.SetInt32("Bank", startingBank);
            int highestBank = 0;
            HttpContext.Session.SetInt32("HighestBank", highestBank);
            int betting = 0;
            HttpContext.Session.SetInt32("Bet", betting);

            return RedirectToAction("Rules");
    }

    [SessionCheck]
    [HttpGet("casino/rules")]
    public IActionResult Rules()
    {
        return View("Rules");
    }

    //Goes to invalid page if enter wrong route
    [HttpGet("{**route}")]
    public IActionResult Unknown()
    {
        Console.WriteLine("Invalid route :(");
        return View("404");
    }
}


public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;

        // Check if the required session data exists (you can modify this based on your session setup)
        if (session.GetInt32("UUID") == null || string.IsNullOrEmpty(session.GetString("Name")))
        {
            // Redirect to another action or handle the case when session data is missing
            context.Result = new RedirectToActionResult("Index", "Casino", null);
        }

        base.OnActionExecuting(context);
    }
}
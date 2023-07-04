using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Antlr.Runtime.Misc;
using CarSale.Controllers;
using CarSale.Models;
using static System.Net.Mime.MediaTypeNames;


namespace CarSale.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Register
        public ActionResult SignUp() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Seller seller)
        {
            using (SellerDbEntities sellerDb = new SellerDbEntities())
            {
                //Checks if the entered username already exists in the database. 
                if (sellerDb.Sellers.Any(x => x.Username == seller.Username))
                {
                    //If the username exists,
                    //sets a error message and returns the "SignUp" view with the provided seller model.
                    ViewBag.DuplicateMessage = "Username already exist";
                    return View("SignUp", seller);
                }
                //If the username doesn't exist, save the seller to the database.
                sellerDb.Sellers.Add(seller);
                sellerDb.SaveChanges();

                // Sets a success message
                TempData["SuccessMessage"] = "Registration successful";
            }
            //clears the ModelState, and redirects the user to the "Login" action of the "Account" controller.     
            ModelState.Clear();
            return RedirectToAction("Login", "Account");
        }

        //Login
        [HttpGet]
        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForger yToken]
        public ActionResult Login(Seller seller)
        {
            using (SellerDbEntities sellerDb = new SellerDbEntities())
            {
                // Check if the username and password match a seller in the database
                var sellerDetail = sellerDb.Sellers.Where(x => x.Username == seller.Username && x.Password == seller.Password).FirstOrDefault();
                if (sellerDetail == null)
                {
                    // If no seller is found, set an error message and return the Login view with the snentered seller model
                    seller.LoginErrorMsg = "Invalid Username or Password";
                    return View("Login", seller);
                }
                else
                {
                    // If a seller is found, store the username in the session and
                    // redirect to the Add action method in the Home controller
                    Session["Username"] = sellerDetail.Username;
                    return RedirectToAction("Add", "Home");
                }
            }
        }

        //Logout
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
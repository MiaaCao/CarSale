using CarSale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CarSale.Controllers
{
    public class HomeController : Controller
    {
        //Home page
        public ActionResult Index()
        {
            return View();
        }

        //Add Car
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Car car)
        {
            using (CarDbEntities carDb = new CarDbEntities())
            {
                //add the car detail to database
                carDb.Cars.Add(car);
                carDb.SaveChanges();
                ViewBag.addCarMsg = "Add car successfully!";
            }
            return View (car);
        }


        //Car Search
        public ActionResult CarSearch(string searching)
        {
            CarDbEntities carDb = new CarDbEntities();
            // checks if the search criteria is not empty or null
            if (!String.IsNullOrEmpty(searching))
            {
                //uses the Where method with multiple conditions
                //to check if any of the car's properties contain the search criteria.
                //The matching cars are then converted to a list using the ToList method
                //and stored in the cars variable.
                var cars = carDb.Cars
        .Where(x => x.CompanyName.Contains(searching) ||
                    x.CarModel.Contains(searching) ||
                    x.Year.Contains(searching) ||
                    x.Price.ToString().Contains(searching) ||
                    x.Location.Contains(searching) ||
                    x.BodyType.Contains(searching))
        .ToList();
                //check the user enter value are not matching the car database
                //it will display the message
                if (!cars.Any())
                {
                    ViewBag.msg = "Not Match!!!";
                }
            }
            //display the car database information
            return View(carDb.Cars.Where(x => 
            x.CompanyName.Contains(searching) ||
            x.CarModel.Contains(searching) || 
            x.Year.Contains(searching) ||
            x.Price.ToString().Contains(searching) || 
            x.Location.Contains(searching) ||
            x.BodyType.Contains(searching)).ToList());
        }


        //Feedback
        public ActionResult UFeedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UFeedback(Feedback feedback)
        {
            using (FeedbackDbEntities feedbackDb = new FeedbackDbEntities())
            {
                //add feedback value to the database
                feedbackDb.Feedbacks.Add(feedback);
                feedbackDb.SaveChanges();
                //ViewBag.SuccMsg = "Thank you for your comment!";
            }
            
            ModelState.Clear();
            return View(feedback);
        }


    }
}
using SharedTrip.Models;
using SharedTrip.Services;
using SharedTrip.ViewModels;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private ITripsService service;


        public TripsController(ITripsService service)
        {
            this.service = service;

        }

        [HttpGet]
        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripsInputModel model)
        {

 
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(model.Description) || model.Description.Length > 80)
            {
                return this.Redirect("/Trips/Add");
            }

            if(model.Seats > 6 || model.Seats < 2)
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrEmpty(model.EndPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if(model.DepartureTime == null)
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrEmpty(model.StartPoint))
            {
                return this.Redirect("/Trips/Add");
            }
         
            var trip = new Trip
            {
                DepartureTime = DateTime.ParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Description = model.Description,
                EndPoint = model.EndPoint,
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                StartPoint = model.StartPoint,
            };

            this.service.AddTrip(trip);

            return this.Redirect("/Trips/All");
        }


        [HttpGet]

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var model = new List<TripsViewModel>();

            var trips = service.GetAll();

            foreach (var trip in trips)
            {
                model.Add(new TripsViewModel
                {
                    DepartureTime = trip.DepartureTime.ToString(),
                    EndPoint = trip.EndPoint,
                    Id = trip.Id,
                    Seats = trip.Seats,
                    StartPoint = trip.StartPoint
                });
            }
     
            return this.View(model.ToArray());
        }


        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trip = this.service.GetTrip(tripId);

            var model = new TripDetailsViewModel
            {
                DepartureTime = trip.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                Description = trip.Description,
                EndPoint = trip.EndPoint,
                Id = trip.Id,
                ImagePath = trip.ImagePath,
                Seats = trip.Seats,
                StartPoint = trip.StartPoint
            };

            return this.View(model);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(this.service.AddUserToTrip(tripId, User))
            {
              return this.Redirect("/Trips/All");
            }
            else
            {
                return this.Redirect("/Trips/Details?tripId=" + tripId);
            }
        }

    }
}

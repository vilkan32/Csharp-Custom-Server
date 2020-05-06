using SharedTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {

        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddTrip(Trip trip)
        {
            this.db.Trips.Add(trip);
            this.db.SaveChanges();
        }

        public List<Trip> GetAll()
        {
           return this.db.Trips.ToList();
        }


        public Trip GetTrip(string id)
        {
           return this.db.Trips.FirstOrDefault(x => x.Id == id);
        }


        public bool AddUserToTrip(string tripId, string userId)
        {

            var user = this.db.Users.FirstOrDefault(x => x.Id == userId);

            var trip = this.db.Trips.FirstOrDefault(x => x.Id == tripId);

            if(trip.Seats <= 0)
            {
                return false;
            }
            else
            {
                trip.Seats -= 1;

                user.UserTrips.Add(new UserTrip { TripId = tripId });

                try
                {
                    this.db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                    
                }
            }

            return true;
        }
    }
}

using SharedTrip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        void AddTrip(Trip trip);

        List<Trip> GetAll();

        Trip GetTrip(string id);

        bool AddUserToTrip(string tripId, string userId);
    }
}

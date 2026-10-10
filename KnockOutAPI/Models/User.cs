using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KnockOutAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }

    public class Guest
    {
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public string Contact { get; set; }
        public DateTime CheckOut { get; set; }
        public bool IsPaid { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<GuestFacility> GuestFacilities { get; set; }
    }

    public class Facility
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
    }

    public class GuestFacility
    {
        public int GuestFacilityId { get; set; }
        public int GuestId { get; set; }
        public int FacilityId { get; set; }
        public int TotalVisits { get; set; }
        public decimal Price { get; set; }
        public virtual Facility Facility { get; set; }
    }

    public class GuestDbContext : DbContext
    {
        public GuestDbContext() : base("GuestDbContext")
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<GuestFacility> GuestFacilities { get; set; }
    }
}
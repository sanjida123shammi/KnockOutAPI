namespace KnockOutAPI.Migrations
{
    using KnockOutAPI.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KnockOutAPI.Models.GuestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(KnockOutAPI.Models.GuestDbContext context)
        {
            context.Facilities.AddOrUpdate(f => f.FacilityId,
                new Facility { FacilityId = 1, FacilityName = "Fitness" },
                new Facility { FacilityId = 2, FacilityName = "Swimming Pool" },
                new Facility { FacilityId = 3, FacilityName = "Meeting room" },
                new Facility { FacilityId = 4, FacilityName = "Jacuzzi" }
            );
        }
    }
}

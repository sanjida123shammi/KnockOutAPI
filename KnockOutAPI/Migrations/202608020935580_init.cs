namespace KnockOutAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Facilities",
                c => new
                    {
                        FacilityId = c.Int(nullable: false, identity: true),
                        FacilityName = c.String(),
                    })
                .PrimaryKey(t => t.FacilityId);
            
            CreateTable(
                "dbo.GuestFacilities",
                c => new
                    {
                        GuestFacilityId = c.Int(nullable: false, identity: true),
                        GuestId = c.Int(nullable: false),
                        FacilityId = c.Int(nullable: false),
                        TotalVisits = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.GuestFacilityId)
                .ForeignKey("dbo.Facilities", t => t.FacilityId, cascadeDelete: true)
                .ForeignKey("dbo.Guests", t => t.GuestId, cascadeDelete: true)
                .Index(t => t.GuestId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestId = c.Int(nullable: false, identity: true),
                        GuestName = c.String(),
                        Contact = c.String(),
                        CheckOut = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.GuestId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Roles = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GuestFacilities", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.GuestFacilities", "FacilityId", "dbo.Facilities");
            DropIndex("dbo.GuestFacilities", new[] { "FacilityId" });
            DropIndex("dbo.GuestFacilities", new[] { "GuestId" });
            DropTable("dbo.Users");
            DropTable("dbo.Guests");
            DropTable("dbo.GuestFacilities");
            DropTable("dbo.Facilities");
        }
    }
}

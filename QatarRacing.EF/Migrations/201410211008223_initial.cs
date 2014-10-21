namespace QatarRacing.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jockeys",
                c => new
                    {
                        JockeyID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.JockeyID);
            
            CreateTable(
                "dbo.Runners",
                c => new
                    {
                        RunnerID = c.Int(nullable: false, identity: true),
                        Horse = c.String(),
                        Equipment = c.String(),
                        Name = c.String(),
                        Trainer = c.String(),
                        Position = c.String(),
                        Drawn = c.Int(nullable: false),
                        OR = c.String(),
                        Margin = c.String(),
                        Race_RaceID = c.Int(nullable: false),
                        Jockey_JockeyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RunnerID)
                .ForeignKey("dbo.Races", t => t.Race_RaceID, cascadeDelete: true)
                .ForeignKey("dbo.Jockeys", t => t.Jockey_JockeyID, cascadeDelete: true)
                .Index(t => t.Race_RaceID)
                .Index(t => t.Jockey_JockeyID);
            
            CreateTable(
                "dbo.Races",
                c => new
                    {
                        RaceID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Type = c.String(),
                        Class = c.String(),
                        TrackLength = c.String(),
                        TrackType = c.String(),
                        WinningPrice = c.Long(nullable: false),
                        WinningCurrency = c.String(),
                        Weather = c.String(),
                        RaceConditions = c.String(),
                        RailPosition = c.String(),
                        SafetyLimit = c.String(),
                        RunningTime = c.String(),
                        RunningTimeType = c.String(),
                        Location = c.String(),
                        Notes = c.String(),
                        Link_LinkID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RaceID)
                .ForeignKey("dbo.Links", t => t.Link_LinkID, cascadeDelete: true)
                .Index(t => t.Link_LinkID);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.LinkID);
            
            CreateTable(
                "dbo.WinningPrices",
                c => new
                    {
                        WinningPriceID = c.Int(nullable: false, identity: true),
                        RaceId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Price = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.WinningPriceID)
                .ForeignKey("dbo.Races", t => t.RaceId, cascadeDelete: true)
                .Index(t => t.RaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Runners", "Jockey_JockeyID", "dbo.Jockeys");
            DropForeignKey("dbo.WinningPrices", "RaceId", "dbo.Races");
            DropForeignKey("dbo.Runners", "Race_RaceID", "dbo.Races");
            DropForeignKey("dbo.Races", "Link_LinkID", "dbo.Links");
            DropIndex("dbo.WinningPrices", new[] { "RaceId" });
            DropIndex("dbo.Races", new[] { "Link_LinkID" });
            DropIndex("dbo.Runners", new[] { "Jockey_JockeyID" });
            DropIndex("dbo.Runners", new[] { "Race_RaceID" });
            DropTable("dbo.WinningPrices");
            DropTable("dbo.Links");
            DropTable("dbo.Races");
            DropTable("dbo.Runners");
            DropTable("dbo.Jockeys");
        }
    }
}

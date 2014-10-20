namespace QatarRacing.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.LinkID);
            
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
                        TrackCondition = c.String(),
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
                "dbo.Runners",
                c => new
                    {
                        RunnerID = c.Int(nullable: false, identity: true),
                        Horse = c.String(),
                        Jockey = c.String(),
                        Equipment = c.String(),
                        Name = c.String(),
                        Trainer = c.String(),
                        Position = c.String(),
                        Drawn = c.Int(nullable: false),
                        OR = c.String(),
                        Margin = c.String(),
                        Race_RaceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RunnerID)
                .ForeignKey("dbo.Races", t => t.Race_RaceID, cascadeDelete: true)
                .Index(t => t.Race_RaceID);
            
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
            DropForeignKey("dbo.Races", "Link_LinkID", "dbo.Links");
            DropForeignKey("dbo.WinningPrices", "RaceId", "dbo.Races");
            DropForeignKey("dbo.Runners", "Race_RaceID", "dbo.Races");
            DropIndex("dbo.WinningPrices", new[] { "RaceId" });
            DropIndex("dbo.Runners", new[] { "Race_RaceID" });
            DropIndex("dbo.Races", new[] { "Link_LinkID" });
            DropTable("dbo.WinningPrices");
            DropTable("dbo.Runners");
            DropTable("dbo.Races");
            DropTable("dbo.Links");
        }
    }
}

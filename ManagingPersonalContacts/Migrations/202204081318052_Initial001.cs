namespace ManagingPersonalContacts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        Desc = c.String(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                        Entrance = c.String(),
                        Floor = c.String(),
                        Apartment = c.String(),
                        Default = c.Boolean(nullable: false),
                        PersonalContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.PersonalContacts", t => t.PersonalContactID, cascadeDelete: true)
                .Index(t => t.PersonalContactID);
            
            CreateTable(
                "dbo.PersonalContacts",
                c => new
                    {
                        PersonalContactID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        SurName = c.String(),
                        DateOfBirth = c.DateTime(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        IBAN = c.String(),
                        ImgFileName = c.String(),
                        ImgContentType = c.String(),
                        ImgContent = c.Binary(),
                        ImgFileNameGen = c.String(),
                    })
                .PrimaryKey(t => t.PersonalContactID);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        BankID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Desc = c.String(),
                        IBAN = c.String(nullable: false),
                        Default = c.Boolean(nullable: false),
                        PersonalContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BankID)
                .ForeignKey("dbo.PersonalContacts", t => t.PersonalContactID, cascadeDelete: true)
                .Index(t => t.PersonalContactID);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        PhoneNumberID = c.Int(nullable: false, identity: true),
                        PhoneNo = c.String(nullable: false),
                        Desc = c.String(),
                        Default = c.Boolean(nullable: false),
                        PersonalContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhoneNumberID)
                .ForeignKey("dbo.PersonalContacts", t => t.PersonalContactID, cascadeDelete: true)
                .Index(t => t.PersonalContactID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhoneNumbers", "PersonalContactID", "dbo.PersonalContacts");
            DropForeignKey("dbo.Banks", "PersonalContactID", "dbo.PersonalContacts");
            DropForeignKey("dbo.Addresses", "PersonalContactID", "dbo.PersonalContacts");
            DropIndex("dbo.PhoneNumbers", new[] { "PersonalContactID" });
            DropIndex("dbo.Banks", new[] { "PersonalContactID" });
            DropIndex("dbo.Addresses", new[] { "PersonalContactID" });
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.Banks");
            DropTable("dbo.PersonalContacts");
            DropTable("dbo.Addresses");
        }
    }
}

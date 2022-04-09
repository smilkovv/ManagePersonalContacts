namespace ManagingPersonalContacts.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using ManagingPersonalContacts.Models;


    internal sealed class Configuration : DbMigrationsConfiguration<ManagingPersonalContacts.Models.PersonalContactDBContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ManagingPersonalContacts.Models.PersonalContactDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.PersonalContacts.AddOrUpdate(i => i.FirstName,
                new PersonalContact
                {
                    FirstName = "New Data Base Migrated",
                }
             );
        }
    }
}

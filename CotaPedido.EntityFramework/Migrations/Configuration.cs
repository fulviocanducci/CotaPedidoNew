namespace CotaPedido.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CotaPedido.EntityFramework.Models.COTAPEDIDOContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CotaPedido.EntityFramework.Models.COTAPEDIDOContext context)
        {
           
        }
    }
}

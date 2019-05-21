using Sindikat.Ankete.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Ankete.Persistence
{
    class AnketeDbContextFactory : DesignTimeDbContextFactoryBase<AnketeDbContext>
    {
        protected override AnketeDbContext CreateNewInstance(DbContextOptions<AnketeDbContext> options)
        {
            return new AnketeDbContext(options);
        }
    }
}

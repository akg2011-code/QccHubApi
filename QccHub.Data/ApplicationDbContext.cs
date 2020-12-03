using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QccHub.Data.Extensions;
using QccHub.Logic.Helpers;

namespace QccHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly CurrentSession currentSession;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, CurrentSession currentSession)
            : base(options)
        {
            this.currentSession = currentSession;
        }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<ApplyJobs> ApplyJobs { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsComments> NewsComments { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Question> Question { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureShadowProperties();
            builder.SetGlobalQueryFilters();
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.SetShadowProperties(currentSession);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

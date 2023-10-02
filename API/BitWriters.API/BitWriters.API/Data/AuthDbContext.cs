using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BitWriters.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "c4510cc3-0804-4d56-9151-e3cca812171f";
            var writerRoleId = "94deae23-2fb1-4805-b0b1-6f2bc80146b9";

            //Create reader and writer role 
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                },
            };

            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);


            //Create an Admin user
            var adminUserId = "d25fe288-abd9-42c9-86b2-ebd38001791b";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "chinmay@bitwriters.com",
                Email = "chinmay@bitwriters.com",
                NormalizedEmail = "chinmay@bitwriters.com".ToUpper(),
                NormalizedUserName = "chinmay@bitwriters.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            //seed admin user data to DB
            builder.Entity<IdentityUser>().HasData(admin);


            //give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            //seed user-role to db
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using QccHub.Data;
using QccHubApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QccHubApi
{
    public class CountryDTO
    {
        public string Name { get; set; }
    }
    public class SeedingData
    {
        public ApplicationDbContext Context;
        public SeedingData(ApplicationDbContext _context)
        {
            Context = _context;
        }

        public void SeedAllCountry()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://restcountries.eu/rest/v2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var responseTask = client.GetAsync("all?fields=name");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask =  result.Content.ReadAsStringAsync().Result;
                    var JsonReuslt = JsonConvert.DeserializeObject<List<CountryDTO>>(readTask);

                    foreach (var country in JsonReuslt)
                    {
                        if (!Context.Country.Any(x => x.Name == country.Name))
                             Context.Country.Add(new Country { IsDeleted = false, CreatedDate = DateTime.Now, Name = country.Name });
                    }
                     Context.SaveChanges();
                }
            }
        }


        public void SeedGender()
        {
            List<Gender> genders = new List<Gender>
            {
                new Gender{Name = "Male",IsDeleted =false , CreatedDate = DateTime.Now},
                new Gender{Name="Female",IsDeleted =false , CreatedDate = DateTime.Now},
                new Gender{Name="Other",IsDeleted =false , CreatedDate = DateTime.Now}
            };

            foreach (var record in genders)
            {
                if (!Context.Gender.Any(g => g.Name == record.Name))
                {
                    Context.Gender.Add(record);
                }
            }
            Context.SaveChanges();
        }

        public void SeedPaymentStatus()
        {
            List<PaymentStatus> status = new List<PaymentStatus>
            {
                new PaymentStatus{StatusName = "Done",IsDeleted =false , CreatedDate = DateTime.Now},
                new PaymentStatus{StatusName = "Pending",IsDeleted =false , CreatedDate = DateTime.Now},
                new PaymentStatus{StatusName="Canceled",IsDeleted =false , CreatedDate = DateTime.Now}
            };

            foreach (var record in status)
            {
                if (!Context.PaymentStatus.Where(s => s.StatusName == record.StatusName).Any())
                {
                Context.PaymentStatus.Add(record);
                }
            }
            Context.SaveChanges();
        }
    }

    public class ApplicationDbInitializer
    {
        public static void SeedingData(UserManager<User> userManager , RoleManager<IdentityRole> roleManager , ApplicationDbContext context)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            
            SeedingData data = new SeedingData(context);
            data.SeedGender();
            data.SeedPaymentStatus();
            data.SeedAllCountry();
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // create admin role :
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole { Name = "Admin" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            // create User role :
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole { Name = "User" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            // create Vendor role :
            if (!roleManager.RoleExistsAsync("Company").Result)
            {
                IdentityRole role = new IdentityRole { Name = "Company" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("qcchub@admin.com").Result == null)
            {
                User admin = new User
                {
                    UserName = "admin",
                    Email = "qcchub@admin.com",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "COOKIESADMIN.ADMIN.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "01032873503",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsTrusted = true,
                };

                IdentityResult result = userManager.CreateAsync(admin, "Admin@2020").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }

            }
        }

        
    }
}

using ExaminationSystem.Components;
using ExaminationSystem.Components.Account;
using ExaminationSystem.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;



//Role Email	Password
//Admin	admin@exam.com	Admin@123
//Student 1	student1@exam.com	Student@123
//Student 2	student2@exam.com	Student@123

namespace ExaminationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("ConnecString")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAntiforgery();
            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapAdditionalIdentityEndpoints();

            // Seed database with admin user and sample data
            SeedDatabase(app).GetAwaiter().GetResult();

            app.Run();
        }

        private static async Task SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                try
                {
                    // Ensure database is created
                    await dbContext.Database.MigrateAsync();

                    // Create roles if they don't exist
                    if (!await roleManager.RoleExistsAsync("Admin"))
                        await roleManager.CreateAsync(new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN" });

                    if (!await roleManager.RoleExistsAsync("Instructor"))
                        await roleManager.CreateAsync(new ApplicationRole { Name = "Instructor", NormalizedName = "INSTRUCTOR" });

                    if (!await roleManager.RoleExistsAsync("Student"))
                        await roleManager.CreateAsync(new ApplicationRole { Name = "Student", NormalizedName = "STUDENT" });

                    // Create admin user
                    var adminUser = await userManager.FindByEmailAsync("admin@exam.com");
                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            UserName = "admin@exam.com",
                            Email = "admin@exam.com",
                            FName = "Admin",
                            LName = "User",
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(adminUser, "Admin@123");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }

                    // Create instructor user
                    var instructorUser = await userManager.FindByEmailAsync("instructor@exam.com");
                    if (instructorUser == null)
                    {
                        instructorUser = new ApplicationUser
                        {
                            UserName = "instructor@exam.com",
                            Email = "instructor@exam.com",
                            FName = "John",
                            LName = "Instructor",
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(instructorUser, "Instructor@123");
                        await userManager.AddToRoleAsync(instructorUser, "Instructor");
                    }

                    // Create sample student users
                    var student1 = await userManager.FindByEmailAsync("student1@exam.com");
                    if (student1 == null)
                    {
                        student1 = new ApplicationUser
                        {
                            UserName = "student1@exam.com",
                            Email = "student1@exam.com",
                            FName = "Ahmed",
                            LName = "Student",
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(student1, "Student@123");
                        await userManager.AddToRoleAsync(student1, "Student");
                    }

                    var student2 = await userManager.FindByEmailAsync("student2@exam.com");
                    if (student2 == null)
                    {
                        student2 = new ApplicationUser
                        {
                            UserName = "student2@exam.com",
                            Email = "student2@exam.com",
                            FName = "Fatima",
                            LName = "Student",
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(student2, "Student@123");
                        await userManager.AddToRoleAsync(student2, "Student");
                    }

                    // Re-fetch instructor and students to ensure tracked entities are available
                    instructorUser = await userManager.FindByEmailAsync("instructor@exam.com");
                    student1 = await userManager.FindByEmailAsync("student1@exam.com");
                    student2 = await userManager.FindByEmailAsync("student2@exam.com");

                    // Seed courses
                    if (!dbContext.Courses.Any())
                    {
                        var courses = new List<ExaminationSystem.Data.Models.Course>
                        {
                            new ExaminationSystem.Data.Models.Course
                            {
                                Id = Guid.NewGuid(),
                                CourseName = "C# Fundamentals",
                                InstructorId = instructorUser.Id,
                                Instructor = instructorUser
                            },
                            new ExaminationSystem.Data.Models.Course
                            {
                                Id = Guid.NewGuid(),
                                CourseName = "Entity Framework Core",
                                InstructorId = instructorUser.Id,
                                Instructor = instructorUser
                            },
                            new ExaminationSystem.Data.Models.Course
                            {
                                Id = Guid.NewGuid(),
                                CourseName = "ASP.NET Core Blazor",
                                InstructorId = instructorUser.Id,
                                Instructor = instructorUser
                            }
                        };

                        // Enroll both students in all courses
                        foreach (var course in courses)
                        {
                            course.Students.Add(student1);
                            course.Students.Add(student2);
                        }

                        dbContext.Courses.AddRange(courses);
                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding database: {ex.Message}");
                }
            }
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.DataAccess.Data
{
    public static class InquiryInitializer
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    context.Database.EnsureCreated();

                    var inquiries = context.Inquiries.FirstOrDefault();
                    if (inquiries == null)
                    {
                        context.Addresses.AddRange(
                            new Models.Address
                            {
                                streetName = "Street",
                                houseNumber = 1,
                                flatNumber = 2,
                                postcode = "12-345",
                                city = "City"
                            },
                            new Models.Address
                            {
                                streetName = "Street_1",
                                houseNumber = 1,
                                flatNumber = 2,
                                postcode = "12-345",
                                city = "City_1"
                            }
                        );

                        context.Packages.AddRange(
                            new Models.Package
                            {
                                width = 2,
                                height = 2,
                                length = 1,
                                dimensionsUnit = Models.DimensionUnit.Meters,
                                weight = 1,
                                weightUnit = Models.WeightUnit.Kilograms
                            },
                            new Models.Package
                            {
                                width = 4,
                                height = 6,
                                length = 7,
                                dimensionsUnit = Models.DimensionUnit.Inches,
                                weight = 1,
                                weightUnit = Models.WeightUnit.Pounds
                            }
                        );

                        context.Inquiries.AddRange(
                            new Models.Inquiry
                            {
                                pickupDate = DateTime.Now.AddDays(5),
                                deliveryDate = DateTime.Now.AddDays(8),
                                isPriority = true,
                                isCompany = false,
                                sourceAddressId = 1,
                                destinationAddressId = 2,
                                packageId = 1,
                                creationDate = DateTime.Now,
                            },
                            new Models.Inquiry
                            {
                                pickupDate = DateTime.Now.AddDays(6),
                                deliveryDate = DateTime.Now.AddDays(10),
                                isPriority = false,
                                isCompany = true,
                                sourceAddressId = 2,
                                destinationAddressId = 1,
                                packageId = 2,
                                creationDate = DateTime.Now,
                            }
                        );

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
                return app;
            }
        }

    }
}

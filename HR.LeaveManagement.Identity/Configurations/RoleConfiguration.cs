using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "791A6180-7115-4ED0-8BDD-46D143893723",
                Name = "Employee",
                NormalizedName = "EMPLOYEE",
                ConcurrencyStamp = "2DC46D09-ED54-407F-BF4D-0838745DE162"
            },
            new IdentityRole
            {
                Id = "21FE5075-DBCD-4806-83C9-877C692E24AA",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = "1155D2D2-A1D2-409D-8AC2-BB6ACA4B76C8"
            }
        );
    }
}

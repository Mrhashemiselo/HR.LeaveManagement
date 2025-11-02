using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;
public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "21FE5075-DBCD-4806-83C9-877C692E24AA",
                UserId = "400E0CE2-8405-4CB2-93AA-0BB4E59558B2"
            },
            new IdentityUserRole<string>
            {
                RoleId = "791A6180-7115-4ED0-8BDD-46D143893723",
                UserId = "DC9BD49C-5DFF-4555-AFB9-550067551265"
            }
        );
    }
}
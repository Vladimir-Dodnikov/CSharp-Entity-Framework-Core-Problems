using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Comman;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(b => b.Username)
                .HasMaxLength(GlobalConstant.ClientUsernameMaxLength)
                .IsUnicode(false);

            builder.Property(b => b.Email)
                .HasMaxLength(GlobalConstant.ClientEmailMaxLength)
                .IsUnicode(false);


            builder.Property(b => b.FirstName)
                .HasMaxLength(GlobalConstant.ClientUsernameMaxLength)
                .IsUnicode(true);

            builder.Property(b => b.LastName)
                .HasMaxLength(GlobalConstant.ClientUsernameMaxLength)
                .IsUnicode(true);
        }
    }
}

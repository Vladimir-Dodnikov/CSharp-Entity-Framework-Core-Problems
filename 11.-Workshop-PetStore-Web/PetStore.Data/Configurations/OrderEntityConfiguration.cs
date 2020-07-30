using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Comman;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Data.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Town)
                .HasMaxLength(GlobalConstant.OrderTownNameMaxLength)
                .IsUnicode(true);

            builder.Property(x => x.Address)
                .HasMaxLength(GlobalConstant.OrderAddressMaxLength)
                .IsUnicode(true);

            builder.Ignore(o => o.TotalPrice);
        }
    }
}

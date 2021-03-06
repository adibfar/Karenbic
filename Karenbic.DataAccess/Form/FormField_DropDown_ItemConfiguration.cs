﻿using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_DropDown_ItemConfiguration : EntityTypeConfiguration<DomainClasses.FormField_DropDown_Item>
    {
        public FormField_DropDown_ItemConfiguration()
        {
            HasRequired(x => x.DropDown)
                .WithMany(x => x.Items)
                .WillCascadeOnDelete(true);

            HasMany(x => x.OrderPriceValues)
                .WithOptional(x => x.Value)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Orders_Value)
                .WithOptional(x => x.Value)
                .WillCascadeOnDelete(false);
        }
    }
}

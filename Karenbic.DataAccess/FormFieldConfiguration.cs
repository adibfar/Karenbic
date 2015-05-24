﻿using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormFieldConfiguration : EntityTypeConfiguration<DomainClasses.FormField>
    {
        public FormFieldConfiguration()
        {
            HasRequired(x => x.DesktopPosition)
                .WithOptional(x => x.FormField)
                .WillCascadeOnDelete(true);

            HasRequired(x => x.TabletPosition)
                .WithOptional(x => x.FormField)
                .WillCascadeOnDelete(true);

            HasRequired(x => x.MobilePosition)
                .WithOptional(x => x.FormField)
                .WillCascadeOnDelete(true);

            HasRequired(x => x.Form)
                .WithMany(x => x.Fields)
                .WillCascadeOnDelete(true);
        }
    }
}

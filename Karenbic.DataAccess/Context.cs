using System;
using System.Data.Entity;

namespace Karenbic.DataAccess
{
    public class Context : DbContext
    {
        public Context()
            : base("cn1")
        {

        }

        public DbSet<DomainClasses.Setting> Setting { get; set; }

        public DbSet<DomainClasses.Province> Province { get; set; }
        public DbSet<DomainClasses.City> Cities { get; set; }


        public DbSet<DomainClasses.CustomerGroup> CustomerGroups { get; set; }
        public DbSet<DomainClasses.Customer> Customers { get; set; }


        public DbSet<DomainClasses.FileFormat> FileFormats { get; set; }


        public DbSet<DomainClasses.FormGroup> FormGroups { get; set; }
        public DbSet<DomainClasses.Form> Forms { get; set; }
        public DbSet<DomainClasses.FormField> FormFields { get; set; }
        public DbSet<DomainClasses.FormField_TextBox> FormFields_TextBox { get; set; }
        public DbSet<DomainClasses.FormField_TextArea> FormFields_TextArea { get; set; }
        public DbSet<DomainClasses.FormField_Numeric> FormFileds_Numeric { get; set; }
        public DbSet<DomainClasses.FormField_ColorPicker> FormFields_ColorPicker { get; set; }
        public DbSet<DomainClasses.FormField_FileUploader> FormFields_FileUploader { get; set; }
        public DbSet<DomainClasses.FormField_DropDown> FormFields_DropDown { get; set; }
        public DbSet<DomainClasses.FormField_DropDown_Item> FormField_DropDown_Items { get; set; }
        public DbSet<DomainClasses.FormField_RadioButtonGroup> FormFields_RadioButtonGroup { get; set; }
        public DbSet<DomainClasses.FormField_RadioButtonGroup_Item> FormField_RadioButtonGroup_Items { get; set; }
        public DbSet<DomainClasses.FormField_CheckBox> FormFields_CheckBox { get; set; }
        public DbSet<DomainClasses.FormField_CheckBoxGroup> FormFields_CheckBoxGroup { get; set; }
        public DbSet<DomainClasses.FormField_CheckBoxGroup_Item> FormField_CheckBoxGroup_Items { get; set; }
        public DbSet<DomainClasses.FormField_WebUrl> FormFields_WebUrl{ get; set; }
        public DbSet<DomainClasses.FormField_DatePicker> FormFields_DatePicker { get; set; }


        public DbSet<DomainClasses.Order> Orders { get; set; }
        public DbSet<DomainClasses.PrintOrder> PrintOrders { get; set; }
        public DbSet<DomainClasses.DesignOrder> DesignOrders { get; set; }
        public DbSet<DomainClasses.DesignOrder_Design> DesignOrder_Designs { get; set; }
        public DbSet<DomainClasses.DesignOrder_Design_File> DesignOrder_Design_Files { get; set; }
        public DbSet<DomainClasses.Order_Value> Order_Values { get; set; }
        public DbSet<DomainClasses.Order_Value_TextBox> Order_Values_TextBox { get; set; }
        public DbSet<DomainClasses.Order_Value_TextArea> Order_Values_TextArea { get; set; }
        public DbSet<DomainClasses.Order_Value_Numeric> Order_Values_Numeric { get; set; }
        public DbSet<DomainClasses.Order_Value_ColorPicker> Order_Values_ColorPicker { get; set; }
        public DbSet<DomainClasses.Order_Value_FileUploader> Order_Values_FileUploader { get; set; }
        public DbSet<DomainClasses.Order_Value_Checkbox> Order_Values_Checkbox { get; set; }
        public DbSet<DomainClasses.Order_Value_WebUrl> Order_Values_WebUrl { get; set; }
        public DbSet<DomainClasses.Order_Value_DatePicker> Order_Values_DatePicker { get; set; }
        public DbSet<DomainClasses.Order_Value_DropDown> Order_Values_DropDown { get; set; }
        public DbSet<DomainClasses.Order_Value_RadioButtonGroup> Order_Values_RadioButtonGroup { get; set; }
        public DbSet<DomainClasses.Order_Value_CheckboxGroup> Order_Values_CheckboxGroup { get; set; }


        public DbSet<DomainClasses.PrintFactor> PrintFactors { get; set; }
        public DbSet<DomainClasses.PrepaymentDesignFactor> PrepaymentDesignFactors { get; set; }
        public DbSet<DomainClasses.FinalDesignFactor> FinalDesignFactors { get; set; }
        public DbSet<DomainClasses.Payment> Payments { get; set; }
        public DbSet<DomainClasses.PrintPayment> PrintPayments { get; set; }
        public DbSet<DomainClasses.PrintPaymentItem> PrintPaymentItems { get; set; }
        public DbSet<DomainClasses.DesignPayment> DesignPayments { get; set; }
        public DbSet<DomainClasses.PrepaymentDesignPaymentItem> PrepaymentDesignPaymentItems { get; set; }
        public DbSet<DomainClasses.FinalDesignPaymentItem> FinalDesignPaymentItems { get; set; }


        public DbSet<DomainClasses.FinancialConflict> FinancialConflicts { get; set; }
        public DbSet<DomainClasses.FinancialConflictItem> FinancialConflictItems { get; set; }

        public DbSet<DomainClasses.PriceList> PriceLists { get; set; }


        public DbSet<DomainClasses.Message> Messages { get; set; }
        public DbSet<DomainClasses.CustomerMessage> CustomerMessages { get; set; }
        public DbSet<DomainClasses.AdminMessage_Admin> AdminMessages_Admin { get; set; }
        public DbSet<DomainClasses.AdminMessage_Customer> AdminMessages_Customer { get; set; }


        public DbSet<DomainClasses.PortfolioType> PortfolioTypes { get; set; }
        public DbSet<DomainClasses.PortfolioCategory> PortfolioCategories { get; set; }
        public DbSet<DomainClasses.Portfolio> Portfolios { get; set; }
        public DbSet<DomainClasses.PublicPriceCategory> PublicPriceCategories { get; set; }
        public DbSet<DomainClasses.PublicPrice> PublicPrices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProvinceConfiguration());
            modelBuilder.Configurations.Add(new CityConfiguration());

            modelBuilder.Configurations.Add(new CustomerGroupConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());

            modelBuilder.Configurations.Add(new FormGroupConfiguration());
            modelBuilder.Configurations.Add(new FormConfiguration());
            modelBuilder.Configurations.Add(new FormFieldConfiguration());
            modelBuilder.Configurations.Add(new FormField_FileUploaderConfiguration());
            modelBuilder.Configurations.Add(new FileFormatConfiguration());
            modelBuilder.Configurations.Add(new FormField_DropDownConfiguration());
            modelBuilder.Configurations.Add(new FormField_DropDown_ItemConfiguration());
            modelBuilder.Configurations.Add(new FormField_RadioButtonGroupConfiguration());
            modelBuilder.Configurations.Add(new FormField_RadioButtonGroup_ItemConfiguration());
            modelBuilder.Configurations.Add(new FormField_CheckBoxGroupConfiguration());
            modelBuilder.Configurations.Add(new FormField_CheckBoxGroup_ItemConfiguration());

            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new PrintOrderConfiguration());
            modelBuilder.Configurations.Add(new DesignOrderConfiguration());
            modelBuilder.Configurations.Add(new DesignOrder_DesignConfiguration());
            modelBuilder.Configurations.Add(new DesignOrder_Design_FileConfiguration());
            modelBuilder.Configurations.Add(new Order_ValueConfiguration());
            modelBuilder.Configurations.Add(new Order_Value_DropDownConfiguration());
            modelBuilder.Configurations.Add(new Order_Value_RadioButtonGroupConfiguration());
            modelBuilder.Configurations.Add(new Order_Value_CheckboxGroupConfiguration());

            modelBuilder.Configurations.Add(new PrintFactorConfiguration());
            modelBuilder.Configurations.Add(new PrepaymentDesignFactorConfiguration());
            modelBuilder.Configurations.Add(new FinalDesignFactorConfiguration());
            modelBuilder.Configurations.Add(new PaymentConfiguration());
            modelBuilder.Configurations.Add(new PrintPaymentConfiguration());
            modelBuilder.Configurations.Add(new PrintPaymentItemConfiguration());
            modelBuilder.Configurations.Add(new DesignPaymentConfiguration());
            modelBuilder.Configurations.Add(new PrepaymentDesignPaymentItemConfiguration());
            modelBuilder.Configurations.Add(new FinalDesignPaymentItemConfiguration());

            modelBuilder.Configurations.Add(new FinancialConflictConfiguration());
            modelBuilder.Configurations.Add(new FinancialConflictItemConfiguration());

            modelBuilder.Configurations.Add(new CustomerMessageConfiguration());
            modelBuilder.Configurations.Add(new AdminMessage_AdminConfiguration());
            modelBuilder.Configurations.Add(new AdminMessage_CustomerConfiguration());

            modelBuilder.Configurations.Add(new PortfolioTypeConfiguration());
            modelBuilder.Configurations.Add(new PortfolioCategoryConfiguration());
            modelBuilder.Configurations.Add(new PortfolioConfiguration());
            modelBuilder.Configurations.Add(new PublicPriceCategoryConfiguration());
            modelBuilder.Configurations.Add(new PublicPriceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

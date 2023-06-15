using System;
using System.Linq;
using DataLayer.FunctionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class WASContext : DbContext
    {
        public WASContext()
        {
        }

        public WASContext(DbContextOptions<WASContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Access> Accesses { get; set; }
        public virtual DbSet<AllExpression> AllExpressions { get; set; }
        public virtual DbSet<Ambulance> Ambulances { get; set; }
        public virtual DbSet<Analysis> Analyses { get; set; }
        public virtual DbSet<AnalysisAnalysisItem> AnalysisAnalysisItems { get; set; }
        public virtual DbSet<AnalysisItem> AnalysisItems { get; set; }
        public virtual DbSet<AnalysisItemMinMaxValue> AnalysisItemMinMaxValues { get; set; }
        public virtual DbSet<AnalysisItemValuesRange> AnalysisItemValuesRanges { get; set; }
        public virtual DbSet<AnalysisResult> AnalysisResults { get; set; }
        public virtual DbSet<AnalysisResultMaster> AnalysisResultMasters { get; set; }
        public virtual DbSet<AnalysisResultTemplate> AnalysisResultTemplates { get; set; }
        public virtual DbSet<BaseInfo> BaseInfos { get; set; }
        public virtual DbSet<BaseInfoGeneral> BaseInfoGenerals { get; set; }
        public virtual DbSet<BaseInfoGeneralType> BaseInfoGeneralTypes { get; set; }
        public virtual DbSet<BaseInfoSectionType> BaseInfoSectionTypes { get; set; }
        public virtual DbSet<BaseInfoType> BaseInfoTypes { get; set; }
        public virtual DbSet<Child> Children { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<ClinicSection> ClinicSections { get; set; }
        public virtual DbSet<ClinicSectionChoosenValue> ClinicSectionChoosenValues { get; set; }
        public virtual DbSet<ClinicSectionSetting> ClinicSectionSettings { get; set; }
        public virtual DbSet<ClinicSectionSettingValue> ClinicSectionSettingValues { get; set; }
        public virtual DbSet<ClinicSectionUser> ClinicSectionUsers { get; set; }
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Damage> Damages { get; set; }
        public virtual DbSet<DamageDetail> DamageDetails { get; set; }
        public virtual DbSet<DamageDiscount> DamageDiscounts { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<DiseaseSymptom> DiseaseSymptoms { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Emergency> Emergencies { get; set; }
        public virtual DbSet<GroupAnalysis> GroupAnalyses { get; set; }
        public virtual DbSet<GroupAnalysisAnalysis> GroupAnalysisAnalyses { get; set; }
        public virtual DbSet<GroupAnalysisItem> GroupAnalysisItems { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageExpression> LanguageExpressions { get; set; }
        public virtual DbSet<LicenceKey> LicenceKeys { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineDisease> MedicineDiseases { get; set; }
        public virtual DbSet<MoneyConvert> MoneyConverts { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientDiseaseRecord> PatientDiseaseRecords { get; set; }
        public virtual DbSet<PatientImage> PatientImages { get; set; }
        public virtual DbSet<PatientMedicineRecord> PatientMedicineRecords { get; set; }
        public virtual DbSet<PatientReception> PatientReceptions { get; set; }
        public virtual DbSet<PatientReceptionAnalysis> PatientReceptionAnalyses { get; set; }
        public virtual DbSet<PatientVariable> PatientVariables { get; set; }
        public virtual DbSet<PatientVariablesValue> PatientVariablesValues { get; set; }
        public virtual DbSet<Pay> Pays { get; set; }
        public virtual DbSet<PayAmount> PayAmounts { get; set; }
        public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }
        public virtual DbSet<PrescriptionTestDetail> PrescriptionTestDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; } 
        public virtual DbSet<ProductBarcode> ProductBarcodes { get; set; }
        public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public virtual DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public virtual DbSet<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePrices { get; set; }
        public virtual DbSet<PurchaseInvoiceDiscount> PurchaseInvoiceDiscounts { get; set; }
        public virtual DbSet<PurchaseInvoicePay> PurchaseInvoicePays { get; set; }
        public virtual DbSet<Receive> Receives { get; set; }
        public virtual DbSet<ReceiveAmount> ReceiveAmounts { get; set; }
        public virtual DbSet<Reception> Receptions { get; set; }
        public virtual DbSet<ReceptionAmbulance> ReceptionAmbulances { get; set; }
        public virtual DbSet<ReceptionClinicSection> ReceptionClinicSections { get; set; }
        public virtual DbSet<ReceptionDetailPay> ReceptionDetailPays { get; set; }

        public virtual DbSet<ReceptionDoctor> ReceptionDoctors { get; set; }
        public virtual DbSet<ReceptionInsurance> ReceptionInsurances { get; set; }
        public virtual DbSet<ReceptionInsuranceReceived> ReceptionInsuranceReceiveds { get; set; }
        public virtual DbSet<ReceptionRoomBed> ReceptionRoomBeds { get; set; }
        public virtual DbSet<ReceptionService> ReceptionServices { get; set; }
        public virtual DbSet<ReceptionServiceReceived> ReceptionServiceReceiveds { get; set; }
        public virtual DbSet<ReceptionTemperature> ReceptionTemperatures { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<Reserve> Reserves { get; set; }
        public virtual DbSet<ReserveDetail> ReserveDetails { get; set; }
        public virtual DbSet<ReturnPurchaseInvoice> ReturnPurchaseInvoices { get; set; }
        public virtual DbSet<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetails { get; set; }
        public virtual DbSet<ReturnPurchaseInvoiceDiscount> ReturnPurchaseInvoiceDiscounts { get; set; }
        public virtual DbSet<ReturnPurchaseInvoicePay> ReturnPurchaseInvoicePays { get; set; }
        public virtual DbSet<ReturnSaleInvoice> ReturnSaleInvoices { get; set; }
        public virtual DbSet<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetails { get; set; }
        public virtual DbSet<ReturnSaleInvoiceDiscount> ReturnSaleInvoiceDiscounts { get; set; }
        public virtual DbSet<ReturnSaleInvoiceReceive> ReturnSaleInvoiceReceives { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomBed> RoomBeds { get; set; }
        public virtual DbSet<RoomItem> RoomItems { get; set; }
        public virtual DbSet<SaleInvoice> SaleInvoices { get; set; }
        public virtual DbSet<SaleInvoiceCost> SaleInvoiceCosts { get; set; }
        public virtual DbSet<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
        public virtual DbSet<SaleInvoiceDiscount> SaleInvoiceDiscounts { get; set; }
        public virtual DbSet<SaleInvoiceReceive> SaleInvoiceReceives { get; set; }
        public virtual DbSet<Secretary> Secretaries { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SettingValueOfSetting> SettingValueOfSettings { get; set; }
        public virtual DbSet<SoftwareSetting> SoftwareSettings { get; set; }
        public virtual DbSet<SubSystem> SubSystems { get; set; }
        public virtual DbSet<SubSystemAccess> SubSystemAccesses { get; set; }
        public virtual DbSet<SubSystemSection> SubSystemSections { get; set; }
        public virtual DbSet<Surgery> Surgeries { get; set; }
        public virtual DbSet<SurgeryDoctor> SurgeryDoctors { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Symptom> Symptoms { get; set; }
        public virtual DbSet<TmpVisit> TmpVisits { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }
        public virtual DbSet<TransferDetail> TransferDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPortion> UserPortions { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserSubSystemAccess> UserSubSystemAccesses { get; set; }
        public virtual DbSet<ValueOfSetting> ValueOfSettings { get; set; }
        public virtual DbSet<VisitPatientDisease> VisitPatientDiseases { get; set; }
        public virtual DbSet<VisitSymptom> VisitSymptoms { get; set; }
        public virtual DbSet<HumanResource> HumanResources { get; set; }
        public virtual DbSet<HumanResourceSalary> HumanResourceSalaries { get; set; }
        public virtual DbSet<HumanResourceSalaryPayment> HumanResourceSalaryPayments { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {




        //            if (!optionsBuilder.IsConfigured)
        //            {

        //                //string connString = this.Configuration.GetConnectionString("MyConn");
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more Guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=DESKTOP-PGSD8J6\\MSSQLSERVER2021;Database=MokhtabarNew4;Trusted_Connection=True;MultipleActiveResultSets=true");
        //                //optionsBuilder.UseSqlServer("Server=DESKTOP-15ODL0F\\SQLSERVER2019;Database=EmptyMokhtabar;User Id = sa; password = AZAD12345; Trusted_Connection = False; MultipleActiveResultSets = true;");

        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Access>(entity =>
            {
                entity.ToTable("Access");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

            });

            modelBuilder.Entity<AllExpression>(entity =>
            {
                entity.ToTable("AllExpression");

                entity.HasIndex(e => e.ExpressionText, "UC_Expression")
                    .IsUnique();

                entity.Property(e => e.ExpressionText)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Ambulance>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Ambulance");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Ambulances)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK_Ambulance_Hospital");
            });

            modelBuilder.Entity<Analysis>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_analysis");

                entity.ToTable("Analysis");

                //entity.HasIndex(e => e.Guid, "idx_Guid");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.AnalysisCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_CreatedUser");

                entity.HasOne(d => d.DiscountCurrency)
                    .WithMany(p => p.Analyses)
                    .HasForeignKey(d => d.DiscountCurrencyId)
                    .HasConstraintName("FK_DiscountCurrency");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.AnalysisModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ModifiedUser");
            });


            modelBuilder.Entity<AnalysisAnalysisItem>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Analysis_AnalysisItem");

                //entity.HasIndex(e => new { e.AnalysisId, e.AnalysisItemId }, "idx_Guid");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Analysis)
                    .WithMany(p => p.AnalysisAnalysisItems)
                    .HasForeignKey(d => d.AnalysisId)
                    .HasConstraintName("FK_Analysis_AnalysisItem_Analysis");

                entity.HasOne(d => d.AnalysisItem)
                    .WithMany(p => p.AnalysisAnalysisItems)
                    .HasForeignKey(d => d.AnalysisItemId)
                    .HasConstraintName("FK_Analysis_AnalysisItem_AnalysisItem");
            });

            modelBuilder.Entity<AnalysisItem>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("AnalysisItem");

                //entity.HasIndex(e => e.Guid, "idx_Guid");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.Amount).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.NormalValues).HasMaxLength(1000);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.HasOne(d => d.AmountCurrency)
                    .WithMany(p => p.AnalysisItemAmountCurrencies)
                    .HasForeignKey(d => d.AmountCurrencyId)
                    .HasConstraintName("FK_CurrencyId");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.AnalysisItems)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_AnalysisItem_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.AnalysisItemCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_CreatedUser_User");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.AnalysisItemModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ModifiedUser_User");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.AnalysisItems)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_AnalysisItem_BaseInfo");

                entity.HasOne(d => d.ValueType)
                    .WithMany(p => p.AnalysisItemValueTypes)
                    .HasForeignKey(d => d.ValueTypeId)
                    .HasConstraintName("FK_AnalysisItem_GeneralBaseInfoValueTypeId");
            });

            modelBuilder.Entity<AnalysisItemMinMaxValue>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BMaxValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("B_maxValue");

                entity.Property(e => e.BMinValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("B_minValue");

                entity.Property(e => e.CMaxValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("C_maxValue");

                entity.Property(e => e.CMinValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("C_minValue");

                entity.Property(e => e.FMaxValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("F_maxValue");

                entity.Property(e => e.FMinValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("F_minValue");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.MMaxValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("M_maxValue");

                entity.Property(e => e.MMinValue)
                    .HasColumnType("numeric(20, 2)")
                    .HasColumnName("M_minValue");

                entity.HasOne(d => d.AnalysisItem)
                    .WithMany(p => p.AnalysisItemMinMaxValues)
                    .HasForeignKey(d => d.AnalysisItemId)
                    .HasConstraintName("FK_AnalysisItemMinMaxValues");
            });

            modelBuilder.Entity<AnalysisItemValuesRange>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_tblAnalysisItemValuesRange");

                entity.ToTable("AnalysisItemValuesRange");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Value).HasMaxLength(100);

                entity.HasOne(d => d.AnalysisItem)
                    .WithMany(p => p.AnalysisItemValuesRanges)
                    .HasForeignKey(d => d.AnalysisItemId)
                    .HasConstraintName("FK_AnalysisItemValuesRange");
            });

            modelBuilder.Entity<AnalysisResult>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("AnalysisResult");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Value).HasMaxLength(500);

                entity.HasOne(d => d.Analysis)
                    .WithMany(p => p.AnalysisResults)
                    .HasForeignKey(d => d.AnalysisId)
                    .HasConstraintName("FK_AnalysisResult_Analysis");

                entity.HasOne(d => d.AnalysisItem)
                    .WithMany(p => p.AnalysisResults)
                    .HasForeignKey(d => d.AnalysisItemId)
                    .HasConstraintName("FK_AnalysisResult_AnalysisItem");

                entity.HasOne(d => d.AnalysisResultMaster)
                    .WithMany(p => p.AnalysisResults)
                    .HasForeignKey(d => d.AnalysisResultMasterId)
                    .HasConstraintName("FK_AnalysisResult_AnalysisResultMaster");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.AnalysisResultCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_AnalysisResult_User");

                entity.HasOne(d => d.GroupAnalysis)
                    .WithMany(p => p.AnalysisResults)
                    .HasForeignKey(d => d.GroupAnalysisId)
                    .HasConstraintName("FK_AnalysisResult_GroupAnalysis");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.AnalysisResultModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_AnalysisResult_User1");
            });

            modelBuilder.Entity<AnalysisResultMaster>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("AnalysisResultMaster");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                   .WithMany(p => p.AnalysisResultMasterCreatedUsers)
                   .HasForeignKey(d => d.CreatedUserId)
                   .HasConstraintName("FK_AnalysisResultMaster_User");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.AnalysisResultMasterModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_AnalysisResultMaster_User1");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.AnalysisResultMasters)
                    .HasForeignKey(d => d.ReceptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalysisResultMaster_PatientReception");
            });

            modelBuilder.Entity<AnalysisResultTemplate>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("AnalysisResultTemplate");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.AnalysisResultTemplates)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_AnalysisResultTemplate_ClinicSection");
            });

            modelBuilder.Entity<BaseInfo>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("BaseInfo");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.BaseInfos)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_BaseInfo_ClinicSection");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.BaseInfos)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_BaseInfo_BaseInfoType");
            });

            modelBuilder.Entity<BaseInfoGeneral>(entity =>
            {
                entity.ToTable("BaseInfoGeneral", "dbo");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.BaseInfoGenerals)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_BaseInfoGeneral_BaseInfoGeneralType");
            });

            modelBuilder.Entity<BaseInfoGeneralType>(entity =>
            {
                entity.ToTable("BaseInfoGeneralType");

                entity.Property(e => e.Ename)
                    .HasMaxLength(150)
                    .HasColumnName("EName");

                entity.Property(e => e.Fname)
                    .HasMaxLength(150)
                    .HasColumnName("FName");
            });

            modelBuilder.Entity<BaseInfoSectionType>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("BaseInfoSectionType");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.BaseInfoType)
                    .WithMany(p => p.BaseInfoSectionTypes)
                    .HasForeignKey(d => d.BaseInfoTypeId)
                    .HasConstraintName("FK_BaseInfoSectionType_BaseInfo");

                entity.HasOne(d => d.SectionType)
                    .WithMany(p => p.BaseInfoSectionTypes)
                    .HasForeignKey(d => d.SectionTypeId)
                    .HasConstraintName("FK_BaseInfoSectionType_BaseInfoGeneral");
            });

            modelBuilder.Entity<BaseInfoType>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("BaseInfoType");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Ename)
                    .HasMaxLength(150)
                    .HasColumnName("EName");

                entity.Property(e => e.Fname)
                    .HasMaxLength(150)
                    .HasColumnName("FName");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });

            modelBuilder.Entity<Child>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Child", "dbo");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CongenitalAnomalies).HasMaxLength(500);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NeedOperation).HasDefaultValueSql("((0))");

                entity.Property(e => e.OperationOrder).HasMaxLength(500);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.VitalActivities).HasMaxLength(500);

                entity.Property(e => e.Weight)
                    .HasColumnType("numeric(6, 3)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ChildStatus)
                    .HasConstraintName("FK_Child_BaseInfoGeneral");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.ChildCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_Child_UserCreate");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_Child_Doctor");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Child)
                    .HasForeignKey<Child>(d => d.Guid)
                    .HasConstraintName("FK_Child_User");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ChildModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Child_UserModify");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_Child_Reception");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Child_Room");
            });

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Clinic");

                entity.HasIndex(e => e.SystemCode, "UC_ClinicCode")
                    .IsUnique();

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(50)
                    .HasColumnName("licenseNumber");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber1).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber2).HasMaxLength(50);

                entity.Property(e => e.SystemCode).HasMaxLength(50);
            });

            modelBuilder.Entity<ClinicSection>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ClinicSection", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(50)
                    .HasColumnName("licenseNumber");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Priority);

                entity.Property(e => e.SystemCode).HasMaxLength(50);

                entity.HasOne(d => d.Clinic)
                    .WithMany(p => p.ClinicSections)
                    .HasForeignKey(d => d.ClinicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClinicSection_Clinic");

                entity.HasOne(d => d.ClinicSectionShowType)
                    .WithMany(p => p.ClinicSectionClinicSectionShowTypes)
                    .HasForeignKey(d => d.ClinicSectionShowTypeId)
                    .HasConstraintName("FK_ClinicSection_ClinicSectionShowType");

                entity.HasOne(d => d.ClinicSectionType)
                    .WithMany(p => p.ClinicSectionClinicSectionTypes)
                    .HasForeignKey(d => d.ClinicSectionTypeId)
                    .HasConstraintName("FK_ClinicSection_BaseInfoGeneral1");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_ClinicSection_ClinicSection");

                entity.HasOne(d => d.SectionType)
                    .WithMany(p => p.ClinicSectionSectionTypes)
                    .HasForeignKey(d => d.SectionTypeId)
                    .HasConstraintName("FK_ClinicSection_BaseInfoGeneral");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.ClinicSections)
                    .HasForeignKey(d => d.SpecialityId)
                    .HasConstraintName("FK_ClinicSection_BaseInfo");
            });

            modelBuilder.Entity<ClinicSectionChoosenValue>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_ClinicSectionChoosenValue");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.ClinicSectionChoosenValues)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_ClinicSectionChoosenValue_ClinicSection");

                entity.HasOne(d => d.PatientVariable)
                    .WithMany(p => p.ClinicSectionChoosenValues)
                    .HasForeignKey(d => d.PatientVariableId)
                    .HasConstraintName("FK_ClinicSectionChoosenValue_PatientVariables");

                entity.HasOne(d => d.VariableDisplay)
                    .WithMany(p => p.ClinicSectionChoosenValueVariableDisplays)
                    .HasForeignKey(d => d.VariableDisplayId)
                    .HasConstraintName("FK_ClinicSectionChoosenValue_BaseInfoGeneral");

                entity.HasOne(d => d.VariableStatus)
                    .WithMany(p => p.ClinicSectionChoosenValueVariableStatuses)
                    .HasForeignKey(d => d.VariableStatusId)
                    .HasConstraintName("FK_ClinicSectionChoosenValue_BaseInfoGeneral1");
            });

            modelBuilder.Entity<ClinicSectionSetting>(entity =>
            {
                entity.ToTable("ClinicSectionSetting");

                entity.Property(e => e.InputType).HasMaxLength(250);

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("SName");

                entity.HasOne(d => d.SectionType)
                    .WithMany(p => p.ClinicSectionSettings)
                    .HasForeignKey(d => d.SectionTypeId)
                    .HasConstraintName("FK_ClinicSectionSetting_BaseInfoGeneral");
            });

            modelBuilder.Entity<ClinicSectionSettingValue>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ClinicSectionSettingValue");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Svalue)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("SValue");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.ClinicSectionSettingValues)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClinicSectionSettingValue_ClinicSection");

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.ClinicSectionSettingValues)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClinicSectionSettingValue_ClinicSectionSetting");
            });

            modelBuilder.Entity<ClinicSectionUser>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ClinicSection_User");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.ClinicSectionUsers)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClinicSection_User_Clinic");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClinicSectionUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClinicSection_User_User");
            });

            modelBuilder.Entity<Cost>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Cost");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CostDate).HasColumnType("datetime");

                entity.Property(e => e.Explanation).HasMaxLength(250);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Cost_ClinicSection");

                entity.HasOne(d => d.CostType)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.CostTypeId)
                    .HasConstraintName("FK_Cost_Currency");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_Cost_BaseInfoGeneral");

                entity.HasOne(d => d.PurchaseInvoice)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.PurchaseInvoiceId)
                    .HasConstraintName("FK_Costs_PurchaseInvoiceId");

                entity.HasOne(d => d.SaleInvoice)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.SaleInvoiceId)
                    .HasConstraintName("FK_Costs_SaleInvoiceId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Cost_User");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Customer");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CustomerCities)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Customer_BaseInfo1");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Customer_ClinicSection");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.CustomerCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_Customer_CreateUser");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.CustomerTypes)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_Customer_BaseInfo");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.Guid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_User");

                entity.HasOne(d => d.ModidiedUser)
                    .WithMany(p => p.CustomerModidiedUsers)
                    .HasForeignKey(d => d.ModidiedUserId)
                    .HasConstraintName("FK_Customer_ModidiedUser");
            });

            modelBuilder.Entity<Damage>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Damage");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasMaxLength(150);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Damages)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Damage_ClinicSection");

                entity.HasOne(d => d.CostType)
                    .WithMany(p => p.DamageCostTypes)
                    .HasForeignKey(d => d.CostTypeId)
                    .HasConstraintName("FK_Damage_CostTypeBaseInfo");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.DamageCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Damage_CreateUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.DamageModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Damage_ModifiedUser");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.DamageReasons)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_Damage_ReasonBaseInfo");
            });

            modelBuilder.Entity<DamageDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.DamageDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_DamageDetails_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.DamageDetails)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_DamageDetails_BaseInfoGeneral");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.DamageDetails)
                    .HasForeignKey(d => d.MasterId)
                    .HasConstraintName("FK_DamageDetails_Damage");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.DamageDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_DamageDetails_ModifiedUser");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DamageDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_DamageDetails_Product");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.DamageDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .HasConstraintName("FK_DamageDetails_PurchaseInvoiceDetails");

                entity.HasOne(d => d.TransferDetail)
                    .WithMany(p => p.DamageDetails)
                    .HasForeignKey(d => d.TransferDetailId)
                    .HasConstraintName("FK_DamageDetails_TransferDetail");
            });

            modelBuilder.Entity<DamageDiscount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("DamageDiscount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.DamageDiscountCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_DamageDiscount_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.DamageDiscounts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_DamageDiscount_BaseInfoGeneral");

                entity.HasOne(d => d.Damage)
                    .WithMany(p => p.DamageDiscounts)
                    .HasForeignKey(d => d.DamageId)
                    .HasConstraintName("FK_DamageDiscount_Damage");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.DamageDiscountModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_DamageDiscount_ModifiedUser");
            });

            modelBuilder.Entity<Disease>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Disease");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Diseases)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Disease_ClinicSection");

                entity.HasOne(d => d.DiseaseType)
                    .WithMany(p => p.Diseases)
                    .HasForeignKey(d => d.DiseaseTypeId)
                    .HasConstraintName("FK_Disease_BaseInfoGeneral");
            });

            modelBuilder.Entity<DiseaseSymptom>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Disease_Symptom");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Explanation).HasMaxLength(400);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.DiseaseSymptoms)
                    .HasForeignKey(d => d.DiseaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Disease_Symptom_Disease");

                entity.HasOne(d => d.Symptom)
                    .WithMany(p => p.DiseaseSymptoms)
                    .HasForeignKey(d => d.SymptomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Disease_Symptom_Symptom");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Doctor");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LogoAddress).HasMaxLength(500);

                entity.Property(e => e.MedicalSystemCode).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Doctor_ClinicSection");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.Guid)
                    .HasConstraintName("FK_Doctor_Inherits_User");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.SpecialityId)
                    .HasConstraintName("FK_Doctor_BaseInfo");


            });

            modelBuilder.Entity<Emergency>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Emergency");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Arrival)
                    .WithMany(p => p.EmergencyArrivals)
                    .HasForeignKey(d => d.ArrivalId)
                    .HasConstraintName("FK_Emergency_BaseInfoGeneral");

                entity.HasOne(d => d.Critically)
                    .WithMany(p => p.EmergencyCriticallies)
                    .HasForeignKey(d => d.CriticallyId)
                    .HasConstraintName("FK_Emergency_BaseInfoGeneral1");

                entity.HasOne(d => d.Reception)
                    .WithOne(p => p.Emergency)
                    .HasForeignKey<Emergency>(d => d.Guid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Emergency_Reception");
            });

            modelBuilder.Entity<GroupAnalysis>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("GroupAnalysis");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.GroupAnalysisCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_CreateUser_GroupAnalysis");

                entity.HasOne(d => d.DiscountCurrency)
                    .WithMany(p => p.GroupAnalyses)
                    .HasForeignKey(d => d.DiscountCurrencyId)
                    .HasConstraintName("FK_DiscountCurrency_GroupAnalysis");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.GroupAnalysisModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ModifiedUser_GroupAnalysis");
            });

            modelBuilder.Entity<GroupAnalysisAnalysis>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("GroupAnalysis_Analysis");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Priority).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.Analysis)
                    .WithMany(p => p.GroupAnalysisAnalyses)
                    .HasForeignKey(d => d.AnalysisId)
                    .HasConstraintName("FK_GroupAnalysis_Analysis_Analysis");

                entity.HasOne(d => d.GroupAnalysis)
                    .WithMany(p => p.GroupAnalysisAnalyses)
                    .HasForeignKey(d => d.GroupAnalysisId)
                    .HasConstraintName("FK_GroupAnalysis_Analysis_GroupAnalysis");
            });

            modelBuilder.Entity<GroupAnalysisItem>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("GroupAnalysisItem");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Priority).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.AnalysisItem)
                    .WithMany(p => p.GroupAnalysisItems)
                    .HasForeignKey(d => d.AnalysisItemId)
                    .HasConstraintName("FK_AnalysisItem");

                entity.HasOne(d => d.GroupAnalysis)
                    .WithMany(p => p.GroupAnalysisItems)
                    .HasForeignKey(d => d.GroupAnalysisId)
                    .HasConstraintName("FK_GroupAnalysisItem");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Hospital");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LanguageName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<LanguageExpression>(entity =>
            {
                entity.ToTable("LanguageExpression");

                entity.Property(e => e.ExpressionEquivalent)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Expression)
                    .WithMany(p => p.LanguageExpressions)
                    .HasForeignKey(d => d.ExpressionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageExpression_Expression");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LanguageExpressions)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageExpression_Language");
            });

            modelBuilder.Entity<LicenceKey>(entity =>
            {
                entity.ToTable("LicenceKeys", "dbo");

                entity.Property(e => e.ComputerSerial).HasMaxLength(450);

                entity.Property(e => e.SerialKey).IsRequired();
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Medicine");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.MedNum).HasColumnType("decimal(20, 7)");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Medicine_ClinicSection");

                entity.HasOne(d => d.MedicineForm)
                    .WithMany(p => p.MedicineMedicineForms)
                    .HasForeignKey(d => d.MedicineFormId)
                    .HasConstraintName("FK_Medicine_BaseInfoMedForm");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.MedicineProducers)
                    .HasForeignKey(d => d.ProducerId)
                    .HasConstraintName("FK_Medicine_BaseInfoProducer");
            });

            modelBuilder.Entity<MedicineDisease>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Medicine_Disease");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.MedicineDiseases)
                    .HasForeignKey(d => d.DiseaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicine_Disease_Disease");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.MedicineDiseases)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicine_Disease_Medicine");
            });

            modelBuilder.Entity<MoneyConvert>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("MoneyConvert");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BaseAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DestAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.BaseCurrency)
                    .WithMany(p => p.MoneyConvertBaseCurrencies)
                    .HasForeignKey(d => d.BaseCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MoneyConvert_BaseInfoGeneral");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.MoneyConverts)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MoneyConvert_ClinicSection");

                entity.HasOne(d => d.DestCurrency)
                    .WithMany(p => p.MoneyConvertDestCurrencies)
                    .HasForeignKey(d => d.DestCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MoneyConvert_BaseInfoGeneral1");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Patient");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FileNum).HasMaxLength(250);

                entity.Property(e => e.MotherName).HasMaxLength(50);

                entity.Property(e => e.FormNumber).HasMaxLength(100);

                entity.Property(e => e.IdentityNumber).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PatientAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Patient_BaseInfo3");

                entity.HasOne(d => d.BloodType)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.BloodTypeId)
                    .HasConstraintName("FK_Patient_BaseInfoGeneral");

                entity.HasOne(d => d.FatherJob)
                    .WithMany(p => p.PatientFatherJobs)
                    .HasForeignKey(d => d.FatherJobId)
                    .HasConstraintName("FK_Patient_BaseInfo");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Patient)
                    .HasForeignKey<Patient>(d => d.Guid)
                    .HasConstraintName("FK_Patient_Inherits_User");

                entity.HasOne(d => d.MotherJob)
                    .WithMany(p => p.PatientMotherJobs)
                    .HasForeignKey(d => d.MotherJobId)
                    .HasConstraintName("FK_Patient_BaseInfo1");
            });

            modelBuilder.Entity<PatientDiseaseRecord>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_PatientDiseaseRecord_1");

                entity.ToTable("PatientDiseaseRecord");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.PatientDiseaseRecords)
                    .HasForeignKey(d => d.DiseaseId)
                    .HasConstraintName("FK_PatientDiseaseRecord_Disease");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientDiseaseRecords)
                    .HasForeignKey(d => d.Patientid)
                    .HasConstraintName("FK_PatientDiseaseRecord_Patient");
            });

            modelBuilder.Entity<PatientImage>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PatientImage", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ImageAddress).HasMaxLength(500);

                entity.Property(e => e.ImageDateTime).HasColumnType("datetime");

                entity.Property(e => e.ThumbNailAddress).HasMaxLength(500);

                entity.HasOne(d => d.AttachmentType)
                    .WithMany(p => p.PatientImages)
                    .HasForeignKey(d => d.AttachmentTypeId)
                    .HasConstraintName("FK_AttachmentTypeId_BaseInfoGeneral");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientImages)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientImage_Patient");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.PatientImages)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionId_Reception");
            });

            modelBuilder.Entity<PatientMedicineRecord>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PatientMedicineRecord");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.PatientMedicineRecords)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedicineRecord_Medicine");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientMedicineRecords)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedicineRecord_Patient");
            });

            modelBuilder.Entity<PatientReception>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PatientReception");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.BaseCurrency)
                    .WithMany(p => p.PatientReceptionBaseCurrencies)
                    .HasForeignKey(d => d.BaseCurrencyId)
                    .HasConstraintName("FK_PatientReception_BaseInfoGeneral1");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.PatientReceptions)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_PatientReception_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PatientReceptionCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_PatientReception_User");

                entity.HasOne(d => d.DiscountCurrency)
                    .WithMany(p => p.PatientReceptionDiscountCurrencies)
                    .HasForeignKey(d => d.DiscountCurrencyId)
                    .HasConstraintName("FK_PatientReception_BaseInfoGeneral");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.PatientReceptions)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientReception_Doctor");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PatientReceptionModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PatientReception_User1");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientReceptions)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientReception_Patient");
            });

            modelBuilder.Entity<PatientReceptionAnalysis>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PatientReceptionAnalysis");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.AmountCurrency)
                    .WithMany(p => p.PatientReceptionAnalyses)
                    .HasForeignKey(d => d.AmountCurrencyId)
                    .HasConstraintName("FK_PatientReceptionAnalysis_BaseInfoGeneral1");

                entity.HasOne(d => d.Analysis)
                    .WithMany(p => p.PatientReceptionAnalyses)
                    .HasForeignKey(d => d.AnalysisId)
                    .HasConstraintName("FK_PatientReceptionAnalysis_Analysis");

                entity.HasOne(d => d.AnalysisItem)
                    .WithMany(p => p.PatientReceptionAnalyses)
                    .HasForeignKey(d => d.AnalysisItemId)
                    .HasConstraintName("FK_PatientReceptionAnalysisAnalysisItem");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PatientReceptionAnalysisCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_PatientReceptionAnalysis_User");

                entity.HasOne(d => d.GroupAnalysis)
                    .WithMany(p => p.PatientReceptionAnalyses)
                    .HasForeignKey(d => d.GroupAnalysisId)
                    .HasConstraintName("FK_PatientReceptionAnalysis_GroupAnalysis");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PatientReceptionAnalysisModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PatientReceptionAnalysis_User1");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.PatientReceptionAnalyses)
                    .HasForeignKey(d => d.ReceptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientReceptionAnalysis_PatientReception");
            });

            modelBuilder.Entity<PatientVariable>(entity =>
            {

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.VariableName).HasMaxLength(500);

                entity.HasOne(d => d.VariableType)
                    .WithMany(p => p.PatientVariables)
                    .HasForeignKey(d => d.VariableTypeId)
                    .HasConstraintName("FK_PatientVariables_BaseInfoGeneral");

                entity.HasOne(d => d.VariableDisplay)
                    .WithMany(p => p.PatientVariableTypes)
                    .HasForeignKey(d => d.VariableDisplayId)
                    .HasConstraintName("FK_PatientVariables_BaseInfoGeneral1");

                entity.HasOne(d => d.VariableStatus)
                    .WithMany(p => p.PatientVariableStatuses)
                    .HasForeignKey(d => d.VariableStatusId)
                    .HasConstraintName("FK_PatientVariables_BaseInfoGeneral2");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.PatientVariables)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_PatientVariables_Doctor");

                entity.Property(e => e.VariableUnit).HasMaxLength(50);
            });

            modelBuilder.Entity<PatientVariablesValue>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_PatinetVariablesValue");

                entity.ToTable("PatientVariablesValue");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Value).HasMaxLength(500);

                entity.Property(e => e.VariableInsertedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.PatientVariablesValues)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_PatientVariablesValue_ClinicSection");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientVariablesValues)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientVariablesValue_Patient");

                entity.HasOne(d => d.PatientVariable)
                    .WithMany(p => p.PatientVariablesValues)
                    .HasForeignKey(d => d.PatientVariableId)
                    .HasConstraintName("FK_PatientVariablesValue_PatientVariables");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.PatientVariablesValues)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_PatientVariablesValue_Reception");
            });

            modelBuilder.Entity<Pay>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Pay");

                entity.HasIndex(e => new { e.InvoiceNum, e.ClinicSectionId }, "UQ_Pay_InvoiceNum_ClinicSectionId")
                    .IsUnique();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.MainInvoiceNum).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Pays)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Pay_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PayCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Pay_CreateUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PayModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Pay_ModifiedUser");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Pays)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Pay_Supplier");
            });

            modelBuilder.Entity<PayAmount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PayAmount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.BaseAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.DestAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.BaseCurrency)
                    .WithMany(p => p.PayAmountBaseCurrencies)
                    .HasForeignKey(d => d.BaseCurrencyId)
                    .HasConstraintName("FK_PayAmount_Base_BaseInfoGeneral");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PayAmountCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_PayAmount_BaseInfoGeneral");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.PayAmounts)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayAmount_Pay");
            });

            modelBuilder.Entity<PrescriptionDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PrescriptionDetail");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ConsumptionInstruction).HasMaxLength(250);

                entity.Property(e => e.Explanation).HasMaxLength(400);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Num).HasMaxLength(250);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.PrescriptionDetails)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDetail_ClinicSection");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.PrescriptionDetails)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDetail_Medicine");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.PrescriptionDetails)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_PrescriptionDetail_Reception");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PrescriptionDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_PrescriptionDetail_CreatedUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PrescriptionDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PrescriptionDetail_ModifiedUser");
            });

            modelBuilder.Entity<PrescriptionTestDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PrescriptionTestDetail");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AnalysisName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.PrescriptionTestDetails)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionTestDetail_ClinicSection");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.PrescriptionTestDetails)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_PrescriptionTestDetail_Reception");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.PrescriptionTestDetails)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_PrescriptionTestDetail_BaseInfo");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PrescriptionTestDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_PrescriptionTestDetail_CreatedUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PrescriptionTestDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PrescriptionTestDetail_ModifiedUser");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Product", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.ProductLocation).HasMaxLength(200);

                entity.Property(e => e.ScientificName).HasMaxLength(200);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Product_ClinicSection");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.ProductCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCreateUser_User");

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.MaterialTypeId)
                    .HasConstraintName("FK_MaterialType_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ProductModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ProductModifiedUser_User");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.ProductProducers)
                    .HasForeignKey(d => d.ProducerId)
                    .HasConstraintName("FK_Producer_BaseInfo");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ProductProductTypes)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_ProductType_BaseInfo");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.ProductUnits)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Unit_BaseInfo");
            });

            modelBuilder.Entity<ProductBarcode>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ProductBarcode");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.ProductBarcodeCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_ProductBarcode_CreateUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ProductBarcodeModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ProductBarcode_ModifiedUser");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductBarcodes)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductBarcode_Product");
            });

            modelBuilder.Entity<PurchaseInvoice>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PurchaseInvoice");

                entity.HasIndex(e => new { e.InvoiceNum, e.ClinicSectionId }, "UQ_PurchaseInvoice_InvoiceNum_ClinicSectionId")
                    .IsUnique();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.MainInvoiceNum).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasMaxLength(150);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.PurchaseInvoices)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_PurchaseInvoice_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PurchaseInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_PurchaseInvoice_CreateUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PurchaseInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PurchaseInvoice_ModifiedUser");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.PurchaseInvoices)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_PurchaseInvoice_Supplier");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.PurchaseInvoices)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_PurchaseInvoice_BaseInfoGeneral");
            });

            modelBuilder.Entity<PurchaseInvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BujNumber).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.MiddleSellPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingNum).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SellingPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.WholeSellPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.PurchaseInvoiceDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_PurchaseInvoiceDetails_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PurchaseInvoiceDetails)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_PurchaseInvoiceDetails_BaseInfoGeneral");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.PurchaseInvoiceDetails)
                    .HasForeignKey(d => d.MasterId)
                    .HasConstraintName("FK_PurchaseInvoiceDetails_PurchaseInvoice");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PurchaseInvoiceDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PurchaseInvoiceDetails_ModifiedUser");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PurchaseInvoiceDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_PurchaseInvoiceDetails_Product");
            });

            modelBuilder.Entity<PurchaseInvoiceDetailSalePrice>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PurchaseInvoiceDetailSalePrice");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePriceCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePriceCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_Currency_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePriceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_ModifiedUser");

                entity.HasOne(d => d.MoneyConvert)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePrices)
                    .HasForeignKey(d => d.MoneyConvertId)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_MoneyConvert");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePrices)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_PurchaseInvoiceDetails");

                entity.HasOne(d => d.TransferDetail)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePrices)
                    .HasForeignKey(d => d.TransferDetailId)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_TransferDetail");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.PurchaseInvoiceDetailSalePriceTypes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_PurchaseInvoiceDetailSalePrice_Type_BaseInfoGeneral");
            });

            modelBuilder.Entity<PurchaseInvoiceDiscount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PurchaseInvoiceDiscount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.PurchaseInvoiceDiscountCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_PurchaseInvoiceDiscount_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.PurchaseInvoiceDiscounts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_PurchaseInvoiceDiscount_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PurchaseInvoiceDiscountModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PurchaseInvoiceDiscount_ModifiedUser");

                entity.HasOne(d => d.PurchaseInvoice)
                    .WithMany(p => p.PurchaseInvoiceDiscounts)
                    .HasForeignKey(d => d.PurchaseInvoiceId)
                    .HasConstraintName("FK_PurchaseInvoiceDiscount_PurchaseInvoice");
            });

            modelBuilder.Entity<PurchaseInvoicePay>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("PurchaseInvoicePay");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.PurchaseInvoicePays)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_PurchaseInvoicePay_PurchaseInvoice");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.PurchaseInvoicePays)
                    .HasForeignKey(d => d.PayId)
                    .HasConstraintName("FK_PurchaseInvoicePay_Pay");
            });

            modelBuilder.Entity<Receive>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Receive");

                entity.HasIndex(e => new { e.InvoiceNum, e.ClinicSectionId }, "UQ_Receive_InvoiceNum_ClinicSectionId")
                    .IsUnique();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Receives)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Receive_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceiveCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Receive_CreateUser");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Receives)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Receive_Customer");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReceiveModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Receive_ModifiedUser");

                entity.HasOne(d => d.SaleInvoice)
                    .WithMany(p => p.Receives)
                    .HasForeignKey(d => d.SaleInvoiceId)
                    .HasConstraintName("FK_Receive_SaleInvoice");
            });

            modelBuilder.Entity<ReceiveAmount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceiveAmount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.BaseAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.DestAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.BaseCurrency)
                    .WithMany(p => p.ReceiveAmountBaseCurrencies)
                    .HasForeignKey(d => d.BaseCurrencyId)
                    .HasConstraintName("FK_ReceiveAmount_Base_BaseInfoGeneral");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReceiveAmountCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReceiveAmount_BaseInfoGeneral");

                entity.HasOne(d => d.Receive)
                    .WithMany(p => p.ReceiveAmounts)
                    .HasForeignKey(d => d.ReceiveId)
                    .HasConstraintName("FK_ReceiveAmount_Receive");
            });

            modelBuilder.Entity<Reception>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Reception");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ChiefComplaint).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Discharge).HasDefaultValueSql("((0))");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.EntranceDate).HasColumnType("datetime");

                entity.Property(e => e.Examination).HasMaxLength(250);

                entity.Property(e => e.ExitDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PatientAttendanceName).HasMaxLength(50);

                entity.Property(e => e.PoliceReport).HasMaxLength(500);

                entity.Property(e => e.ReceptionDate).HasColumnType("datetime");

                entity.Property(e => e.ReceptionInvoiceNum).HasMaxLength(50);

                entity.Property(e => e.ReceptionNum).HasMaxLength(100);

                entity.HasOne(d => d.ClearanceType)
                    .WithMany(p => p.ReceptionClearanceTypes)
                    .HasForeignKey(d => d.ClearanceTypeId)
                    .HasConstraintName("FK_Reception_BaseInfoGeneral2");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Receptions)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Reception_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Reception_User");

                entity.HasOne(d => d.DiscountCurrency)
                    .WithMany(p => p.ReceptionDiscountCurrencies)
                    .HasForeignKey(d => d.DiscountCurrencyId)
                    .HasConstraintName("FK_Reception_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReceptionModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Reception_User1");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Receptions)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Reception_Patient");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.ReceptionPaymentStatuses)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .HasConstraintName("FK_Reception_BaseInfoGeneral3");

                entity.HasOne(d => d.Purpose)
                    .WithMany(p => p.ReceptionPurposes)
                    .HasForeignKey(d => d.PurposeId)
                    .HasConstraintName("FK_Reception_BaseInfoGeneral4");

                entity.HasOne(d => d.ReceptionType)
                    .WithMany(p => p.ReceptionReceptionTypes)
                    .HasForeignKey(d => d.ReceptionTypeId)
                    .HasConstraintName("FK_Reception_BaseInfoGeneral1");

                entity.HasOne(d => d.ReserveDetail)
                    .WithMany(p => p.Receptions)
                    .HasForeignKey(d => d.ReserveDetailId)
                    .HasConstraintName("FK_Reception_ReserveDetail");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ReceptionStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Status_BaseInfoGeneral");
            });

            modelBuilder.Entity<ReceptionAmbulance>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionAmbulance");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Cost).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Explanation).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Ambulance)
                    .WithMany(p => p.ReceptionAmbulances)
                    .HasForeignKey(d => d.AmbulanceId)
                    .HasConstraintName("FK_ReceptionAmbulance_Ambulance");

                entity.HasOne(d => d.CostCurrency)
                    .WithMany(p => p.ReceptionAmbulanceCostCurrencies)
                    .HasForeignKey(d => d.CostCurrencyId)
                    .HasConstraintName("FK_ReceptionAmbulance_BaseInfoGeneral1");

                entity.HasOne(d => d.FromHospital)
                    .WithMany(p => p.ReceptionAmbulanceFromHospitals)
                    .HasForeignKey(d => d.FromHospitalId)
                    .HasConstraintName("FK_ReceptionAmbulance_Hospital");

                entity.HasOne(d => d.PatientHealth)
                    .WithMany(p => p.ReceptionAmbulancePatientHealths)
                    .HasForeignKey(d => d.PatientHealthId)
                    .HasConstraintName("FK_ReceptionAmbulance_BaseInfoGeneral");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionAmbulances)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionAmbulance_Reception");

                entity.HasOne(d => d.ToHospital)
                    .WithMany(p => p.ReceptionAmbulanceToHospitals)
                    .HasForeignKey(d => d.ToHospitalId)
                    .HasConstraintName("FK_ReceptionAmbulance_Hospital1");
            });

            modelBuilder.Entity<ReceptionClinicSection>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionClinicSection");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.ReceptionClinicSections)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_ReceptionClinicSection_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionClinicSections)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionClinicSection_User");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionClinicSections)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionClinicSection_Reception");

                entity.HasOne(d => d.DestinationReception)
                    .WithMany(p => p.ReceptionClinicSectionDestinations)
                    .HasForeignKey(d => d.DestinationReceptionId)
                    .HasConstraintName("FK_ReceptionClinicSection_Reception1");
            });

            modelBuilder.Entity<ReceptionDetailPay>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionDetailPay");

                entity.Property(e => e.Guid).HasColumnName("GUID").HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionDetailPays)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionDetailPay_Reception");

                entity.HasOne(d => d.UserPortion)
                    .WithMany(p => p.ReceptionDetailPays)
                    .HasForeignKey(d => d.UserPortionId)
                    .HasConstraintName("FK_ReceptionDetailPay_UserPortion");
            });

            modelBuilder.Entity<ReceptionDoctor>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionDoctor", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.ReceptionDoctors)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_ReceptionDoctor_Doctor");

                entity.HasOne(d => d.DoctorRole)
                    .WithMany(p => p.ReceptionDoctors)
                    .HasForeignKey(d => d.DoctorRoleId)
                    .HasConstraintName("FK_ReceptionDoctor_BaseInfoGeneral");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionDoctors)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionDoctor_Reception");
            });

            modelBuilder.Entity<ReceptionInsurance>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionInsurance");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionInsurances)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionInsurance_User");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionInsurances)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionInsurance_Reception");
            });

            modelBuilder.Entity<ReceptionInsuranceReceived>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionInsuranceReceived");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.PayerName).HasMaxLength(250);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionInsuranceReceiveds)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionInsuranceReceived_User");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReceptionInsuranceReceiveds)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReceptionInsuranceReceived_BaseInfoGeneral");

                entity.HasOne(d => d.ReceptionInsurance)
                    .WithMany(p => p.ReceptionInsuranceReceiveds)
                    .HasForeignKey(d => d.ReceptionInsuranceId)
                    .HasConstraintName("FK_ReceptionInsuranceReceived_ReceptionInsurance");
            });

            modelBuilder.Entity<ReceptionRoomBed>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionRoomBed", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EntranceDate).HasColumnType("datetime");

                entity.Property(e => e.ExitDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionRoomBedCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionRoomBed_User");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReceptionRoomBedModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceptionRoomBed_User1");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionRoomBeds)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionRoomBed_Reception");

                entity.HasOne(d => d.RoomBed)
                    .WithMany(p => p.ReceptionRoomBeds)
                    .HasForeignKey(d => d.RoomBedId)
                    .HasConstraintName("FK_RoomBedId_RoomBed");
            });

            modelBuilder.Entity<ReceptionService>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionService", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Number).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.ServiceDate).HasColumnType("datetime");

                entity.Property(e => e.ProductIdDMS).HasColumnName("ProductIdDMS");

                entity.Property(e => e.Explanation).HasMaxLength(100);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionServices)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionService_User");

                entity.HasOne(d => d.DiscountCurrency)
                    .WithMany(p => p.ReceptionServiceDiscountCurrencies)
                    .HasForeignKey(d => d.DiscountCurrencyId)
                    .HasConstraintName("FK_ReceptionService_BaseInfoGeneral");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReceptionServices)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ReceptionService_Product");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionServices)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionService_Reception");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ReceptionServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ReceptionService_Service");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ReceptionServiceStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_StatusId_BaseInfoGeneral");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.ReceptionServices)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .HasConstraintName("FK_ReceptionService_PurchaseInvoiceDetails");

                entity.HasOne(d => d.TransferDetail)
                    .WithMany(p => p.ReceptionServices)
                    .HasForeignKey(d => d.TransferDetailId)
                    .HasConstraintName("FK_ReceptionService_TransferDetail");
            });

            modelBuilder.Entity<ReceptionServiceReceived>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionServiceReceived", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.PayerName).HasMaxLength(250);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionServiceReceiveds)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionServiceReceived_User");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReceptionServiceReceiveds)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReceptionServiceReceived_BaseInfoGeneral");

                entity.HasOne(d => d.ReceptionService)
                    .WithMany(p => p.ReceptionServiceReceiveds)
                    .HasForeignKey(d => d.ReceptionServiceId)
                    .HasConstraintName("FK_ReceptionServiceReceived_ReceptionService");
            });

            modelBuilder.Entity<ReceptionTemperature>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReceptionTemperature", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Temperature).HasColumnType("numeric(6, 2)");

                entity.Property(e => e.DIABloodPressure).HasColumnType("numeric(6, 2)");

                entity.Property(e => e.PulseRate).HasColumnType("numeric(6, 2)");

                entity.Property(e => e.RespirationRate).HasColumnType("numeric(6, 2)");

                entity.Property(e => e.SYSBloodPressure).HasColumnType("numeric(6, 2)");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.ReceptionTemperatures)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_ReceptionTemperature_Reception");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReceptionTemperatures)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReceptionTemperature_User");
            });

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Reminder", "dbo");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ReminderDate).HasColumnType("datetime");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Reminders)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Reminder_ClinicSection");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.ReminderCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_Reminder_UserCreate");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReminderModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Reminder_UserModify");
            });


            modelBuilder.Entity<Reserve>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Reserve");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EndTime).HasColumnType("time(0)");

                entity.Property(e => e.Explanation).HasMaxLength(250);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.StartTime).HasColumnType("time(0)");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Reserves)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserve_ClinicSection");
            });

            modelBuilder.Entity<ReserveDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReserveDetail");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Explanation).HasMaxLength(250);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ReserveDate).HasColumnType("datetime");

                entity.Property(e => e.ReserveEndTime).HasMaxLength(50);

                entity.Property(e => e.ReserveStartTime).HasMaxLength(50);

                entity.Property(e => e.ReservedTime).HasColumnType("time(0)");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.ReserveDetails)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_ReserveDetail_Doctor");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.ReserveDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReserveDetail_Reserve");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.ReserveDetails)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_ReserveDetail_Patient");

                entity.HasOne(d => d.Secretary)
                    .WithMany(p => p.ReserveDetails)
                    .HasForeignKey(d => d.SecretaryId)
                    .HasConstraintName("FK_ReserveDetail_Secretary");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ReserveDetails)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_ReserveDetail_BaseInfoGeneral");
            });

            modelBuilder.Entity<ReturnPurchaseInvoice>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_ReturnPurchase");

                entity.ToTable("ReturnPurchaseInvoice");

                entity.HasIndex(e => new { e.InvoiceNum, e.ClinicSectionId }, "UQ_ReturnPurchaseInvoice_InvoiceNum_ClinicSectionId")
                    .IsUnique();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasMaxLength(150);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.ReturnPurchaseInvoices)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_ReturnPurchaseInvoice_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReturnPurchaseInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReturnPurchaseInvoice_CreateUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReturnPurchaseInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ReturnPurchaseInvoice_ModifiedUser");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ReturnPurchaseInvoices)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_ReturnPurchaseInvoice_Supplier");
            });

            modelBuilder.Entity<ReturnPurchaseInvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnPurchaseInvoiceDetail");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_BaseInfoGeneral");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.MasterId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_ReturnPurchaseInvoice");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_User");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_PurchaseInvoiceDetails");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_BaseInfo");

                entity.HasOne(d => d.TransferDetail)
                    .WithMany(p => p.ReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.TransferDetailId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDetail_TransferDetail");
            });

            modelBuilder.Entity<ReturnPurchaseInvoiceDiscount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnPurchaseInvoiceDiscount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.ReturnPurchaseInvoiceDiscountCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDiscount_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReturnPurchaseInvoiceDiscounts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDiscount_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReturnPurchaseInvoiceDiscountModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDiscount_ModifiedUser");

                entity.HasOne(d => d.ReturnPurchaseInvoice)
                    .WithMany(p => p.ReturnPurchaseInvoiceDiscounts)
                    .HasForeignKey(d => d.ReturnPurchaseInvoiceId)
                    .HasConstraintName("FK_ReturnPurchaseInvoiceDiscount_ReturnPurchaseInvoiceDiscount");
            });

            modelBuilder.Entity<ReturnPurchaseInvoicePay>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnPurchaseInvoicePay");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.ReturnPurchaseInvoicePays)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_ReturnPurchaseInvoicePay_ReturnPurchaseInvoice");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.ReturnPurchaseInvoicePays)
                    .HasForeignKey(d => d.PayId)
                    .HasConstraintName("FK_ReturnPurchaseInvoicePay_Pay");
            });

            modelBuilder.Entity<ReturnSaleInvoice>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnSaleInvoice");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasMaxLength(150);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.ReturnSaleInvoices)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_ReturnSaleInvoice_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReturnSaleInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReturnSaleInvoice_CreateUser");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ReturnSaleInvoices)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ReturnSaleInvoice_Customer");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReturnSaleInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ReturnSaleInvoice_ModifiedUser");
            });

            modelBuilder.Entity<ReturnSaleInvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnSaleInvoiceDetail");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.ReturnSaleInvoiceDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDetail_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReturnSaleInvoiceDetails)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDetail_BaseInfoGeneral");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.ReturnSaleInvoiceDetails)
                    .HasForeignKey(d => d.MasterId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDetail_ReturnSaleInvoice");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReturnSaleInvoiceDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDetail_ModifiedUser");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.ReturnSaleInvoiceDetails)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDetail_BaseInfo");

                entity.HasOne(d => d.SaleInvoiceDetail)
                    .WithMany(p => p.ReturnSaleInvoiceDetails)
                    .HasForeignKey(d => d.SaleInvoiceDetailId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDetail_SaleInvoiceDetails");
            });

            modelBuilder.Entity<ReturnSaleInvoiceDiscount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnSaleInvoiceDiscount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.ReturnSaleInvoiceDiscountCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDiscount_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ReturnSaleInvoiceDiscounts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDiscount_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ReturnSaleInvoiceDiscountModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDiscount_ModifiedUser");

                entity.HasOne(d => d.ReturnSaleInvoice)
                    .WithMany(p => p.ReturnSaleInvoiceDiscounts)
                    .HasForeignKey(d => d.ReturnSaleInvoiceId)
                    .HasConstraintName("FK_ReturnSaleInvoiceDiscount_ReturnSaleInvoice");
            });

            modelBuilder.Entity<ReturnSaleInvoiceReceive>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("ReturnSaleInvoiceReceive");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.ReturnSaleInvoiceReceives)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_ReturnSaleInvoiceReceive_ReturnSaleInvoice");

                entity.HasOne(d => d.Receive)
                    .WithMany(p => p.ReturnSaleInvoiceReceives)
                    .HasForeignKey(d => d.ReceiveId)
                    .HasConstraintName("FK_ReturnSaleInvoiceReceive_Receive");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Room", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Room_ClinicSection");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RoomStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Room_BaseInfoGeneral");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.RoomTypes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Room_BaseInfoGeneral1");
            });

            modelBuilder.Entity<RoomBed>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("RoomBed", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomBeds)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_RoomBed_Room");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RoomBeds)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_RoomBed_BaseInfoGeneral");
            });

            modelBuilder.Entity<SaleInvoice>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("SaleInvoice");

                entity.HasIndex(e => new { e.InvoiceNum, e.ClinicSectionId }, "UQ_SaleInvoice_InvoiceNum_ClinicSectionId")
                    .IsUnique();

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasMaxLength(150);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.SaleInvoices)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_SaleInvoice_ClinicSection");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.SaleInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_SaleInvoice_CreateUser");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SaleInvoices)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_SaleInvoice_Customer");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.SaleInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_SaleInvoice_ModifiedUser");

                entity.HasOne(d => d.SalePriceType)
                    .WithMany(p => p.SaleInvoices)
                    .HasForeignKey(d => d.SalePriceTypeId)
                    .HasConstraintName("FK_SaleInvoice_BaseInfoGeneral");
            });

            modelBuilder.Entity<SaleInvoiceCost>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("SaleInvoiceCost");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CostDate).HasColumnType("datetime");

                entity.Property(e => e.Explanation).HasMaxLength(250);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.SaleInvoiceCosts)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_SaleInvoiceCost_ClinicSection");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SaleInvoiceCosts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_SaleInvoiceCost_BaseInfoGeneral");

                entity.HasOne(d => d.SaleInvoice)
                    .WithMany(p => p.SaleInvoiceCosts)
                    .HasForeignKey(d => d.SaleInvoiceId)
                    .HasConstraintName("FK_SaleInvoiceCost_SaleInvoice");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SaleInvoiceCosts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SaleInvoiceCost_User");
            });

            modelBuilder.Entity<SaleInvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BujNumber).HasMaxLength(50);

                entity.Property(e => e.Consideration).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingNum).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SalePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.OldSalePrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.SaleInvoiceDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_SaleInvoiceDetails_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SaleInvoiceDetails)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_SaleInvoiceDetails_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.SaleInvoiceDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_SaleInvoiceDetails_ModifiedUser");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SaleInvoiceDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_SaleInvoiceDetails_Product");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.SaleInvoiceDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .HasConstraintName("FK_SaleInvoiceDetails_PurchaseInvoiceDetails");

                entity.HasOne(d => d.SaleInvoice)
                    .WithMany(p => p.SaleInvoiceDetails)
                    .HasForeignKey(d => d.SaleInvoiceId)
                    .HasConstraintName("FK_SaleInvoiceDetails_SaleInvoice");

                entity.HasOne(d => d.TransferDetail)
                    .WithMany(p => p.SaleInvoiceDetails)
                    .HasForeignKey(d => d.TransferDetailId)
                    .HasConstraintName("FK_SaleInvoiceDetails_TransferDetail");

                entity.HasOne(d => d.MoneyConvert)
                    .WithMany(p => p.SaleInvoiceDetails)
                    .HasForeignKey(d => d.MoneyConvertId)
                    .HasConstraintName("FK_SaleInvoiceDetails_MoneyConvert");
            });

            modelBuilder.Entity<SaleInvoiceDiscount>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("SaleInvoiceDiscount");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.SaleInvoiceDiscountCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_SaleInvoiceDiscount_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SaleInvoiceDiscounts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_SaleInvoiceDiscount_BaseInfoGeneral");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.SaleInvoiceDiscountModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_SaleInvoiceDiscount_ModifiedUser");

                entity.HasOne(d => d.SaleInvoice)
                    .WithMany(p => p.SaleInvoiceDiscounts)
                    .HasForeignKey(d => d.SaleInvoiceId)
                    .HasConstraintName("FK_SaleInvoiceDiscount_SaleInvoice");
            });

            modelBuilder.Entity<SaleInvoiceReceive>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("SaleInvoiceReceive");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.SaleInvoiceReceives)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_SaleInvoiceReceive_SaleInvoice");

                entity.HasOne(d => d.Receive)
                    .WithMany(p => p.SaleInvoiceReceives)
                    .HasForeignKey(d => d.ReceiveId)
                    .HasConstraintName("FK_SaleInvoiceReceive_Receive");
            });

            modelBuilder.Entity<Secretary>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Secretary");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Gu)
                    .WithOne(p => p.Secretary)
                    .HasForeignKey<Secretary>(d => d.Guid)
                    .HasConstraintName("FK_Secretary_Inherits_User");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Secretaries)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_Secretary_BaseInfoGeneral");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Service", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.DoctorWage).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Explanation).HasMaxLength(250);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Service_ClinicSection");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.ServiceCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_Service_Currency");

                entity.HasOne(d => d.OperationType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.OperationTypeId)
                    .HasConstraintName("FK_Service_BaseInfo");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ServiceTypes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_TypeId_BaseInfoGeneral");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<SettingValueOfSetting>(entity =>
            {
                entity.ToTable("SettingValueOfSetting");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.SettingValueOfSettings)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SettingValueOfSetting_Setting");

                entity.HasOne(d => d.Value)
                    .WithMany(p => p.SettingValueOfSettings)
                    .HasForeignKey(d => d.ValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SettingValueOfSetting_ValueOfSetting");
            });

            modelBuilder.Entity<SoftwareSetting>(entity =>
            {
                entity.ToTable("SoftwareSetting");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.SettingName).HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(100);
            });

            modelBuilder.Entity<SubSystem>(entity =>
            {
                entity.ToTable("SubSystem");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Link).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ShowName).HasMaxLength(255);

                entity.HasOne(d => d.ParentRelation)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentRelationId)
                    .HasConstraintName("FK_SubSystem_SubSystem");
            });

            modelBuilder.Entity<SubSystemAccess>(entity =>
            {
                entity.ToTable("SubSystemAccess");

                entity.HasOne(d => d.Access)
                    .WithMany(p => p.SubSystemAccesses)
                    .HasForeignKey(d => d.AccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubSystemAccess_Access1");

                entity.HasOne(d => d.SubSystem)
                    .WithMany(p => p.SubSystemAccesses)
                    .HasForeignKey(d => d.SubSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubSystemAccess_SubSystem1");
            });

            modelBuilder.Entity<SubSystemSection>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("SubSystemSection");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.SectionType)
                    .WithMany(p => p.SubSystemSections)
                    .HasForeignKey(d => d.SectionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubSystemSection_BaseInfoGeneral");

                entity.HasOne(d => d.SubSystem)
                    .WithMany(p => p.SubSystemSections)
                    .HasForeignKey(d => d.SubSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubSystemSection_SubSystem");
            });

            modelBuilder.Entity<Surgery>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Surgery", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AnesthesiologistionMedicine).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExitDate).HasColumnType("datetime");

                entity.Property(e => e.Explanation).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PostOperativeTreatment).HasMaxLength(500);

                entity.Property(e => e.SideEffects).HasMaxLength(500);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.SurgeryDetail).HasMaxLength(500);

                entity.Property(e => e.SurgeryDate).HasColumnType("datetime");

                entity.HasOne(d => d.AnesthesiologistionType)
                    .WithMany(p => p.SurgeryAnesthesiologistionTypes)
                    .HasForeignKey(d => d.AnesthesiologistionTypeId)
                    .HasConstraintName("FK_Surgery_BaseInfoGeneral");

                entity.HasOne(d => d.Classification)
                    .WithMany(p => p.SurgeryClassifications)
                    .HasForeignKey(d => d.ClassificationId)
                    .HasConstraintName("FK_Surgery_BaseInfoGeneral1");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.SurgeryCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Surgery_User1");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.SurgeryModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Surgery_User");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.Surgeries)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_Surgery_Reception");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Surgeries)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Surgery_ClinicSection");

            });

            modelBuilder.Entity<SurgeryDoctor>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("SurgeryDoctor", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Explanation).HasMaxLength(250);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.SurgeryDoctors)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_SurgeryDoctor_Doctor");

                entity.HasOne(d => d.DoctorRole)
                    .WithMany(p => p.SurgeryDoctors)
                    .HasForeignKey(d => d.DoctorRoleId)
                    .HasConstraintName("FK_SurgeryDoctor_BaseInfoGeneral");

                entity.HasOne(d => d.Surgery)
                    .WithMany(p => p.SurgeryDoctors)
                    .HasForeignKey(d => d.SurgeryId)
                    .HasConstraintName("FK_SurgeryDoctor_Surgery");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Supplier");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.SupplierCities)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Supplier_BaseInfo");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Supplier_ClinicSection");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.SupplierCountries)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Supplier_BaseInfo1");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.SupplierCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Supplier_User1");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Supplier)
                    .HasForeignKey<Supplier>(d => d.Guid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_User");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.SupplierModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Supplier_User2");

                entity.HasOne(d => d.SupplierType)
                    .WithMany(p => p.SupplierSupplierTypes)
                    .HasForeignKey(d => d.SupplierTypeId)
                    .HasConstraintName("FK_Supplier_BaseInfo2");
            });

            modelBuilder.Entity<Symptom>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Symptom");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Symptoms)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Symptom_ClinicSection");
            });

            modelBuilder.Entity<TmpVisit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Tmp_Visit");

                entity.Property(e => e.BloodPressureDia)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("BloodPressureDIA");

                entity.Property(e => e.BloodPressureSys)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("BloodPressureSYS");

                entity.Property(e => e.BodyTemperature).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Explanation).HasMaxLength(400);

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.VisitDate).HasColumnType("datetime");

                entity.Property(e => e.VisitTime).HasColumnType("time(0)");
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Transfer");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiverDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiverName).HasMaxLength(150);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TransferCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_Transfer_CreateUser");

                entity.HasOne(d => d.DestinationClinicSection)
                    .WithMany(p => p.TransferDestinationClinicSections)
                    .HasForeignKey(d => d.DestinationClinicSectionId)
                    .HasConstraintName("FK_Transfer_DestinationClinicSection");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TransferModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Transfer_ModifiedUser");

                entity.HasOne(d => d.ReceiverUser)
                    .WithMany(p => p.TransferReceiverUsers)
                    .HasForeignKey(d => d.ReceiverUserId)
                    .HasConstraintName("FK_Transfer_ReceiverUser");

                entity.HasOne(d => d.SourceClinicSection)
                    .WithMany(p => p.TransferSourceClinicSections)
                    .HasForeignKey(d => d.SourceClinicSectionId)
                    .HasConstraintName("FK_Transfer_SourceClinicSection");
            });

            modelBuilder.Entity<TransferDetail>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("TransferDetail");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Consideration).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingNum).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SellingPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.MiddleSellPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.WholeSellPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TransferDetailCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_TransferDetail_CreateUser");

                entity.HasOne(d => d.DestinationProduct)
                    .WithMany(p => p.TransferDetailDestinationProducts)
                    .HasForeignKey(d => d.DestinationProductId)
                    .HasConstraintName("FK_TransferDetail_Product1");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TransferDetails)
                    .HasForeignKey(d => d.MasterId)
                    .HasConstraintName("FK_TransferDetail_Transfer");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TransferDetailModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_TransferDetail_ModifiedUser");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TransferDetailProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TransferDetail_Product");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.TransferDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .HasConstraintName("FK_TransferDetail_PurchaseInvoiceDetails");

                entity.HasOne(d => d.SourcePurchaseInvoiceDetail)
                    .WithMany(p => p.TransferDetailSourcePurchaseInvoiceDetails)
                    .HasForeignKey(d => d.SourcePurchaseInvoiceDetailId)
                    .HasConstraintName("FK_TransferDetail_SourcePurchaseInvoiceDetail");

                entity.HasOne(d => d.SourceTransferDetail)
                    .WithMany(p => p.InverseSourceTransferDetail)
                    .HasForeignKey(d => d.TransferDetailId)
                    .HasConstraintName("FK_TransferDetail_TransferDetail");

                entity.HasOne(d => d.PurchaseCurrency)
                    .WithMany(p => p.TransferDetailPurchaseCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_TransferDetail_BaseInfoGeneral");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("User");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Email).HasMaxLength(250);


                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Mobile).HasMaxLength(100);


                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pass1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Pass2).HasMaxLength(100);

                entity.Property(e => e.Pass3).HasMaxLength(250);

                entity.Property(e => e.Pass4).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AccessType)
                    .WithMany(p => p.UserAccessTypes)
                    .HasForeignKey(d => d.AccessTypeId)
                    .HasConstraintName("FK_User_AccessType");

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_User_ClinicSection");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.UserGenders)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_User_BaseInfoGeneral_Gender");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.UserUserTypes)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK_User_BaseInfoGeneral_UserType");
            });

            modelBuilder.Entity<UserPortion>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("UserPortion");

                entity.Property(e => e.Guid).HasColumnName("GUID").HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserPortion)
                    .HasForeignKey<UserPortion>(d => d.Guid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPortion_User");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("UserProfile");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_Setting");

                entity.HasOne(d => d.SettingValue)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.SettingValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_SettingValueOfSetting");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_User");
            });

            modelBuilder.Entity<UserSubSystemAccess>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK_UserSubSystemAcces");

                entity.ToTable("UserSubSystemAccess");

                entity.Property(e => e.Guid)
                    .HasColumnName("Guid")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.UserSubSystemAccesses)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_UserSubSystemAccess_ClinicSection");

                entity.HasOne(d => d.SubSystemAccess)
                    .WithMany(p => p.UserSubSystemAccesses)
                    .HasForeignKey(d => d.SubSystemAccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSubSystemAccess_SubSystemAccess");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubSystemAccesses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSubSystemAccess_User");
            });

            modelBuilder.Entity<ValueOfSetting>(entity =>
            {
                entity.ToTable("ValueOfSetting");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<VisitPatientDisease>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Visit_Patient_Disease");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Explanation).HasMaxLength(400);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.VisitPatientDiseases)
                    .HasForeignKey(d => d.DiseaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visit_Patient_Disease_Disease");

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.VisitPatientDiseases)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_Visit_Patient_Disease_Reception");
            });

            modelBuilder.Entity<VisitSymptom>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Visit_Symptom");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Explanation).HasMaxLength(400);

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Reception)
                    .WithMany(p => p.VisitSymptoms)
                    .HasForeignKey(d => d.ReceptionId)
                    .HasConstraintName("FK_Visit_Symptom_Reception");

                entity.HasOne(d => d.Symptom)
                    .WithMany(p => p.VisitSymptoms)
                    .HasForeignKey(d => d.SymptomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visit_Symptom_Symptom");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("Item", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.ClinicSection)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ClinicSectionId)
                    .HasConstraintName("FK_Item_ClinicSection");

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ItemTypeId)
                    .HasConstraintName("FK_Item_BaseInfo");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.ItemSections)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_Item_BaseInfo1");
            });


            modelBuilder.Entity<RoomItem>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("RoomItem", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.RoomItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_RoomItem_Item");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomItems)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_RoomItem_Room");
            });

            modelBuilder.Entity<HumanResource>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__HumanRes__15B69B8E7C4B96D1");

                entity.ToTable("HumanResource", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ExtraSalaryPh)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("ExtraSalaryPH");

                entity.Property(e => e.FixSalary).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.MinWorkTime).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.HumanResourceCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK__HumanReso__Curre__650CE9D0");

                entity.HasOne(d => d.Gu)
                    .WithOne(p => p.HumanResource)
                    .HasForeignKey<HumanResource>(d => d.Guid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HumanResou__GUID__62307D25");

                entity.HasOne(d => d.RoleType)
                    .WithMany(p => p.HumanResourceRoleTypes)
                    .HasForeignKey(d => d.RoleTypeId)
                    .HasConstraintName("FK__HumanReso__RoleT__6BB9E75F");
            });

            modelBuilder.Entity<HumanResourceSalary>(entity =>
            {
                entity.HasKey(e => e.Guid)
                    .HasName("PK__HumanRes__15B69B8E23F11D35");

                entity.ToTable("HumanResourceSalary", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BeginDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ExtraSalary).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Salary).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.WorkTime).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.CadreType)
                    .WithMany(p => p.HumanResourceSalaryCadreTypes)
                    .HasForeignKey(d => d.CadreTypeId)
                    .HasConstraintName("FK_CadreType_BaseInfoGeneral");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.HumanResourceSalaryCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_HumanResourceSalary_CreateUser");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.HumanResourceSalaryCurrencies)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_HumanResourceSalary_BaseInfoGeneral");

                entity.HasOne(d => d.HumanResource)
                    .WithMany(p => p.HumanResourceSalaries)
                    .HasForeignKey(d => d.HumanResourceId)
                    .HasConstraintName("FK_HumanResourceSalary_HumanResource");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.HumanResourceSalaryModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_HumanResourceSalary_ModifiedUser");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.HumanResourceSalaryPaymentStatuses)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .HasConstraintName("FK_PaymentStatusId_BaseInfoGeneral");

                entity.HasOne(d => d.SalaryType)
                    .WithMany(p => p.HumanResourceSalarySalaryTypes)
                    .HasForeignKey(d => d.SalaryTypeId)
                    .HasConstraintName("FK_SalaryTypeId_BaseInfoGeneral");

                entity.HasOne(d => d.Surgery)
                    .WithMany(p => p.HumanResourceSalaries)
                    .HasForeignKey(d => d.SurgeryId)
                    .HasConstraintName("FK_SurgeryId_Surgery");
            });


            modelBuilder.Entity<HumanResourceSalaryPayment>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("HumanResourceSalaryPayment", "dbo");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.HumanResourceSalaryPaymentCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_HumanResourceSalaryPayment_User");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.HumanResourceSalaryPayments)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_HumanResourceSalaryPayment_BaseInfoGeneral");

                entity.HasOne(d => d.HumanResourceSalary)
                    .WithMany(p => p.HumanResourceSalaryPayments)
                    .HasForeignKey(d => d.HumanResourceSalaryId)
                    .HasConstraintName("FK_HumanResourceSalaryPayment_HumanResourceSalary");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.HumanResourceSalaryPaymentModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_HumanResourceSalaryPayment_UserModify");
            });

            modelBuilder.Entity<FN_GetPastAnalysisResult_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<GetDecimalDefaultCurrencyModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<LastestReceptionInvoiceNumModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<LatestFileNumModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<LatestPurchaseInvoiceNumModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<FN_GetAllEventsForCalendar_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<MoneyConvertModel>(entity =>
            {
                entity.HasNoKey();
            });


            modelBuilder.Entity<FN_GetMoneyConvertAmount_Result>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<ReceptionForCashReport>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<AnalysisResultMasterGrid>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<SurgeryGrid>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<HospitalDashboardModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<ReturnPurchaseDetailModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<ReturnSaleDetailModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<SupplierAccountModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<CheckStatusModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<ProductPurchaseReportModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<ProductCardexReportModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<ReturnProductPurchaseReportModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<ProductTransferReportModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<CustomerAccountModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<PartialPayModel>(entity =>
            {
                entity.HasNoKey();
            });
            
            modelBuilder.Entity<PurchaseInvoiceDetailPriceModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<ProductWithBarcodeModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<PurchaseOrTransferProductDetail>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<PieChartModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<ExpiredProductModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<IncomeModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<UserPortionExceptPaysModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<UserPortionReport>(entity =>
            {
                entity.HasNoKey();
            }); 

            modelBuilder.Entity<ExpireListModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_GetMoneyConvertAmuont)))
           .HasName("FN_GetMoneyConvertAmuont");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_GetPastAnalysisResult)))
           .HasName("FN_GetPastAnalysisResult");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_GetDecimalDefaultCurrency)))
           .HasName("FN_GetDecimalDefaultCurrency");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestReceptionInvoiceNum)))
           .HasName("FN_LatestReceptionInvoiceNum");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestFileNum)))
           .HasName("FN_LatestFileNum");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestPurchaseInvoiceNum)))
           .HasName("FN_LatestPurchaseInvoiceNum");
            
            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestReturnPurchaseInvoiceNum)))
           .HasName("FN_LatestReturnPurchaseInvoiceNum");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestDamageNum)))
           .HasName("FN_LatestDamageNum");
            
            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestSaleInvoiceNum)))
           .HasName("FN_LatestSaleInvoiceNum");
            
            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestReturnSaleInvoiceNum)))
           .HasName("FN_LatestReturnSaleInvoiceNum");
            
            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestPayInvoiceNum)))
           .HasName("FN_LatestPayInvoiceNum");
            
            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_LatestReceiveInvoiceNum)))
           .HasName("FN_LatestReceiveInvoiceNum");

            modelBuilder.HasDbFunction(typeof(WASContext)
           .GetMethod(nameof(FN_GetAllEventsForCalendar)))
           .HasName("FN_GetAllEventsForCalendar");

            modelBuilder.HasDbFunction(typeof(WASContext)
          .GetMethod(nameof(FN_GetAllSubSystemsWithAccess)))
          .HasName("FN_GetAllSubSystemsWithAccess");

            modelBuilder.HasDbFunction(typeof(WASContext)
          .GetMethod(nameof(FN_GetAllUserSubsystems)))
          .HasName("FN_GetAllUserSubsystems");

            modelBuilder.HasDbFunction(typeof(WASContext)
          .GetMethod(nameof(FN_LatestVisitNum)))
          .HasName("FN_LatestVisitNum");

            //  modelBuilder.HasDbFunction(typeof(WASContext)
            //.GetMethod(nameof(DSP_RefreshVisitNums)))
            //.HasName("DSP_RefreshVisitNums");


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public IQueryable<FN_GetMoneyConvertAmount_Result> FN_GetMoneyConvertAmuont(Nullable<Guid> ClinicSectionId, Nullable<int> AmountCurrencyId, Nullable<int> destCurrencyId, Nullable<decimal> Amount)
        => FromExpression(() => FN_GetMoneyConvertAmuont(ClinicSectionId, AmountCurrencyId, destCurrencyId, Amount));

        public IQueryable<FN_GetPastAnalysisResult_Result> FN_GetPastAnalysisResult(Nullable<Guid> PatientId, Nullable<Guid> AnalysisResultMasterId)
        => FromExpression(() => FN_GetPastAnalysisResult(PatientId, AnalysisResultMasterId));

        public IQueryable<GetDecimalDefaultCurrencyModel> FN_GetDecimalDefaultCurrency(Nullable<Guid> clinicSectionId, Nullable<int> sectionTypeId)
        => FromExpression(() => FN_GetDecimalDefaultCurrency(clinicSectionId, sectionTypeId));

        public IQueryable<LastestReceptionInvoiceNumModel> FN_LatestReceptionInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestReceptionInvoiceNum(clinicSectionId));

        public IQueryable<LatestFileNumModel> FN_LatestFileNum(Nullable<Guid> clinicSectionId, Nullable<Guid> clinicId)
        => FromExpression(() => FN_LatestFileNum(clinicSectionId, clinicId));

        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestPurchaseInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestPurchaseInvoiceNum(clinicSectionId));
        
        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestReturnPurchaseInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestReturnPurchaseInvoiceNum(clinicSectionId));

        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestSaleInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestSaleInvoiceNum(clinicSectionId));
        
        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestReturnSaleInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestReturnSaleInvoiceNum(clinicSectionId));
        
        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestPayInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestPayInvoiceNum(clinicSectionId));
        
        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestReceiveInvoiceNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestReceiveInvoiceNum(clinicSectionId));
        
        public IQueryable<LatestPurchaseInvoiceNumModel> FN_LatestDamageNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestDamageNum(clinicSectionId));

        public IQueryable<FN_GetAllEventsForCalendar_Result> FN_GetAllEventsForCalendar(Nullable<Guid> originalClinicSectionId, Nullable<Guid> clinicSectionId, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, Guid? doctorId)
        => FromExpression(() => FN_GetAllEventsForCalendar(originalClinicSectionId, clinicSectionId, fromDate, toDate, doctorId));

        public IQueryable<AllSubSystemsWithAccess> FN_GetAllSubSystemsWithAccess(Nullable<Guid> userId, Nullable<int> SectionTypeId, Nullable<Guid> clinicSectionId, Nullable<int> LanguageId, Nullable<Guid> parentUserId)
        => FromExpression(() => FN_GetAllSubSystemsWithAccess(userId, SectionTypeId, clinicSectionId, LanguageId, parentUserId));

        public IQueryable<SubSystem> FN_GetAllUserSubsystems(Nullable<Guid> userId, Nullable<int> SectionTypeId, Nullable<Guid> clinicSectionId, Nullable<int> LanguageId)
        => FromExpression(() => FN_GetAllUserSubsystems(userId, SectionTypeId, clinicSectionId, LanguageId));

        public IQueryable<LatestFileNumModel> FN_LatestVisitNum(Nullable<Guid> clinicSectionId)
        => FromExpression(() => FN_LatestVisitNum(clinicSectionId));

        //public void DSP_RefreshVisitNums(Nullable<Guid> clinicSectionId)
        //=> FromExpression(() => DSP_RefreshVisitNums(clinicSectionId));
    }
}

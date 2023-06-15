using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class DMSContext : DbContext
    {
        public DMSContext()
        {
        }

        public DMSContext(DbContextOptions<DMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<JardNewMedicineNum> JardNewMedicineNums { get; set; }
        public virtual DbSet<TblActiveCustomerType> TblActiveCustomerTypes { get; set; }
        public virtual DbSet<TblActiveSupplierType> TblActiveSupplierTypes { get; set; }
        public virtual DbSet<TblActiveUserCustomerType> TblActiveUserCustomerTypes { get; set; }
        public virtual DbSet<TblActiveUserFormType> TblActiveUserFormTypes { get; set; }
        public virtual DbSet<TblActiveUserMedicine> TblActiveUserMedicines { get; set; }
        public virtual DbSet<TblActiveUserProducerType> TblActiveUserProducerTypes { get; set; }
        public virtual DbSet<TblActiveUserSupplierType> TblActiveUserSupplierTypes { get; set; }
        public virtual DbSet<TblAlarmSetting> TblAlarmSettings { get; set; }
        public virtual DbSet<TblAnnotation> TblAnnotations { get; set; }
        public virtual DbSet<TblAutoPurchaseSupplier> TblAutoPurchaseSuppliers { get; set; }
        public virtual DbSet<TblAutoSaleClinic> TblAutoSaleClinics { get; set; }
        public virtual DbSet<TblBank> TblBanks { get; set; }
        public virtual DbSet<TblBaseInfo> TblBaseInfos { get; set; }
        public virtual DbSet<TblBaseInfoType> TblBaseInfoTypes { get; set; }
        public virtual DbSet<TblChangeTrackingCommand> TblChangeTrackingCommands { get; set; }
        public virtual DbSet<TblChangeTrackingCommandDistributed> TblChangeTrackingCommandDistributeds { get; set; }
        public virtual DbSet<TblChangeTrackingDestination> TblChangeTrackingDestinations { get; set; }
        public virtual DbSet<TblChangeTrackingGeneratorLog> TblChangeTrackingGeneratorLogs { get; set; }
        public virtual DbSet<TblChangeTrackingLog> TblChangeTrackingLogs { get; set; }
        public virtual DbSet<TblChangeTrackingLogTemp> TblChangeTrackingLogTemps { get; set; }
        public virtual DbSet<TblChangeTrackingPlan> TblChangeTrackingPlans { get; set; }
        public virtual DbSet<TblChangeTrackingReplicationLog> TblChangeTrackingReplicationLogs { get; set; }
        public virtual DbSet<TblChangeTrackingSchedule> TblChangeTrackingSchedules { get; set; }
        public virtual DbSet<TblChangeTrackingSetting> TblChangeTrackingSettings { get; set; }
        public virtual DbSet<TblChangeTrackingSettingClient> TblChangeTrackingSettingClients { get; set; }
        public virtual DbSet<TblCheckinCheckOutUser> TblCheckinCheckOutUsers { get; set; }
        public virtual DbSet<TblCombinedInvoice> TblCombinedInvoices { get; set; }
        public virtual DbSet<TblCombinedMedicine> TblCombinedMedicines { get; set; }
        public virtual DbSet<TblCost> TblCosts { get; set; }
        public virtual DbSet<TblCustomer> TblCustomers { get; set; }
        public virtual DbSet<TblCustomerSmstemp> TblCustomerSmstemps { get; set; }
        public virtual DbSet<TblDamaged> TblDamageds { get; set; }
        public virtual DbSet<TblDamagedDetail> TblDamagedDetails { get; set; }
        public virtual DbSet<TblDeletedStuff> TblDeletedStuffs { get; set; }
        public virtual DbSet<TblDeletedType> TblDeletedTypes { get; set; }
        public virtual DbSet<TblDoctor> TblDoctors { get; set; }
        public virtual DbSet<TblExchangeDetail> TblExchangeDetails { get; set; }
        public virtual DbSet<TblExchangeInvoice> TblExchangeInvoices { get; set; }
        public virtual DbSet<TblExchangeInvoicePay> TblExchangeInvoicePays { get; set; }
        public virtual DbSet<TblFirstPeriodNum> TblFirstPeriodNums { get; set; }
        public virtual DbSet<TblFont> TblFonts { get; set; }
        public virtual DbSet<TblFormAccess> TblFormAccesses { get; set; }
        public virtual DbSet<TblFormtblAccess> TblFormtblAccesses { get; set; }
        public virtual DbSet<TblHesabat> TblHesabats { get; set; }
        public virtual DbSet<TblJardSetting> TblJardSettings { get; set; }
        public virtual DbSet<TblMaktab> TblMaktabs { get; set; }
        public virtual DbSet<TblMaktabAcc> TblMaktabAccs { get; set; }
        public virtual DbSet<TblMaktabPay> TblMaktabPays { get; set; }
        public virtual DbSet<TblMandoobHistory> TblMandoobHistories { get; set; }
        public virtual DbSet<TblMandoobReturnHistory> TblMandoobReturnHistories { get; set; }
        public virtual DbSet<TblMandoobTarget> TblMandoobTargets { get; set; }
        public virtual DbSet<TblMedicine> TblMedicines { get; set; }
        public virtual DbSet<TblMedicineBarcode> TblMedicineBarcodes { get; set; }
        public virtual DbSet<TblMedicineImage> TblMedicineImages { get; set; }
        public virtual DbSet<TblMedicinePurchaseFactorList> TblMedicinePurchaseFactorLists { get; set; }
        public virtual DbSet<TblMedicineSaleFactorList> TblMedicineSaleFactorLists { get; set; }
        public virtual DbSet<TblMedicineSold> TblMedicineSolds { get; set; }
        public virtual DbSet<TblMedicinesEquivalent> TblMedicinesEquivalents { get; set; }
        public virtual DbSet<TblMedicinesEquivalentClinic> TblMedicinesEquivalentClinics { get; set; }
        public virtual DbSet<TblMoneyConvert> TblMoneyConverts { get; set; }
        public virtual DbSet<TblMosh> TblMoshes { get; set; }
        public virtual DbSet<TblPatient> TblPatients { get; set; }
        public virtual DbSet<TblPay> TblPays { get; set; }
        public virtual DbSet<TblPeriod> TblPeriods { get; set; }
        public virtual DbSet<TblPrefactor> TblPrefactors { get; set; }
        public virtual DbSet<TblPrefactorDetail> TblPrefactorDetails { get; set; }
        public virtual DbSet<TblPurchasOrderDetail> TblPurchasOrderDetails { get; set; }
        public virtual DbSet<TblPurchase> TblPurchases { get; set; }
        public virtual DbSet<TblPurchaseInvoice> TblPurchaseInvoices { get; set; }
        public virtual DbSet<TblPurchaseInvoiceDetail> TblPurchaseInvoiceDetails { get; set; }
        public virtual DbSet<TblPurchaseInvoicePay> TblPurchaseInvoicePays { get; set; }
        public virtual DbSet<TblPurchaseOrder> TblPurchaseOrders { get; set; }
        public virtual DbSet<TblPurchases1> TblPurchases1s { get; set; }
        public virtual DbSet<TblReciever> TblRecievers { get; set; }
        public virtual DbSet<TblReplicationLog> TblReplicationLogs { get; set; }
        public virtual DbSet<TblReportSetting> TblReportSettings { get; set; }
        public virtual DbSet<TblResetInvoiceNum> TblResetInvoiceNums { get; set; }
        public virtual DbSet<TblReturnPurchaseInvoice> TblReturnPurchaseInvoices { get; set; }
        public virtual DbSet<TblReturnPurchaseInvoiceDetail> TblReturnPurchaseInvoiceDetails { get; set; }
        public virtual DbSet<TblReturnPurchaseInvoicePay> TblReturnPurchaseInvoicePays { get; set; }
        public virtual DbSet<TblReturnSaleInvoice> TblReturnSaleInvoices { get; set; }
        public virtual DbSet<TblReturnSaleInvoiceDetail> TblReturnSaleInvoiceDetails { get; set; }
        public virtual DbSet<TblReturnSaleInvoiceReceiver> TblReturnSaleInvoiceReceivers { get; set; }
        public virtual DbSet<TblSale> TblSales { get; set; }
        public virtual DbSet<TblSale1> TblSale1s { get; set; }
        public virtual DbSet<TblSaleInvoice> TblSaleInvoices { get; set; }
        public virtual DbSet<TblSaleInvoice1> TblSaleInvoice1s { get; set; }
        public virtual DbSet<TblSaleInvoice2> TblSaleInvoice2s { get; set; }
        public virtual DbSet<TblSaleInvoiceDetail> TblSaleInvoiceDetails { get; set; }
        public virtual DbSet<TblSaleInvoiceDetails1> TblSaleInvoiceDetails1s { get; set; }
        public virtual DbSet<TblSaleInvoiceDetails2> TblSaleInvoiceDetails2s { get; set; }
        public virtual DbSet<TblSaleInvoiceLock> TblSaleInvoiceLocks { get; set; }
        public virtual DbSet<TblSaleInvoiceReciever> TblSaleInvoiceRecievers { get; set; }
        public virtual DbSet<TblSalePurchaseDetail> TblSalePurchaseDetails { get; set; }
        public virtual DbSet<TblSettingValue> TblSettingValues { get; set; }
        public virtual DbSet<TblSii> TblSiis { get; set; }
        public virtual DbSet<TblSmshistory> TblSmshistories { get; set; }
        public virtual DbSet<TblSmstemplate> TblSmstemplates { get; set; }
        public virtual DbSet<TblSoftwareSetting> TblSoftwareSettings { get; set; }
        public virtual DbSet<TblSoftwareSettingSecond> TblSoftwareSettingSeconds { get; set; }
        public virtual DbSet<TblSummary> TblSummaries { get; set; }
        public virtual DbSet<TblSuppAcc> TblSuppAccs { get; set; }
        public virtual DbSet<TblSuppAcc1> TblSuppAcc1s { get; set; }
        public virtual DbSet<TblSuppAcc2> TblSuppAcc2s { get; set; }
        public virtual DbSet<TblSuppAcc3> TblSuppAcc3s { get; set; }
        public virtual DbSet<TblSuppAcc4> TblSuppAcc4s { get; set; }
        public virtual DbSet<TblSupplier> TblSuppliers { get; set; }
        public virtual DbSet<TblSuppliers1> TblSuppliers1s { get; set; }
        public virtual DbSet<TblTablesList> TblTablesLists { get; set; }
        public virtual DbSet<TblTempDate> TblTempDates { get; set; }
        public virtual DbSet<TblTempInt> TblTempInts { get; set; }
        public virtual DbSet<TblTempNumeric> TblTempNumerics { get; set; }
        public virtual DbSet<TblTempNvarchar> TblTempNvarchars { get; set; }
        public virtual DbSet<TblTest> TblTests { get; set; }
        public virtual DbSet<TblTheme> TblThemes { get; set; }
        public virtual DbSet<TblTopMedicine> TblTopMedicines { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }
        public virtual DbSet<TblUserFormAccess> TblUserFormAccesses { get; set; }
        public virtual DbSet<TblWareHouseHandling> TblWareHouseHandlings { get; set; }
        public virtual DbSet<TblWareHouseHandlingDetail> TblWareHouseHandlingDetails { get; set; }
        public virtual DbSet<TblWarehouseTransferring> TblWarehouseTransferrings { get; set; }
        public virtual DbSet<TblWarehouseTransferringDetail> TblWarehouseTransferringDetails { get; set; }
        public virtual DbSet<TblWebFormAccess> TblWebFormAccesses { get; set; }
        public virtual DbSet<TblWebFormtblAccess> TblWebFormtblAccesses { get; set; }
        public virtual DbSet<TblWebUser> TblWebUsers { get; set; }
        public virtual DbSet<TblWebUserCustomer> TblWebUserCustomers { get; set; }
        public virtual DbSet<TblWebUserFormAccess> TblWebUserFormAccesses { get; set; }
        public virtual DbSet<TblZone> TblZones { get; set; }
        public virtual DbSet<TblZoneMandoob> TblZoneMandoobs { get; set; }
        public virtual DbSet<VwActiveCustomerType> VwActiveCustomerTypes { get; set; }
        public virtual DbSet<VwActiveSupplierType> VwActiveSupplierTypes { get; set; }
        public virtual DbSet<VwCost> VwCosts { get; set; }
        public virtual DbSet<VwNumMidicine> VwNumMidicines { get; set; }
        public virtual DbSet<VwOrderPointList> VwOrderPointLists { get; set; }
        public virtual DbSet<VwReminingMedicineNumReport> VwReminingMedicineNumReports { get; set; }
        public virtual DbSet<VwReturnPurchaseInvoice> VwReturnPurchaseInvoices { get; set; }
        public virtual DbSet<VwShantyonPurchaseInvoice> VwShantyonPurchaseInvoices { get; set; }

        //[DbFunction("FN_MedicineNum", "dbo")]
        //public static int FN_MedicineNum(int MedicineId)
        //{
        //    throw new NotImplementedException();
        //}

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("data source=DESKTOP-PGSD8J6\\MSSQLSERVER2021;initial catalog=THCOstoora;persist security info=True;user id=sa;password=1990301223;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<JardNewMedicineNum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("JardNewMedicineNum");

                entity.Property(e => e.Currentnum).HasColumnName("currentnum");

                entity.Property(e => e.JoineryName).HasMaxLength(400);

                entity.Property(e => e.Price)
                    .HasColumnType("numeric(20, 6)")
                    .HasColumnName("price");
            });

            modelBuilder.Entity<TblActiveCustomerType>(entity =>
            {
                entity.ToTable("tblActiveCustomerType");
            });

            modelBuilder.Entity<TblActiveSupplierType>(entity =>
            {
                entity.ToTable("tblActiveSupplierType");
            });

            modelBuilder.Entity<TblActiveUserCustomerType>(entity =>
            {
                entity.ToTable("tblActiveUserCustomerType");
            });

            modelBuilder.Entity<TblActiveUserFormType>(entity =>
            {
                entity.ToTable("tblActiveUserFormType");
            });

            modelBuilder.Entity<TblActiveUserMedicine>(entity =>
            {
                entity.ToTable("tblActiveUserMedicine");
            });

            modelBuilder.Entity<TblActiveUserProducerType>(entity =>
            {
                entity.ToTable("tblActiveUserProducerType");
            });

            modelBuilder.Entity<TblActiveUserSupplierType>(entity =>
            {
                entity.ToTable("tblActiveUserSupplierType");
            });

            modelBuilder.Entity<TblAlarmSetting>(entity =>
            {
                entity.ToTable("tblAlarmSettings");

                entity.Property(e => e.ExpireDateLessThanIs).HasColumnType("date");
            });

            modelBuilder.Entity<TblAnnotation>(entity =>
            {
                entity.ToTable("tblAnnotation");

                entity.Property(e => e.AutoAnnotation).HasMaxLength(2000);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IsShowing).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReminderDate).HasColumnType("datetime");

                entity.Property(e => e.SourceType).HasMaxLength(50);

                entity.Property(e => e.TextAnnotation)
                    .IsRequired()
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<TblAutoPurchaseSupplier>(entity =>
            {
                entity.ToTable("tblAutoPurchaseSuppliers");

                entity.Property(e => e.HostDataSource).HasMaxLength(250);

                entity.Property(e => e.HostDbname)
                    .HasMaxLength(250)
                    .HasColumnName("HostDBName");

                entity.Property(e => e.HostPass).HasMaxLength(250);

                entity.Property(e => e.HostUserName).HasMaxLength(250);

                entity.Property(e => e.TablePrefix).HasMaxLength(50);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.TblAutoPurchaseSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_tblAutoPurchaseSuppliers_tblSuppliers");
            });

            modelBuilder.Entity<TblAutoSaleClinic>(entity =>
            {
                entity.ToTable("tblAutoSaleClinics");

                entity.Property(e => e.HostDataSource).HasMaxLength(250);

                entity.Property(e => e.HostDbname)
                    .HasMaxLength(250)
                    .HasColumnName("HostDBName");

                entity.Property(e => e.HostPass).HasMaxLength(250);

                entity.Property(e => e.HostUserName).HasMaxLength(250);
            });

            modelBuilder.Entity<TblBank>(entity =>
            {
                entity.ToTable("tblBank");

                entity.Property(e => e.Amount).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Desc).HasMaxLength(2000);

                entity.Property(e => e.InvoiceCost).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.InvoiceId).HasMaxLength(20);

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.Property(e => e.StatusC).HasMaxLength(1);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblBankCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblBank_tblUser_Created");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblBankModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblBank_tblUser_Modified");
            });

            modelBuilder.Entity<TblBaseInfo>(entity =>
            {
                entity.ToTable("tblBaseInfo");

                entity.HasIndex(e => new { e.TypeId, e.Name }, "UQ__tblBaseI__36585BFB3C69FB99")
                    .IsUnique();

                entity.Property(e => e.Description1).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TblBaseInfos)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_tblBaseInfo_tblBaseInfoType");
            });

            modelBuilder.Entity<TblBaseInfoType>(entity =>
            {
                entity.ToTable("tblBaseInfoType");

                entity.Property(e => e.Ename)
                    .HasMaxLength(100)
                    .HasColumnName("EName");

                entity.Property(e => e.Fname)
                    .HasMaxLength(100)
                    .HasColumnName("FName");
            });

            modelBuilder.Entity<TblChangeTrackingCommand>(entity =>
            {
                entity.ToTable("tblChangeTrackingCommand");

                entity.Property(e => e.CtcreationVersion).HasColumnName("CTCreationVersion");

                entity.Property(e => e.Ctsqlresult).HasColumnName("CTSQLResult");

                entity.Property(e => e.Ctversion).HasColumnName("CTVersion");

                entity.Property(e => e.StatusC).HasMaxLength(10);
            });

            modelBuilder.Entity<TblChangeTrackingCommandDistributed>(entity =>
            {
                entity.ToTable("tblChangeTrackingCommandDistributed");

                entity.Property(e => e.CtcreationVersion).HasColumnName("CTCreationVersion");

                entity.Property(e => e.Ctsqlresult).HasColumnName("CTSQLResult");

                entity.Property(e => e.Ctversion).HasColumnName("CTVersion");

                entity.Property(e => e.StatusC).HasMaxLength(10);
            });

            modelBuilder.Entity<TblChangeTrackingDestination>(entity =>
            {
                entity.ToTable("tblChangeTrackingDestination");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Destination).HasMaxLength(150);
            });

            modelBuilder.Entity<TblChangeTrackingGeneratorLog>(entity =>
            {
                entity.ToTable("tblChangeTrackingGeneratorLog");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblChangeTrackingLog>(entity =>
            {
                entity.ToTable("tblChangeTrackingLog");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.TblChangeTrackingLogs)
                    .HasForeignKey(d => d.DestinationId)
                    .HasConstraintName("FK_tblChangeTrackingLog_tblChangeTrackingDestination");
            });

            modelBuilder.Entity<TblChangeTrackingLogTemp>(entity =>
            {
                entity.ToTable("tblChangeTrackingLogTemp");

                entity.Property(e => e.Query).HasColumnName("query");

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblChangeTrackingPlan>(entity =>
            {
                entity.ToTable("tblChangeTrackingPlan");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<TblChangeTrackingReplicationLog>(entity =>
            {
                entity.ToTable("tblChangeTrackingReplicationLog");

                entity.Property(e => e.DestinationDbname)
                    .HasMaxLength(150)
                    .HasColumnName("DestinationDBName");

                entity.Property(e => e.DestinationServerName).HasMaxLength(250);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).HasColumnType("text");

                entity.Property(e => e.SourceDbname)
                    .HasMaxLength(150)
                    .HasColumnName("SourceDBName");

                entity.Property(e => e.SourceServerName).HasMaxLength(250);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblChangeTrackingSchedule>(entity =>
            {
                entity.ToTable("tblChangeTrackingSchedule");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.TblChangeTrackingSchedules)
                    .HasForeignKey(d => d.DestinationId)
                    .HasConstraintName("FK_tblChangeTrackingSchedule_tblChangeTrackingDestination");
            });

            modelBuilder.Entity<TblChangeTrackingSetting>(entity =>
            {
                entity.ToTable("tblChangeTrackingSetting");
            });

            modelBuilder.Entity<TblChangeTrackingSettingClient>(entity =>
            {
                entity.ToTable("tblChangeTrackingSettingClient");

                entity.Property(e => e.ClientName).HasMaxLength(250);

                entity.Property(e => e.TablePrefix).HasMaxLength(250);
            });

            modelBuilder.Entity<TblCheckinCheckOutUser>(entity =>
            {
                entity.ToTable("tblCheckinCheckOutUsers");

                entity.Property(e => e.CheckIn).HasColumnType("datetime");

                entity.Property(e => e.Checkout).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblCheckinCheckOutUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCheckinCheckOutUsers_tblUser");
            });

            modelBuilder.Entity<TblCombinedInvoice>(entity =>
            {
                entity.ToTable("tblCombinedInvoice");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.HandPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblCombinedInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblCombinedInvoice_tblUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblCombinedInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblCombinedInvoice_tblUser1");
            });

            modelBuilder.Entity<TblCombinedMedicine>(entity =>
            {
                entity.ToTable("tblCombinedMedicine");
            });

            modelBuilder.Entity<TblCost>(entity =>
            {
                entity.ToTable("tblCost");

                entity.Property(e => e.CostType).HasDefaultValueSql("((0))");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.QabzNum).HasMaxLength(50);

                entity.Property(e => e.StatusC).HasMaxLength(1);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblCostCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblCost_tblUser_Created");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblCostModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblCost_tblUser_Modified");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.ToTable("tblCustomers");

                entity.HasIndex(e => e.Name, "UQ__tblCustomers1")
                    .IsUnique();

                entity.Property(e => e.ActivityStartDate).HasColumnType("datetime");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Caccount)
                    .HasColumnType("decimal(20, 5)")
                    .HasColumnName("CAccount");

                entity.Property(e => e.CaccountSecond)
                    .HasColumnType("decimal(20, 5)")
                    .HasColumnName("CAccountSecond");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CustomerProfit).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.ExtraPhone).HasMaxLength(150);

                entity.Property(e => e.Fax).HasMaxLength(15);

                entity.Property(e => e.LicenseNumber).HasMaxLength(150);

                entity.Property(e => e.LicenseeMobile).HasMaxLength(150);

                entity.Property(e => e.Mob1).HasMaxLength(150);

                entity.Property(e => e.Mob2).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Sponsor).HasMaxLength(150);

                entity.Property(e => e.StatusC).HasMaxLength(1);

                entity.Property(e => e.Tel1).HasMaxLength(150);

                entity.Property(e => e.Tel2).HasMaxLength(150);

                entity.Property(e => e.Website).HasMaxLength(100);

                entity.HasOne(d => d.CustomersType)
                    .WithMany(p => p.TblCustomers)
                    .HasForeignKey(d => d.CustomersTypeId)
                    .HasConstraintName("FK_tblCustomers_tblBaseInfo");
            });

            modelBuilder.Entity<TblCustomerSmstemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblCustomerSMSTemp");

                entity.Property(e => e.CustomersType).HasMaxLength(100);

                entity.Property(e => e.Mob1).HasMaxLength(15);

                entity.Property(e => e.Mob2).HasMaxLength(15);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RemainAccountDate).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.RemaindAccount).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<TblDamaged>(entity =>
            {
                entity.ToTable("tblDamaged");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblDamagedCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblDamaged_tblUser_Created");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblDamagedModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblDamaged_tblUser_Modified");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.TblDamageds)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_tblDamaged_tblBaseInfo");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblDamagedUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblDamaged_tblUser");
            });

            modelBuilder.Entity<TblDamagedDetail>(entity =>
            {
                entity.ToTable("tblDamagedDetails");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TblDamagedDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDamagedDetails_tblDamaged");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.TblDamagedDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblDamagedDetails_tblPurchaseInvoiceDetails");
            });

            modelBuilder.Entity<TblDeletedStuff>(entity =>
            {
                entity.ToTable("tblDeletedStuff");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.TblDeletedStuffs)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_tblDeletedStuff_tblDeletedType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblDeletedStuffs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblDeletedStuff_tblUser");
            });

            modelBuilder.Entity<TblDeletedType>(entity =>
            {
                entity.ToTable("tblDeletedType");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Persian).HasMaxLength(100);
            });

            modelBuilder.Entity<TblDoctor>(entity =>
            {
                entity.ToTable("tblDoctor");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Expertise)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("FName");

                entity.Property(e => e.Image).IsRequired();

                entity.Property(e => e.Lname)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("LName");

                entity.Property(e => e.MedicalSystemNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TblExchangeDetail>(entity =>
            {
                entity.ToTable("tblExchangeDetail");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Consideration).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 2)");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TblExchangeDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExchangeDetail_tblExchangeInvoice");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.TblExchangeDetails)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExchangeDetail_tblSuppliers");
            });

            modelBuilder.Entity<TblExchangeInvoice>(entity =>
            {
                entity.ToTable("tblExchangeInvoice");

                entity.Property(e => e.CostDinar).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.CostDollar).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblExchangeInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblExchangeInvoice_tblUser");

                entity.HasOne(d => d.Maktab)
                    .WithMany(p => p.TblExchangeInvoices)
                    .HasForeignKey(d => d.MaktabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExchangeInvoice_tblMaktab");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblExchangeInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExchangeInvoice_tblUserModified");
            });

            modelBuilder.Entity<TblExchangeInvoicePay>(entity =>
            {
                entity.ToTable("tblExchangeInvoicePay");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.TblExchangeInvoicePays)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExchangeInvoicePay_tblExchangeInvoice");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.TblExchangeInvoicePays)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblExchangeInvoicePay_tblMaktabPay");
            });

            modelBuilder.Entity<TblFirstPeriodNum>(entity =>
            {
                entity.ToTable("tblFirstPeriodNum");
            });

            modelBuilder.Entity<TblFont>(entity =>
            {
                entity.ToTable("tblFont");

                entity.Property(e => e.Name).HasMaxLength(70);
            });

            modelBuilder.Entity<TblFormAccess>(entity =>
            {
                entity.ToTable("tblFormAccess");
            });

            modelBuilder.Entity<TblFormtblAccess>(entity =>
            {
                entity.ToTable("tblFormtblAccess");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Ename)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("EName");

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("FName");
            });

            modelBuilder.Entity<TblHesabat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblHesabat");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Mcode)
                    .HasMaxLength(255)
                    .HasColumnName("MCode");

                entity.Property(e => e.Mgroup)
                    .HasMaxLength(255)
                    .HasColumnName("MGroup");

                entity.Property(e => e.Mname)
                    .HasMaxLength(255)
                    .HasColumnName("MName");

                entity.Property(e => e.Mobile).HasMaxLength(255);

                entity.Property(e => e.Text42).HasMaxLength(255);
            });

            modelBuilder.Entity<TblJardSetting>(entity =>
            {
                entity.ToTable("tblJardSettings");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.FormName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblMaktab>(entity =>
            {
                entity.ToTable("tblMaktab");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Mob1).HasMaxLength(50);

                entity.Property(e => e.Mob2).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Tel1).HasMaxLength(50);

                entity.Property(e => e.Tel2).HasMaxLength(50);
            });

            modelBuilder.Entity<TblMaktabAcc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblMaktabAcc");

                entity.Property(e => e.CreatedUsername).HasMaxLength(250);

                entity.Property(e => e.EndRemain).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.InvoiceNum).HasMaxLength(50);

                entity.Property(e => e.MaktabName).HasMaxLength(100);

                entity.Property(e => e.ModifiedUsername).HasMaxLength(250);

                entity.Property(e => e.MyDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RecievedAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RemainAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalExchange).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalMaktabExchange).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalMaktabPaid).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalMaktabRecieved).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalPaid).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalRecieved).HasColumnType("numeric(20, 5)");
            });

            modelBuilder.Entity<TblMaktabPay>(entity =>
            {
                entity.ToTable("tblMaktabPay");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Desc).IsRequired();

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblMaktabPayCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaktabPay_tblUser");

                entity.HasOne(d => d.Maktab)
                    .WithMany(p => p.TblMaktabPays)
                    .HasForeignKey(d => d.MaktabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaktabPay_tblMaktab");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblMaktabPayModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaktabPay_tblUser1");
            });

            modelBuilder.Entity<TblMandoobHistory>(entity =>
            {
                entity.ToTable("tblMandoobHistory");

                entity.HasOne(d => d.MainSaleInvoiceDetail)
                    .WithMany(p => p.TblMandoobHistoryMainSaleInvoiceDetails)
                    .HasForeignKey(d => d.MainSaleInvoiceDetailId)
                    .HasConstraintName("FK_tblMandoobHistory_tblSaleInvoiceDetails_Main");

                entity.HasOne(d => d.MandoobSaleInvoiceDetail)
                    .WithMany(p => p.TblMandoobHistoryMandoobSaleInvoiceDetails)
                    .HasForeignKey(d => d.MandoobSaleInvoiceDetailId)
                    .HasConstraintName("FK_tblMandoobHistory_tblSaleInvoiceDetails_Mandoob");
            });

            modelBuilder.Entity<TblMandoobReturnHistory>(entity =>
            {
                entity.ToTable("tblMandoobReturnHistory");

                entity.HasOne(d => d.MainSaleInvoiceDetail)
                    .WithMany(p => p.TblMandoobReturnHistoryMainSaleInvoiceDetails)
                    .HasForeignKey(d => d.MainSaleInvoiceDetailId)
                    .HasConstraintName("FK_tblMandoobReturnHistory_tblSaleInvoiceDetails_Main");

                entity.HasOne(d => d.MandoobSaleInvoiceDetail)
                    .WithMany(p => p.TblMandoobReturnHistoryMandoobSaleInvoiceDetails)
                    .HasForeignKey(d => d.MandoobSaleInvoiceDetailId)
                    .HasConstraintName("FK_tblMandoobReturnHistory_tblSaleInvoiceDetails_Mandoob");
            });

            modelBuilder.Entity<TblMandoobTarget>(entity =>
            {
                entity.ToTable("tblMandoobTarget");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.Datefrom).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblMedicine>(entity =>
            {
                entity.ToTable("tblMedicine");

                entity.HasIndex(e => e.JoineryName, "CL_JoineryName");

                entity.HasIndex(e => new { e.Id, e.ProducerId, e.FormId }, "_dta_index_tblMedicine_13_1733581214__K1_K6_K8");

                entity.HasIndex(e => e.ProducerId, "_dta_index_tblMedicine_ProducerId");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.BaseNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Crc).HasColumnName("CRC");

                entity.Property(e => e.GeneralUnitNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.JoineryName).HasMaxLength(1000);

                entity.Property(e => e.LatestExpireDate).HasColumnType("datetime");

                entity.Property(e => e.LatestPurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.LatestPurchasePrice).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.LatestSellingPrice).HasColumnType("decimal(20, 7)");

                entity.Property(e => e.Location).HasMaxLength(50);

                //entity.Ignore(e=>e.MdNum);

                entity.Property(e => e.MfreeNumRemaining)
                    .HasColumnType("numeric(20, 7)")
                    .HasColumnName("MFreeNumRemaining");

                entity.Property(e => e.MnumRemaining)
                    .HasColumnType("numeric(20, 7)")
                    .HasColumnName("MNumRemaining");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PaakatWeight).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.ScientificName).HasMaxLength(1000);

                entity.Property(e => e.StatusC).HasMaxLength(1);

            });

            modelBuilder.Entity<TblMedicineBarcode>(entity =>
            {
                entity.ToTable("tblMedicineBarcode");

                entity.Property(e => e.Barcode).HasMaxLength(50);
            });

            modelBuilder.Entity<TblMedicineImage>(entity =>
            {
                entity.ToTable("tblMedicineImage");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Crc).HasColumnName("CRC");
            });

            modelBuilder.Entity<TblMedicinePurchaseFactorList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblMedicinePurchaseFactorList");

                entity.Property(e => e.BujNumber).HasMaxLength(100);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 1)");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasMaxLength(10);

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Paakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PaakatFreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingFreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingNum).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblMedicineSaleFactorList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblMedicineSaleFactorList");

                entity.Property(e => e.BujNumber).HasMaxLength(50);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 1)");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasMaxLength(10);

                entity.Property(e => e.InvoiceNum).HasMaxLength(10);

                entity.Property(e => e.MandoobDetail).HasMaxLength(250);

                entity.Property(e => e.MandoobFreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Paakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingFreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingMandoobFreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainingNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SalePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SalePriceBeforeDiscount).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblMedicineSold>(entity =>
            {
                entity.ToTable("tblMedicineSold");

                entity.HasIndex(e => new { e.MedicineId, e.SoldDate }, "CL_MedicineId_SoldDate");

                entity.Property(e => e.SoldDate).HasColumnType("date");
            });

            modelBuilder.Entity<TblMedicinesEquivalent>(entity =>
            {
                entity.ToTable("tblMedicinesEquivalent");
            });

            modelBuilder.Entity<TblMedicinesEquivalentClinic>(entity =>
            {
                entity.ToTable("tblMedicinesEquivalentClinic");
            });

            modelBuilder.Entity<TblMoneyConvert>(entity =>
            {
                entity.ToTable("tblMoneyConvert");

                entity.HasIndex(e => e.ConvertDate, "tblMoneyConvert_ConvertDate");

                entity.Property(e => e.ConvertDate).HasColumnType("datetime");

                entity.Property(e => e.Dinar).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Dollar).HasColumnType("decimal(20, 2)");
            });

            modelBuilder.Entity<TblMosh>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblMosh");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Mcode)
                    .HasMaxLength(255)
                    .HasColumnName("MCode");

                entity.Property(e => e.Mgroup)
                    .HasMaxLength(255)
                    .HasColumnName("MGroup");

                entity.Property(e => e.Mname)
                    .HasMaxLength(255)
                    .HasColumnName("MName");

                entity.Property(e => e.Mobile).HasMaxLength(255);

                entity.Property(e => e.Text42).HasMaxLength(255);
            });

            modelBuilder.Entity<TblPatient>(entity =>
            {
                entity.ToTable("tblPatient");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Code).HasMaxLength(250);

                entity.Property(e => e.Fname)
                    .HasMaxLength(250)
                    .HasColumnName("FName");

                entity.Property(e => e.InsuranceNumber).HasMaxLength(250);

                entity.Property(e => e.Lname)
                    .HasMaxLength(250)
                    .HasColumnName("LName");

                entity.Property(e => e.MobileNumber).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(250);
            });

            modelBuilder.Entity<TblPay>(entity =>
            {
                entity.ToTable("tblPay");

                entity.HasIndex(e => e.SuppliersId, "CL_SuppliersId");

                entity.HasIndex(e => e.PayDate, "Index_PayDate");

                entity.HasIndex(e => e.InvoiceId, "Index_PayInvoiceId");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.IsPaid)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblPayCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblPay_tblUser_Created");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblPayModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblPay_tblUser_Modified");

                entity.HasOne(d => d.Suppliers)
                    .WithMany(p => p.TblPays)
                    .HasForeignKey(d => d.SuppliersId)
                    .HasConstraintName("FK_tblPay_tblSuppliers");
            });

            modelBuilder.Entity<TblPeriod>(entity =>
            {
                entity.ToTable("tblPeriod");

                entity.Property(e => e.Period).HasMaxLength(10);
            });

            modelBuilder.Entity<TblPrefactor>(entity =>
            {
                entity.ToTable("tblPrefactor");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.FactorType).HasMaxLength(20);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.TblPrefactorCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_tblPrefactor_tblUser");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblPrefactors)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrefactor_tblCustomers");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblPrefactorModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblPrefactor_tblModifiedUser");
            });

            modelBuilder.Entity<TblPrefactorDetail>(entity =>
            {
                entity.ToTable("tblPrefactorDetail");

                entity.Property(e => e.JoineryName).HasMaxLength(400);

                entity.Property(e => e.SalePrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblPrefactorDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPrefactorDetail_tblUser");
            });

            modelBuilder.Entity<TblPurchasOrderDetail>(entity =>
            {
                entity.ToTable("tblPurchasOrderDetail");

                entity.Property(e => e.JoineryName).HasMaxLength(400);

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblPurchasOrderDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPurchasOrderDetail_tblUser");
            });

            modelBuilder.Entity<TblPurchase>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblPurchase");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<TblPurchaseInvoice>(entity =>
            {
                entity.ToTable("tblPurchaseInvoice");

                entity.HasIndex(e => e.InvoiceDate, "CL_PurchaseInvoiceInvoiceDate");

                entity.Property(e => e.Cost).HasColumnType("numeric(20, 10)");

                entity.Property(e => e.Desc).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.InvoiceRemaining).HasColumnType("decimal(20, 5)");

                entity.Property(e => e.MainInvoiceNum).HasMaxLength(500);

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 5)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblPurchaseInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblPurchaseInvoice_tblUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblPurchaseInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPurchaseInvoice_tblUserModified");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.TblPurchaseInvoices)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_tblPurchaseInvoice_tblSuppliers");
            });

            modelBuilder.Entity<TblPurchaseInvoiceDetail>(entity =>
            {
                entity.ToTable("tblPurchaseInvoiceDetails");

                entity.HasIndex(e => e.ExpireDate, "CL_ExpireDate");

                entity.HasIndex(e => e.MedicineId, "CL_MedicineId");

                entity.HasIndex(e => e.MasterId, "CL_tblPurchaseInvoiceDetails_MasterId");

                entity.Property(e => e.BujNumber).HasMaxLength(50);

                entity.Property(e => e.DetailDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.LeastSellingPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.LeastSellingPriceProfit).HasColumnType("numeric(20, 3)");

                entity.Property(e => e.LeastWholesalePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.LeastWholesalePriceProfit).HasColumnType("numeric(20, 3)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumFreeNumRemaining)
                    .HasColumnType("numeric(20, 7)")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumRemaining)
                    .HasColumnType("numeric(20, 7)")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.Paakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.ProductionDate).HasColumnType("date");

                entity.Property(e => e.Profit).HasColumnType("numeric(20, 3)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SecondProfit).HasColumnType("numeric(20, 3)");

                entity.Property(e => e.SellingPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SharikaFreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SupplierConsideration).HasMaxLength(100);

                entity.Property(e => e.WholesalePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.WholesalePriceProfit).HasColumnType("numeric(20, 3)");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.TblPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.MedicineId)
                    .HasConstraintName("FK_tblPurchaseInvoiceDetails_tblMedicine");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPurchaseInvoiceDetails_tblUserModified");
            });

            modelBuilder.Entity<TblPurchaseInvoicePay>(entity =>
            {
                entity.ToTable("tblPurchaseInvoicePay");

                entity.HasIndex(e => e.InvoiceId, "Index_PurchaseInvoicePayInvoiceId");

                entity.HasIndex(e => e.InvoiceId, "tblPurchaseInvoicePay_InvoiceIdIndex");
            });

            modelBuilder.Entity<TblPurchaseOrder>(entity =>
            {
                entity.ToTable("tblPurchaseOrder");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.FactorType).HasMaxLength(20);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.TblPurchaseOrderCreateUsers)
                    .HasForeignKey(d => d.CreateUserId)
                    .HasConstraintName("FK_tblPurchaseOrder_tblUser");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblPurchaseOrderModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblPurchaseOrder_tblModifiedUser");
            });

            modelBuilder.Entity<TblPurchases1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblPurchases1");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<TblReciever>(entity =>
            {
                entity.ToTable("tblRecievers");

                entity.HasIndex(e => new { e.CustomerId, e.IsRecieved, e.RecieveDate }, "NCL_tblRecievers");

                entity.HasIndex(e => e.CustomerId, "idx_tblRecievers_CustomerId");

                entity.HasIndex(e => e.InvoiceId, "idx_tblRecievers_InvoiceId");

                entity.Property(e => e.Amount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.DinarPerDollar).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.Property(e => e.RebhDamaged).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RecieveDate).HasColumnType("datetime");

                entity.Property(e => e.UnderAccount).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblRecieverCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblRecievers_tblUser_Created");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblRecievers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_tblRecievers_tblCustomers");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblRecieverModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_tblRecievers_tblUser_Modified");
            });

            modelBuilder.Entity<TblReplicationLog>(entity =>
            {
                entity.ToTable("tblReplicationLog");

                entity.Property(e => e.DestinationDbname)
                    .HasMaxLength(150)
                    .HasColumnName("DestinationDBName");

                entity.Property(e => e.DestinationServerName).HasMaxLength(250);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).HasColumnType("text");

                entity.Property(e => e.SourceDbname)
                    .HasMaxLength(150)
                    .HasColumnName("SourceDBName");

                entity.Property(e => e.SourceServerName).HasMaxLength(250);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblReportSetting>(entity =>
            {
                entity.ToTable("tblReportSettings");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.AddressLatin).HasMaxLength(500);

                entity.Property(e => e.CompanyName).HasMaxLength(500);

                entity.Property(e => e.CompanyNameLatin).HasMaxLength(500);

                entity.Property(e => e.Desc).HasMaxLength(500);

                entity.Property(e => e.DescLation).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.LastReportStatement).HasMaxLength(500);

                entity.Property(e => e.Mob).HasMaxLength(11);

                entity.Property(e => e.Tel1).HasMaxLength(11);

                entity.Property(e => e.Tel2).HasMaxLength(11);

                entity.Property(e => e.Website).HasMaxLength(200);
            });

            modelBuilder.Entity<TblResetInvoiceNum>(entity =>
            {
                entity.ToTable("tblResetInvoiceNum");

                entity.Property(e => e.InvoiceType).HasMaxLength(50);

                entity.Property(e => e.PreInvoiceNum).HasMaxLength(50);

                entity.Property(e => e.ResetFromDate).HasColumnType("date");

                entity.Property(e => e.ResetToDate).HasColumnType("date");
            });

            modelBuilder.Entity<TblReturnPurchaseInvoice>(entity =>
            {
                entity.ToTable("tblReturnPurchaseInvoice");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ReturnDiscount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.TblReturnPurchaseInvoices)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_dbo.tblReturnPurchaseInvoice_tblBaseInfo");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.TblReturnPurchaseInvoices)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.tblReturnPurchaseInvoice_tblSuppliers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblReturnPurchaseInvoices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblReturnPurchaseInvoice_tblUser");
            });

            modelBuilder.Entity<TblReturnPurchaseInvoiceDetail>(entity =>
            {
                entity.ToTable("tblReturnPurchaseInvoiceDetails");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TblReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturnPurchaseInvoiceDetails_tblReturnPurchaseInvoice");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.TblReturnPurchaseInvoiceDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.tblReturnPurchaseInvoiceDetails_tblPurchaseInvoiceDetails");
            });

            modelBuilder.Entity<TblReturnPurchaseInvoicePay>(entity =>
            {
                entity.ToTable("tblReturnPurchaseInvoicePay");

                entity.HasIndex(e => e.InvoiceId, "tblReturnPurchaseInvoicePay_InvoiceIdIndex");
            });

            modelBuilder.Entity<TblReturnSaleInvoice>(entity =>
            {
                entity.ToTable("tblReturnSaleInvoice");

                entity.HasIndex(e => new { e.CustomerId, e.InvoiceDate }, "NCL_tblReturnSaleInvoice");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ReturnDiscount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblReturnSaleInvoices)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturnSaleInvoice_tblCustomers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblReturnSaleInvoices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblReturnSaleInvoice_tblUser");
            });

            modelBuilder.Entity<TblReturnSaleInvoiceDetail>(entity =>
            {
                entity.ToTable("tblReturnSaleInvoiceDetails");

                entity.HasIndex(e => e.SalePurchaseDetailId, "CL_tblReturnSaleInvoiceDetails_SalePurchaseDetailId");

                entity.HasIndex(e => e.MasterId, "tblReturnSaleInvoiceDetails_MasterId_SalePurchaseDetailId");

                entity.Property(e => e.DetailDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SalesPrice).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TblReturnSaleInvoiceDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturnSaleInvoiceDetails_tblReturnSaleInvoice");

                entity.HasOne(d => d.SalePurchaseDetail)
                    .WithMany(p => p.TblReturnSaleInvoiceDetails)
                    .HasForeignKey(d => d.SalePurchaseDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturnSaleInvoiceDetails_tblSalePurchaseDetail");
            });

            modelBuilder.Entity<TblReturnSaleInvoiceReceiver>(entity =>
            {
                entity.ToTable("tblReturnSaleInvoiceReceiver");

                entity.HasIndex(e => e.InvoiceId, "tblReturnSaleInvoiceReceiver_InvoiceIdIndex");
            });

            modelBuilder.Entity<TblSale>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSales");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<TblSale1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSale1");

                entity.Property(e => e.Barcode).HasMaxLength(255);

                entity.Property(e => e.DdPrice).HasColumnName("ddPrice");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SourceOfDrug)
                    .HasMaxLength(255)
                    .HasColumnName("Source Of Drug");

                entity.Property(e => e.Text74).HasMaxLength(255);

                entity.Property(e => e.Text80).HasMaxLength(255);

                entity.Property(e => e.Type).HasMaxLength(255);
            });

            modelBuilder.Entity<TblSaleInvoice>(entity =>
            {
                entity.ToTable("tblSaleInvoice");

                entity.HasIndex(e => e.InvoiceDate, "CL_InvoiceDate");

                entity.HasIndex(e => new { e.CustomerId, e.InvoiceDate }, "NCL_tblSaleInvoice");

                entity.HasIndex(e => e.InvoiceNum, "UQ__tblSaleInvoice_InvoiceNum")
                    .IsUnique();

                entity.HasIndex(e => e.CustomerId, "tblSaleInvoice_CustomerId");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.InvoiceProfit).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TaxiFare).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.VisitNum).HasMaxLength(200);

                entity.Property(e => e.WorkerFare).HasColumnType("numeric(20, 2)");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.TblSaleInvoiceCreatedUsers)
                    .HasForeignKey(d => d.CreatedUserId)
                    .HasConstraintName("FK_tblSaleInvoice_tblUser");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblSaleInvoices)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSaleInvoice_tblCustomers");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.TblSaleInvoiceModifiedUsers)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSaleInvoice_tblUserModified");
            });

            modelBuilder.Entity<TblSaleInvoice1>(entity =>
            {
                entity.ToTable("tblSaleInvoice1");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.InvoiceProfit).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblSaleInvoice2>(entity =>
            {
                entity.ToTable("tblSaleInvoice2");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.InvoiceProfit).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblSaleInvoiceDetail>(entity =>
            {
                entity.ToTable("tblSaleInvoiceDetails");

                entity.HasIndex(e => e.MasterId, "CL_MasterIdNew");

                entity.HasIndex(e => e.MedicineId, "CL_MedicineId_tblSaleInvoiceDetails");

                entity.HasIndex(e => e.PurchaseInvoiceDetailId, "CL_PurchaseInvoiceDetailId");

                entity.Property(e => e.Consideration).IsRequired();

                entity.Property(e => e.DetailDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Paakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SalesPrice).HasColumnType("numeric(20, 10)");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TblSaleInvoiceDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSaleInvoiceDetails_tbltblSaleInvoice");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblSaleInvoiceDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSaleInvoiceDetails_tblUserModified");
            });

            modelBuilder.Entity<TblSaleInvoiceDetails1>(entity =>
            {
                entity.ToTable("tblSaleInvoiceDetails1");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Consideration).IsRequired();

                entity.Property(e => e.DetailDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 1)");

                entity.Property(e => e.SalesPrice).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblSaleInvoiceDetails2>(entity =>
            {
                entity.ToTable("tblSaleInvoiceDetails2");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Consideration).IsRequired();

                entity.Property(e => e.DetailDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 1)");

                entity.Property(e => e.SalesPrice).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblSaleInvoiceLock>(entity =>
            {
                entity.ToTable("tblSaleInvoiceLock");

                entity.Property(e => e.SaleInvoiceNum).HasMaxLength(10);
            });

            modelBuilder.Entity<TblSaleInvoiceReciever>(entity =>
            {
                entity.ToTable("tblSaleInvoiceReciever");

                entity.HasIndex(e => e.InvoiceId, "tblSaleInvoiceReciever_InvoiceIdIndex");
            });

            modelBuilder.Entity<TblSalePurchaseDetail>(entity =>
            {
                entity.ToTable("tblSalePurchaseDetail");

                entity.HasIndex(e => e.PurchaseInvoiceDetailId, "NCL_PurchaseInvoiceDetailId");

                entity.HasIndex(e => new { e.SaleInvoiceDetailId, e.PurchaseInvoiceDetailId }, "SaleInvoiceDetailId_PurchaseInvoiceDetailId");

                entity.HasIndex(e => e.SaleInvoiceDetailId, "_dta_index_tblSalePurchaseDetails_113_10525828348__K5_K3_1");

                entity.HasIndex(e => e.PurchaseInvoiceDetailId, "_dta_index_tblSalePurchaseDetails_13_10525828348__K5_K3_1");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.PurchaseInvoiceDetail)
                    .WithMany(p => p.TblSalePurchaseDetails)
                    .HasForeignKey(d => d.PurchaseInvoiceDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tblSalePurchaseDetail");
            });

            modelBuilder.Entity<TblSettingValue>(entity =>
            {
                entity.ToTable("tblSettingValue");

                entity.Property(e => e.Sname)
                    .HasMaxLength(255)
                    .HasColumnName("SName");

                entity.Property(e => e.Value).HasMaxLength(255);
            });

            modelBuilder.Entity<TblSii>(entity =>
            {
                entity.ToTable("tblSIIs");

                entity.Property(e => e.Sii)
                    .IsRequired()
                    .HasColumnName("SII");
            });

            modelBuilder.Entity<TblSmshistory>(entity =>
            {
                entity.ToTable("tblSMSHistory");

                entity.Property(e => e.Message).HasMaxLength(600);

                entity.Property(e => e.Mob).HasMaxLength(15);

                entity.Property(e => e.Smsdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SMSDate");

                entity.Property(e => e.Smsstatus).HasColumnName("SMSStatus");
            });

            modelBuilder.Entity<TblSmstemplate>(entity =>
            {
                entity.ToTable("tblSMSTemplate");

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.Smstemplate)
                    .HasMaxLength(500)
                    .HasColumnName("SMSTemplate");
            });

            modelBuilder.Entity<TblSoftwareSetting>(entity =>
            {
                entity.ToTable("tblSoftwareSetting");

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.Company).HasMaxLength(50);

                entity.Property(e => e.DateFormat).HasMaxLength(200);

                entity.Property(e => e.EftetahSandoq).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.EftetahSandoqDate).HasColumnType("datetime");

                entity.Property(e => e.Esale).HasColumnName("ESale");

                entity.Property(e => e.Exd).HasColumnName("EXD");

                entity.Property(e => e.Exds).HasColumnName("EXDS");

                entity.Property(e => e.LatestJardDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectType).HasMaxLength(50);
            });

            modelBuilder.Entity<TblSoftwareSettingSecond>(entity =>
            {
                entity.ToTable("tblSoftwareSettingSecond");

                entity.Property(e => e.Company).HasMaxLength(50);

                entity.Property(e => e.PreSaleInvoiceNum).HasMaxLength(50);

                entity.Property(e => e.Warehouse).HasMaxLength(50);
            });

            modelBuilder.Entity<TblSummary>(entity =>
            {
                entity.ToTable("tblSummary");

                entity.Property(e => e.CostAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.RebhDamagedAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.RebhProfitAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.ReturnSaleAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SaleAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SaleProfit).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SummDate).HasColumnType("date");
            });

            modelBuilder.Entity<TblSuppAcc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSuppAcc");

                entity.Property(e => e.CustomersName).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.RecieveDate).HasMaxLength(50);

                entity.Property(e => e.RecievedAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.RemainAmount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SaleInvoiceNum).HasMaxLength(50);

                entity.Property(e => e.TotalCustomerPaid).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalCustomerRecieved).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalCustomerReturnSale).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalCustomerSale).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalPaid).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalRecieved).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalReturnSale).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.TotalSale).HasColumnType("numeric(20, 2)");
            });

            modelBuilder.Entity<TblSuppAcc1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSuppAcc1");

                entity.Property(e => e.CreatedUsername).HasMaxLength(250);

                entity.Property(e => e.CustomersName).HasMaxLength(100);

                entity.Property(e => e.Date).HasMaxLength(50);

                entity.Property(e => e.EndRemain).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceNum).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUsername).HasMaxLength(250);

                entity.Property(e => e.MyDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.Property(e => e.RebhDamaged).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RecievedAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.RemainAmount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalCustomerPaid).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalCustomerRebhDamaged).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalCustomerRecieved).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalCustomerReturnSale).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalCustomerSale).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalPaid).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalRecieved).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalReturnSale).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.TotalSale).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblSuppAcc2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSuppAcc2");

                entity.Property(e => e.CreatedUsername).HasMaxLength(250);

                entity.Property(e => e.Date).HasMaxLength(50);

                entity.Property(e => e.EndRemain).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.InvoiceNum).HasMaxLength(50);

                entity.Property(e => e.MainInvoiceNum).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUsername).HasMaxLength(250);

                entity.Property(e => e.MyDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.Property(e => e.RecievedAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RemainAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.SupplierName).HasMaxLength(100);

                entity.Property(e => e.TotalPaid).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalPurchase).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalRecieved).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalReturnPurchase).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierPaid).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierPurchase).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierRecieved).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierReturnPurchase).HasColumnType("numeric(20, 5)");
            });

            modelBuilder.Entity<TblSuppAcc3>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSuppAcc3");

                entity.Property(e => e.CreatedUsername).HasMaxLength(250);

                entity.Property(e => e.CustomersName).HasMaxLength(100);

                entity.Property(e => e.Date).HasMaxLength(50);

                entity.Property(e => e.EndRemain).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.EndRemainSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.InvoiceNum).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUsername).HasMaxLength(250);

                entity.Property(e => e.MyDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.PaidAmountSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.Property(e => e.RecievedAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RecievedAmountSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RemainAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RemainAmountSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerPaid).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerPaidSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerRecieved).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerRecievedSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerReturnSale).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerReturnSaleSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerSale).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalCustomerSaleSecond).HasColumnType("numeric(20, 5)");
            });

            modelBuilder.Entity<TblSuppAcc4>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSuppAcc4");

                entity.Property(e => e.Date).HasMaxLength(50);

                entity.Property(e => e.EndRemain).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.EndRemainSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.InvoiceNum).HasMaxLength(50);

                entity.Property(e => e.MainInvoiceNum).HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MyDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.PaidAmountSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.QabzNum).HasMaxLength(20);

                entity.Property(e => e.RecievedAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RecievedAmountSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RemainAmount).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.RemainAmountSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.SuppliersName).HasMaxLength(100);

                entity.Property(e => e.TotalSupplierPaid).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierPaidSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierPurchase).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierPurchaseSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierRecieved).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierRecievedSecond).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierReturnPurchase).HasColumnType("numeric(20, 5)");

                entity.Property(e => e.TotalSupplierReturnPurchaseSecond).HasColumnType("numeric(20, 5)");
            });

            modelBuilder.Entity<TblSupplier>(entity =>
            {
                entity.ToTable("tblSuppliers");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(15);

                entity.Property(e => e.Mob).HasMaxLength(15);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Saccount)
                    .HasColumnType("decimal(20, 5)")
                    .HasColumnName("SAccount");

                entity.Property(e => e.SaccountSecond)
                    .HasColumnType("decimal(20, 5)")
                    .HasColumnName("SAccountSecond");

                entity.Property(e => e.StatusC).HasMaxLength(1);

                entity.Property(e => e.Tel).HasMaxLength(15);

                entity.Property(e => e.Website).HasMaxLength(100);
            });

            modelBuilder.Entity<TblSuppliers1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblSuppliers1");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<TblTablesList>(entity =>
            {
                entity.ToTable("tblTablesList");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblTempDate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTempDate");
            });

            modelBuilder.Entity<TblTempInt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTempInt");
            });

            modelBuilder.Entity<TblTempNumeric>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTempNumeric");

                entity.Property(e => e.TempVal).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblTempNvarchar>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTempNVarchar");
            });

            modelBuilder.Entity<TblTest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTest");

                entity.Property(e => e.ControlName).HasMaxLength(255);

                entity.Property(e => e.Discreption).HasMaxLength(255);

                entity.Property(e => e.FormName).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
            });

            modelBuilder.Entity<TblTheme>(entity =>
            {
                entity.ToTable("tblTheme");

                entity.Property(e => e.Name).HasMaxLength(70);
            });

            modelBuilder.Entity<TblTopMedicine>(entity =>
            {
                entity.ToTable("tblTopMedicine");

                entity.HasOne(d => d.Med)
                    .WithMany(p => p.TblTopMedicines)
                    .HasForeignKey(d => d.MedId)
                    .HasConstraintName("FK_tblTopMedicine_tblMedicine");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tblUser");

                entity.Property(e => e.Fname)
                    .HasMaxLength(50)
                    .HasColumnName("FName");

                entity.Property(e => e.Lname)
                    .HasMaxLength(50)
                    .HasColumnName("LName");

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pass1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Pass2).HasMaxLength(100);

                entity.Property(e => e.Pass3).HasMaxLength(100);

                entity.Property(e => e.Pass4).HasMaxLength(100);

                entity.HasOne(d => d.Font)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.FontId)
                    .HasConstraintName("FK_tblUser_tblFont");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.ThemeId)
                    .HasConstraintName("FK_tblUser_tblTheme");
            });

            modelBuilder.Entity<TblUserFormAccess>(entity =>
            {
                entity.ToTable("tblUserFormAccess");
            });

            modelBuilder.Entity<TblWareHouseHandling>(entity =>
            {
                entity.ToTable("tblWareHouseHandling");

                entity.Property(e => e.DamagedNum).HasMaxLength(10);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.PurchaseInvoiceNum).HasMaxLength(10);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblWareHouseHandlingDetail>(entity =>
            {
                entity.ToTable("tblWareHouseHandlingDetail");

                entity.Property(e => e.BujNumber).HasMaxLength(50);

                entity.Property(e => e.Consideration).HasMaxLength(250);

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPacket).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Packet).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.SalePrice).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<TblWarehouseTransferring>(entity =>
            {
                entity.ToTable("tblWarehouseTransferring");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<TblWarehouseTransferringDetail>(entity =>
            {
                entity.ToTable("tblWarehouseTransferringDetails");

                entity.Property(e => e.DetailDate).HasColumnType("datetime");

                entity.Property(e => e.FreeNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.NumInPaakat).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.Paakat).HasColumnType("numeric(20, 7)");

                entity.HasOne(d => d.Master)
                    .WithMany(p => p.TblWarehouseTransferringDetails)
                    .HasForeignKey(d => d.MasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWarehouseTransferringDetails_tblWarehouseTransferring");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.TblWarehouseTransferringDetails)
                    .HasForeignKey(d => d.PurchaseId)
                    .HasConstraintName("FK_tblWarehouseTransferringDetails_tblPurchaseInvoiceDetails");
            });

            modelBuilder.Entity<TblWebFormAccess>(entity =>
            {
                entity.ToTable("tblWebFormAccess");

                entity.HasOne(d => d.Access)
                    .WithMany(p => p.TblWebFormAccessAccesses)
                    .HasForeignKey(d => d.AccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWebFormAccess_tblWebFormtblAccess");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.TblWebFormAccessForms)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWebFormAccess_tblWebFormtblAccess1");
            });

            modelBuilder.Entity<TblWebFormtblAccess>(entity =>
            {
                entity.ToTable("tblWebFormtblAccess");

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Ename)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("EName");

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("FName");
            });

            modelBuilder.Entity<TblWebUser>(entity =>
            {
                entity.ToTable("tblWebUser");

                entity.Property(e => e.Fname)
                    .HasMaxLength(50)
                    .HasColumnName("FName");

                entity.Property(e => e.Lname)
                    .HasMaxLength(50)
                    .HasColumnName("LName");

                entity.Property(e => e.MacAddress).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<TblWebUserCustomer>(entity =>
            {
                entity.ToTable("tblWebUserCustomer");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblWebUserCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_tblWebUserCustomer_tblCustomers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblWebUserCustomers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblWebUserCustomer_tblWebUser");
            });

            modelBuilder.Entity<TblWebUserFormAccess>(entity =>
            {
                entity.ToTable("tblWebUserFormAccess");

                entity.HasOne(d => d.FormAccess)
                    .WithMany(p => p.TblWebUserFormAccesses)
                    .HasForeignKey(d => d.FormAccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWebUserFormAccess_tblWebFormAccess");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblWebUserFormAccesses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblWebUserFormAccess_tblWebUser");
            });

            modelBuilder.Entity<TblZone>(entity =>
            {
                entity.ToTable("tblZones");

                entity.HasIndex(e => e.Name, "UQ__tblZones__737584F61F7A7FA8")
                    .IsUnique();

                entity.Property(e => e.Code).HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TblZoneMandoob>(entity =>
            {
                entity.ToTable("tblZoneMandoobs");
            });

            modelBuilder.Entity<VwActiveCustomerType>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwActiveCustomerType");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<VwActiveSupplierType>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwActiveSupplierType");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<VwCost>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCost");

                entity.Property(e => e.CostTypeName).HasMaxLength(100);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("numeric(20, 7)");
            });

            modelBuilder.Entity<VwNumMidicine>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwNumMidicine");

                entity.Property(e => e.Discount).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.Num).HasColumnType("numeric(21, 7)");

                entity.Property(e => e.SalesPrice).HasColumnType("numeric(20, 10)");
            });

            modelBuilder.Entity<VwOrderPointList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwOrderPointList");

                entity.Property(e => e.FormName).HasMaxLength(100);

                entity.Property(e => e.JoineryName).HasMaxLength(400);

                entity.Property(e => e.ProducerName).HasMaxLength(100);
            });

            modelBuilder.Entity<VwReminingMedicineNumReport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwReminingMedicineNumReport");

                entity.Property(e => e.CurrentNum).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.FormName).HasMaxLength(100);

                entity.Property(e => e.JoineryName).HasMaxLength(1000);

                entity.Property(e => e.ProducerName).HasMaxLength(100);
            });

            modelBuilder.Entity<VwReturnPurchaseInvoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwReturnPurchaseInvoice");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.MainInvoiceNum).HasMaxLength(500);

                entity.Property(e => e.ReasonName).HasMaxLength(100);

                entity.Property(e => e.ReturnDiscount).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.SupplierName).HasMaxLength(100);

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(38, 6)");

                entity.Property(e => e.Username).HasMaxLength(154);
            });

            modelBuilder.Entity<VwShantyonPurchaseInvoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwShantyonPurchaseInvoice");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.JoineryName).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Num).HasColumnType("numeric(20, 7)");

                entity.Property(e => e.PurchasePrice).HasColumnType("numeric(20, 7)");
            });

            //  modelBuilder.HasDbFunction(typeof(DMSContext)
            //.GetMethod(nameof(FN_MedicineNum)))
            //.HasName("FN_MedicineNum");

            modelBuilder.HasDbFunction(typeof(DMSContext)
          .GetMethod(nameof(FN_SaleFromProducer)))
          .HasName("FN_SaleFromProducer");

            modelBuilder.Entity<FN_MedicineNumModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<FN_SaleFromProducerModel>(entity =>
            {
                entity.HasNoKey();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //public IQueryable<FN_MedicineNumModel> FN_MedicineNum(Nullable<int> MedicineId)
        //=> FromExpression(() => FN_MedicineNum(MedicineId));

        public IQueryable<FN_SaleFromProducerModel> FN_SaleFromProducer(Nullable<int> UserId, Nullable<int> ProducerId, Nullable<int> MedicineId, Nullable<int> CustomerId, Nullable<int> MandoobId, Nullable<int> CustomerTypeId, Nullable<int> NumFrom, Nullable<int> NumTo, Nullable<int> FreeNumFrom, Nullable<int> FreeNumTo, Nullable<DateTime> DateFrom, Nullable<DateTime> DateTo, Nullable<bool> Dinar, Nullable<DateTime> DateFormat)
        => FromExpression(() => FN_SaleFromProducer(UserId, ProducerId, MedicineId, CustomerId, MandoobId, CustomerTypeId, NumFrom, NumTo, FreeNumFrom, FreeNumTo, DateFrom, DateTo, Dinar, DateFormat));

    }
}

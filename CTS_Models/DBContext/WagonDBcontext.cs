namespace CTS_Models.DBContext
{
    using System.Data.Entity;
	using CTS_Models.WagonDB;

	public partial class WagonDBcontext : DbContext
    {
        public WagonDBcontext()
            : base("name=WagonDB")
        {
        }

        public virtual DbSet<napravlenie> napravlenie { get; set; }
        public virtual DbSet<objects> objects { get; set; }
        public virtual DbSet<operators> operators { get; set; }
        public virtual DbSet<otprav_poluch> otprav_poluch { get; set; }
        public virtual DbSet<recogn> recogn { get; set; }
        public virtual DbSet<scales> scales { get; set; }
        public virtual DbSet<sostav> sostav { get; set; }
        public virtual DbSet<vagon_nums> vagon_nums { get; set; }
        public virtual DbSet<ves_telega> ves_telega { get; set; }
        public virtual DbSet<ves_vagon> ves_vagon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<napravlenie>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<napravlenie>()
                .Property(e => e.display_name)
                .IsUnicode(false);

            modelBuilder.Entity<objects>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<objects>()
                .Property(e => e.display_name)
                .IsUnicode(false);

            modelBuilder.Entity<operators>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<operators>()
                .Property(e => e.pass)
                .IsUnicode(false);

            modelBuilder.Entity<operators>()
                .Property(e => e.display_name)
                .IsUnicode(false);

            modelBuilder.Entity<otprav_poluch>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<otprav_poluch>()
                .Property(e => e.display_name)
                .IsUnicode(false);

            modelBuilder.Entity<recogn>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<scales>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sostav>()
                .Property(e => e.number)
                .IsUnicode(false);

            modelBuilder.Entity<vagon_nums>()
                .Property(e => e.number)
                .IsUnicode(false);

            modelBuilder.Entity<vagon_nums>()
                .Property(e => e.number_operator)
                .IsUnicode(false);

            modelBuilder.Entity<vagon_nums>()
                .Property(e => e.camera)
                .IsUnicode(false);

            modelBuilder.Entity<ves_telega>()
                .Property(e => e.speed)
                .IsUnicode(false);

            modelBuilder.Entity<ves_vagon>()
                .Property(e => e.vagon_num)
                .IsUnicode(false);

            modelBuilder.Entity<ves_vagon>()
                .Property(e => e.nakladn)
                .IsUnicode(false);

            modelBuilder.Entity<ves_vagon>()
                .Property(e => e.gruz)
                .IsUnicode(false);

            modelBuilder.Entity<ves_vagon>()
                .Property(e => e.speed)
                .IsUnicode(false);
        }
    }
}

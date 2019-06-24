namespace Server
{
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext() : base("name=DataContext")
        {
        }

        public DbSet<Street> Streets { get; set; }
    }
}
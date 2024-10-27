namespace Pcf.Administration.DataAccess.Data
{
	public class EfDbInitializer : IDbInitializer
	{
		private readonly DataContext _dataContext;
        public EfDbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void Initialize()
		{
			_dataContext.Database.EnsureDeleted();
			_dataContext.Database.EnsureCreated();

			_dataContext.AddRange(FakeDataFactory.Employees);
			_dataContext.AddRange(FakeDataFactory.Roles);
			_dataContext.SaveChanges();
		}
	}
}

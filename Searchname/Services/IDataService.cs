namespace Searchname.Services
{
    public interface IDataService
    {
        Task<IList<Models.Result>> GetResultsByName(string name);
    }
}

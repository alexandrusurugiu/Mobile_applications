using System;
using System.Collections.Generic;
using System.Text;

namespace Searchname.Data
{
    public interface IRepository
    {
        Task<IList<Models.Result>> GetResultsByName(string name);

        Task SaveResults(IList<Models.Result> items);
    }
}

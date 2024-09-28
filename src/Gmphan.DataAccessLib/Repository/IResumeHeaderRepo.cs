using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IResumeHeaderRepo : IRepository<ResumeHeader>
    {
        Task UpdateAsync(ResumeHeader obj);
    }
}
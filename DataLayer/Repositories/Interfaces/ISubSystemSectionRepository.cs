using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISubSystemSectionRepository : IRepository<SubSystemSection>
    {
        IEnumerable<SubSystem> GetSubSystemBySectionTypeId(int sectionTypeId);
        IEnumerable<SubSystemSection> GetSubSystemSectionBySubSystemId(int subSystemId);
    }
}

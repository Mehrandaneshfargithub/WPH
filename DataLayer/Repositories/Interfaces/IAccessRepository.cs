﻿using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IAccessRepository : IRepository<Access>
    {
        IEnumerable<Access> GetSubsystemAccess(int subSystemId);
    }
}

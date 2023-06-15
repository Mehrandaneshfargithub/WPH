using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface ILicenceKeyRepository : IRepository<LicenceKey>
    {
        LicenceKey GetLastLicence(string computerSerial);
    }
}

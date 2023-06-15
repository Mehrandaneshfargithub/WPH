using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IGeekConfigManager
    {
        string NorthwindConnection { get; }

        string EmailID { get; }

        string AccountKey { get; }

        string GetConnectionString(string connectionName);

        IConfigurationSection GetConfigurationSection(string Key);
    }
}

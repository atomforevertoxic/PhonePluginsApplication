using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesServicePlugin
{
    [Author(Name = "Ivan Shatalov")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info(AppDomain.CurrentDomain.BaseDirectory + "___EmployeeServicePlugin");
            return new List<DataTransferObject>(args);
        }
    }
}

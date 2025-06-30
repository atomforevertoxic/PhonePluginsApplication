using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneApp.Domain;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;

namespace EmployeesLoaderPlugin
{

  [Author(Name = "Ivan Petrov")]
  public class Plugin : IPluggable
  {
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    private static EmployeesServicePlugin.Plugin _service;
    public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
    {
      logger.Info("Starting Viewer");
      logger.Info("Type q or quit to exit");
      logger.Info("Available commands: list, add, del");

      var employeesList = args.Cast<EmployeesDTO>().ToList();

      _service = new EmployeesServicePlugin.Plugin(employeesList);

      string command = " ";
      
      while (!command.Equals("q") && !command.Equals("quit"))
      {
        Console.Write("> ");
        command = Console.ReadLine();

        switch(command)
        {
          case "list":
            _service.ShowEmployeesList();
            break;

          case "add":
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            
            _service.AddEmployeeToList(name, phone);
                        
            break;
          case "del":
            Console.Write("Index of employee to delete: ");
            string index = Console.ReadLine();

            _service.DeleteEmployeeFromList(index);

            break;
        }
        command = command.ToLower().Trim(' ');
        Console.WriteLine("");
      }

      return employeesList.Cast<DataTransferObject>();
    }
  }
}

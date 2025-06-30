using Newtonsoft.Json.Linq;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesServicePlugin
{
    [Author(Name = "Ivan Shatalov")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private const string apiURL = "https://dummyjson.com/users";
        private static List<EmployeesDTO> employeesList = new List<EmployeesDTO>();


        public Plugin() { }

        public Plugin(List<EmployeesDTO> employees)
        {
            employeesList = employees;
        }


        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("EmployeesServicePlugin started");

            var employees = args.Cast<EmployeesDTO>().ToList();

            try
            {
                var apiUsers = LoadUsersFromApi().Result;
                employees.AddRange(apiUsers);
                logger.Info($"Loaded {apiUsers.Count} employees from API dummyjson.com");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to load users from API");
            }


            return employees.Cast<DataTransferObject>();
        }



        private async Task<List<EmployeesDTO>> LoadUsersFromApi()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiURL);
                var json = JObject.Parse(response);

                return json["users"].Select(user =>
                {
                    var employee = new EmployeesDTO
                    {
                        Name = $"{user["firstName"]} {user["lastName"]}"
                    };
                    employee.AddPhone(user["phone"].ToString());
                    return employee;
                }).ToList();
            }
        }

        public void ShowEmployeesList()
        {
            int index = 0;
            logger.Info(employeesList.Count);
            foreach (var employee in employeesList)
            {
                Console.WriteLine($"{index} Name: {employee.Name} | Phone: {employee.Phone}");
                ++index;
            }
        }


        public void AddEmployeeToList(string name, string phone)
        {

            int phoneNumber = 0;
            if (!Int32.TryParse(phone, out phoneNumber))
            {
                logger.Error("Phone number contains not int characters!");
            }
            else
            {
                Console.WriteLine($"{name} added to employees");
                employeesList.Add(new EmployeesDTO()
                {
                    Name = name,
                    Phone = phone
                });
            }
        }

        public void DeleteEmployeeFromList(string index)
        {
            if (!Int32.TryParse(index, out int indexToDelete))
            {
                logger.Error("Not an index or not an int value!");
            }
            else
            {
                if (indexToDelete >= 0 && indexToDelete < employeesList.Count())
                {
                    employeesList.RemoveAt(indexToDelete);
                }
            }
        }
    }
}

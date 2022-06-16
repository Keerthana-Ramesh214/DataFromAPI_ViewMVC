using DataFromAPI_Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataFromAPI_Mvc.Controllers
{
    public class EmployeeController : Controller
    {
        string Baseurl = "https://localhost:44370/";
        public async Task<IActionResult> EmployeeDetails()
            {
                List<Employee> Empdetails = new List<Employee>();

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("https://localhost:44370/api/Employees");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    Empdetails = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);

                    }
                    //returning the employee list to view  
                    return View(Empdetails);
                }
            }
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(Employee e)
        {
            Employee Emplobj = new Employee();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44370/api/Employees", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Emplobj = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return RedirectToAction("EmployeeDetails");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            Employee e = new();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44370/api/Employees/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        e = JsonConvert.DeserializeObject<Employee>(apiResponse);
                    }
                }
                catch (Exception)
                {
                    return View("ServerNotFound");
                }
            }
            return View(e);
        }


        [HttpGet]
        public async Task<ActionResult> UpdateEmployee(int id)
        {
            Employee emp = new Employee();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Employees/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    emp = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return View(emp);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateEmployee(Employee e)
        {
            Employee receivedemp = new Employee();
            using (var httpClient = new HttpClient())
            {
                #region
                //var content = new MultipartFormDataContent();
                //content.Add(new StringContent(reservation.Empid.ToString()), "Empid");
                //content.Add(new StringContent(reservation.Name), "Name");
                //content.Add(new StringContent(reservation.Gender), "Gender");
                //content.Add(new StringContent(reservation.Newcity), "Newcity");
                //content.Add(new StringContent(reservation.Deptid.ToString()), "Deptid");
                #endregion
                int id = e.Eid;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:44370/api/Employees/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedemp = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return RedirectToAction("EmployeeDetails");
        }
        [HttpGet]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            TempData["empid"] = id;
            Employee e = new Employee();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Employees/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    e = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return View(e);
        }
        [HttpPost]
        // [ActionName("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployee(Employee e)
        {
            int empid = Convert.ToInt32(TempData["empid"]);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44370/api/Employees/" + empid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("EmployeeDetails");
        }
    }
}


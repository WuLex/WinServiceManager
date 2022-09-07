using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using WServiceWebManager.Models;

namespace WServiceWebManager.Controllers
{
    public class ManagerController : Controller
    {
        public List<ServiceModel> list = new List<ServiceModel>();

        public IActionResult Index()
        {
            //ServiceController[] myServices = ServiceController.GetServices();

            List<ServiceController> myServiceList = ServiceController.GetServices().OrderBy(s => s.Status).ToList();
            foreach (var item in myServiceList)
            {
                //if (item.ServiceType == ServiceType.Win32OwnProcess && item.DisplayName.Contains("memcached"))
                if (item.ServiceType == ServiceType.Win32OwnProcess)
                {
                    ServiceModel model = new ServiceModel();
                    model.ServiceName = item.ServiceName;
                    model.DisplayName = item.DisplayName;
                    model.IsRunning = item.Status == ServiceControllerStatus.Running;
                    list.Add(model);
                }
            }

            return View(list);
        }
    }
}
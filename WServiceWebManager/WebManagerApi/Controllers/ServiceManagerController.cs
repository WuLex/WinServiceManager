﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using WebManagerApi.Models;

namespace WebManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceManagerController : ControllerBase
    {
        public ServiceManagerController()
        { 
        
        }

        [HttpPost]
        public string ServiceOp(ServiceModelDto serviceModelDto)
        {
            //服务名
            string serviceName = serviceModelDto.ServiceName;
            //string serviceName = HttpContext.Request.Query["serviceName"];
            //操作类型【重启、停止、重启】
            string type = serviceModelDto.Type;

            //string type = HttpContext.Request.Query["type"];

            try
            {
                switch (type)
                {
                    case "start":
                        StartService(serviceName);
                        break;
                    case "stop":
                        StopService(serviceName);
                        break;
                    case "reset":
                        ResetService(serviceName);
                        break;
                    default:
                        ResetService(serviceName);
                        break;
                }

                return "ok";
                //HttpContext.Response.WriteAsync("ok");
            }
            catch (Exception ex)
            {
                return ex.Message;
                //HttpContext.Response.WriteAsync(ex.Message);
            }

        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        private void StartService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
                service.Close();
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        private void StopService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
                service.Close();
            }
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        private void ResetService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Running || service.Status == ServiceControllerStatus.Stopped)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
                service.Close();
            }
        }
    }


}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.ServiceProcess;
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

            //操作类型【重启、停止、重启】
            string type = serviceModelDto.Type;

            //string serviceName = HttpContext.Request.Query["serviceName"];
            //string type = HttpContext.Request.Query["type"];

            try
            {
                switch (type)
                {
                    case "start":
                        StartService(serviceName);
                        //StartServiceByCmd(serviceName);
                        break;

                    case "stop":
                        StopService(serviceName);
                        //StopServiceByCmd(serviceName);
                        break;

                    case "reset":
                        ResetService(serviceName);
                        break;

                    default:
                        //ResetService(serviceName);
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
        private void StartService(string serviceName, int timeoutMilliseconds = 60000)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                service.Close();
            }

            //if (!SysSecurity.IsAdministrator())
            //{
            //    SysSecurity.RunProcess(serviceName, null);
            //}
            //else
            //{
            //}
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        private void StopService(string serviceName)
        {
            using (ServiceController service = new ServiceController(serviceName))
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    service.Close();
                }
            }
            //if (!SysSecurity.IsAdministrator())
            //{
            //    SysSecurity.RunProcess(serviceName, null);
            //}
            //else
            //{
            //}
        }

        [HttpPost]
        public void ContinueService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Paused)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Continue();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public void PauseService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Pause();
                    service.WaitForStatus(ServiceControllerStatus.Paused, timeout);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 通过Cmd启动服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        private void StartServiceByCmd(string serviceName)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine("net start " + serviceName);
            proc.Close();
        }

        /// <summary>
        /// 通过Cmd停止服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        private void StopServiceByCmd(string serviceName)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine("net stop " + serviceName);
            proc.Close();
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
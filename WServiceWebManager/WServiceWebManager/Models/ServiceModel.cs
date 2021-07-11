using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WServiceWebManager.Models
{
    public class ServiceModel
    {
        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public bool IsRunning { get; set; }
    }
}

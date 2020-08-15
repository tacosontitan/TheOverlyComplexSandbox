using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Core {
    public class ModuleParameter {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DisplayElement DisplayElement { get; set; }
        public string RequestMessage { get; set; }
        public bool Required { get; set; }
        public object Value { get; set; }
    }
}
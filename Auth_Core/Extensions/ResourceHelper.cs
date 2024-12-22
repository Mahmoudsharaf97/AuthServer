using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;

namespace Auth_Core
{
    public  class ResourceHelper<T>
    {
        public readonly ComponentResourceManager resourceManager;
        public ResourceHelper()
        {
            this.resourceManager = new ComponentResourceManager(typeof(T));
        }
        public string GetValue(string key)
        {
            return resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }
 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Configuration;

namespace DemoVS4.Core.DAL
{
    public partial class dbTestDataContext: DataContext
    {
        public dbTestDataContext()
            : base(ConfigurationManager.ConnectionStrings["testConnectionString"].ConnectionString)
        {
            OnCreated();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WebSystemTemplet.Utility
{
    public class Config
    {
        public static string SourceVersion
        {
            get { return ConfigurationManager.AppSettings["source_version"]; }
        }
    }
}

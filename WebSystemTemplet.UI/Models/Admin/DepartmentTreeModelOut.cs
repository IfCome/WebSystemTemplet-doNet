using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSystemTemplet.UI.Models.Admin
{
    [Serializable]
    public class DepartmentTreeModelOut
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 部门负责人
        /// </summary>
        public string directorName { get; set; }

        /// <summary>
        /// 下级部门
        /// </summary>
        public List<DepartmentTreeModelOut> children { get; set; }
    }
}
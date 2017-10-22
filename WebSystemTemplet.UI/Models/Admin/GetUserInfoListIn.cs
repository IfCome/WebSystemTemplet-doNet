using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystemTemplet.UI.Models.Admin
{
    public class GetUserInfoListIn
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int RoleType { get; set; }

        /// <summary>
        /// 搜索词
        /// </summary>
        public string KeyWords { get; set; }


    }
}

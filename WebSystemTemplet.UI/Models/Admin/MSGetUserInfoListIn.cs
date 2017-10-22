using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystemTemplet.UI.Models.Admin
{
    public class MSGetUserInfoListIn
    {
        public string PageSize { get; set; }
        public string PageIndex { get; set; }

        /// <summary>
        /// 搜索词
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 岗位 类型
        /// </summary>
        public long PositionType { get; set; }

        /// <summary>
        /// 专业 ID
        /// </summary>
        public long MajorId { get; set; }

        /// <summary>
        /// 班级 ID
        /// </summary>
        public long ClassId { get; set; }
    }
}

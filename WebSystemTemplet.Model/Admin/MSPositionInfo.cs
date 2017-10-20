using System;

namespace WebSystemTemplet.Model.Admin
{
    /// <summary>
    /// MSPostionInfo
    /// </summary>
    public class MSPositionInfo
    {

        /// <summary>
        /// 岗位ID
        /// </summary>
        public long PositionID { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DepartmentID { get; set; }

        /// <summary>
        /// 岗位类型（10001-系统管理员，10002-配置管理员，20001-院长，30001-系主任，30002-助理，30003-讲师，30004-班主任，30005-助教，40001-班长，40002-学生）
        /// </summary>
        public int PositionType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime PositionName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public long UpdateUser { get; set; }

        /// <summary>
        /// 是否删除 1 已删除 ;0 未删除
        /// </summary>
        public byte Deleted { get; set; }

    }
}






using System;

namespace WebSystemTemplet.Model.Admin
{
    /// <summary>
    /// MSDepartmentInfo
    /// </summary>
    public class MSDepartmentInfo
    {

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DepartmentID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门级别 （1-学院; 2-专业; 3-班级）
        /// </summary>
        public int DepartmentLevel { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        public long ParentID { get; set; }

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






using System;

namespace WebSystemTemplet.Model.Admin
{
    /// <summary>
    /// MSActionInfo
    /// </summary>
    public class MSActionInfo
    {

        /// <summary>
        /// 权限ID
        /// </summary>
        public long ActionID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        public string ActionCode { get; set; }

        /// <summary>
        /// 父级权限码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 显示权重
        /// </summary>
        public int ShowLevel { get; set; }

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






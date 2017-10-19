using System;

namespace WebSystemTemplet.Model.Admin
{
    /// <summary>
    /// MSPositionTypeActionGroup
    /// </summary>
    public class MSPositionTypeActionGroup
    {

        /// <summary>
        /// 岗位类型
        /// </summary>
        public int PositionType { get; set; }

        /// <summary>
        /// 权限组ID
        /// </summary>
        public long ActionGroupID { get; set; }

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






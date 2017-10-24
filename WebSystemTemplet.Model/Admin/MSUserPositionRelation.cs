using System;

namespace WebSystemTemplet.Model.Admin
{
    /// <summary>
    /// MSUserPositionRelation
    /// </summary>
    public class MSUserPositionRelation
    {
        /// <summary>
        /// 关系ID
        /// </summary>
        public long RelationID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 岗位ID
        /// </summary>
        public long PositionID { get; set; }

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






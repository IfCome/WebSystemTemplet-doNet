using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSystemTemplet.Model.Admin
{
    /// <summary>
    /// MSSystemOperateLog
    /// </summary>
    public class MSSystemOperateLog
    {

        /// <summary>
        /// LogID
        /// </summary>
        public string LogID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 操作类型（101-登录； 201-访问；301-操作；401-异常；501-其它）
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string LogTile { get; set; }

        /// <summary>
        /// 日志描述
        /// </summary>
        public string LogMsg { get; set; }

        /// <summary>
        /// 日志详情
        /// </summary>
        public string LogDetail { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 操作人用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string RealName { get; set; }
    }


}

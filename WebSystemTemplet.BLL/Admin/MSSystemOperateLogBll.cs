using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Model;

namespace WebSystemTemplet.BLL.Admin
{
    public class MSSystemOperateLogBll
    {
        public static bool AddLoginLog(string msg, string ipAddress)
        {
            Model.Admin.MSSystemOperateLog logEntity = new Model.Admin.MSSystemOperateLog()
            {
                LogID = Guid.NewGuid().ToString().Replace("-", ""),
                UserID = Identity.LoginUserInfo == null ? 0 : Identity.LoginUserInfo.UserID,
                LogType = (int)LogType.登录,
                LogTile = "登录",
                LogMsg = msg,
                LogDetail = "",
                IpAddress = ipAddress
            };
            return DAL.Admin.MSSystemOperateLogDal.Add(logEntity);
        }

        public static bool AddOperateLog(LogType logType, string msg, string detail, string ipAddress)
        {
            Model.Admin.MSSystemOperateLog logEntity = new Model.Admin.MSSystemOperateLog()
            {
                LogID = Guid.NewGuid().ToString().Replace("-", ""),
                UserID = Identity.LoginUserInfo == null ? 0 : Identity.LoginUserInfo.UserID,
                LogType = (int)logType,
                LogTile = logType.ToString(),
                LogMsg = msg,
                LogDetail = detail,
                IpAddress = ipAddress
            };
            return DAL.Admin.MSSystemOperateLogDal.Add(logEntity);
        }

        public static List<Model.Admin.MSSystemOperateLog> GetSingleUserTop10Logs(long userId)
        {
            return DAL.Admin.MSSystemOperateLogDal.GetTop10ListByUserID(userId);
        }

        public static List<Model.Admin.MSSystemOperateLog> GetTop10Logs()
        {
            return DAL.Admin.MSSystemOperateLogDal.GetTop10List();
        }
    }
}

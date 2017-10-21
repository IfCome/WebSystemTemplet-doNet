using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.BLL.Admin
{
    public static class MSUserInfoBll
    {
        public static List<Model.Admin.MSUserInfo> GetAllUserInfoList(SqlParams sqlParams, out int allCount)
        {
            allCount = 0;
            List<Model.Admin.MSUserInfo> msUserInfoList = DAL.Admin.MSUserInfoDal.GetPageListByCondition(sqlParams, out allCount);
            if (msUserInfoList == null)
            {
                msUserInfoList = new List<Model.Admin.MSUserInfo>();
            }
            return msUserInfoList;
        }


        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Model.Admin.MSUserInfo GetSingleUserInfo(long userId)
        {
            return DAL.Admin.MSUserInfoDal.GetUserInfoByID(userId);
        }

        /// <summary>
        /// 删除单个用户
        /// </summary>
        /// <param name="id"></param>
        public static bool DeleteSingleUserInfo(long id)
        {
            return DAL.Admin.MSUserInfoDal.DeleteByID(id);
        }


        #region 登录帐号相关

        /// <summary>
        /// 登陆检测
        /// </summary>
        public static Model.Admin.MSUserInfo Login(string UseName, string password, string ipAddress)
        {
            password = Security.getMD5ByStr(password);
            Model.Admin.MSUserInfo userInfo = DAL.Admin.MSUserInfoDal.GetInfoByUserNameAndPwd(UseName, password);
            if (userInfo != null)
            {
                Model.Identity.LoginUserInfo = userInfo;

                //TODO: 登录成功，记录日志
                //string logTitle = "登录系统";
                //string logMsg = "登录系统";
                //BLL.BackgroundUserBll_log.AddLog(logTitle, logMsg, ipAddress);

                // 修改最近登录时间
                DAL.Admin.MSUserInfoDal.UpdateLastLoginTimeByID(DateTime.Now, userInfo.UserID);
            }

            return userInfo;
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        public static bool IsExistUserName(string userName)
        {
            return DAL.Admin.MSUserInfoDal.GetCountByUserName(userName) > 0;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static bool UpdateUserPassword(long id, string newPassword)
        {
            newPassword = Security.getMD5ByStr(newPassword);
            return DAL.Admin.MSUserInfoDal.UpdataPasswordByID(id, newPassword);
        }

        #endregion
    }
}

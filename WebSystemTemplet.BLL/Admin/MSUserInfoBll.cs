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
            List<Model.Admin.MSUserInfo> msUserInfoList = DAL.Admin.MSUserInfo.GetPageListByCondition(sqlParams, out allCount);
            if (msUserInfoList == null)
            {
                msUserInfoList = new List<Model.Admin.MSUserInfo>();
            }
            return msUserInfoList;
        }
    }
}

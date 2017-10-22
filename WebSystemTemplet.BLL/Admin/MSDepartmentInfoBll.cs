using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.BLL.Admin
{
    public static class MSDepartmentInfoBll
    {
        public static string GetDepartmentNameById(long departmentId)
        {
            string cacheKey = Model.CacheKeyName.MS_CacheKey_PositionName.ToString();
            Dictionary<long, string> dicNameAndId = CacheHelper.GetCache(cacheKey) as Dictionary<long, string>;
            if (dicNameAndId == null)
            {
                dicNameAndId = new Dictionary<long, string>();
                List<Model.Admin.MSDepartmentInfo> departmentList = DAL.Admin.MSDepartmentInfoDal.GetAllDepartmentNameAndId();
                if (departmentList != null)
                {
                    foreach (var item in departmentList)
                    {
                        dicNameAndId.Add(item.DepartmentID, item.DepartmentName);
                    }
                }
                // 不设置过期时间，当更新组织架构时，清除缓存
                CacheHelper.SetCache(cacheKey, dicNameAndId);
            }
            return dicNameAndId.ContainsKey(departmentId) ? dicNameAndId[departmentId] : "";
        }

        public static List<Model.Admin.MSDepartmentInfo> GetAllMSDepartmentInfoList(SqlParams sqlParams, out int allCount)
        {
            allCount = 0;
            List<Model.Admin.MSDepartmentInfo> msUserInfoList = DAL.Admin.MSDepartmentInfoDal.GetPageListByCondition(sqlParams, out allCount);
            if (msUserInfoList == null)
            {
                msUserInfoList = new List<Model.Admin.MSDepartmentInfo>();
            }
            return msUserInfoList;
        }
    }
}

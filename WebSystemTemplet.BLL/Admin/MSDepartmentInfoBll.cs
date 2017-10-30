using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Model.Admin;
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

        public static List<Model.Admin.MSDepartmentInfo> GetAllDepartmentInfoByLevel(int departmentLevel)
        {
            //return DAL.Admin.MSDepartmentInfoDal.GetAllDepartmentNameAndId(departmentLevel);

            string cacheKey = Model.CacheKeyName.MS_CacheKey_PositionList.ToString();
            List<Model.Admin.MSDepartmentInfo> resultList = new List<Model.Admin.MSDepartmentInfo>();
            List<Model.Admin.MSDepartmentInfo> departmentList = CacheHelper.GetCache(cacheKey) as List<Model.Admin.MSDepartmentInfo>;
            if (departmentList == null)
            {
                departmentList = DAL.Admin.MSDepartmentInfoDal.GetAllDepartmentNameAndId();
                // 不设置过期时间，当更新组织架构时，清除缓存
                CacheHelper.SetCache(cacheKey, departmentList);
            }
            if (departmentList != null)
            {
                resultList = departmentList.Where(d => d.DepartmentLevel == departmentLevel).ToList();
            }
            return resultList;
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

        public static bool SaveDepartmentInfo(MSDepartmentInfo departmentInfo, out string errorMssg)
        {
            errorMssg = "保存失败！";

            // 判断部门名称是否存在
            int count = DAL.Admin.MSDepartmentInfoDal.GetCountByDepartmentNameAndId(departmentInfo.DepartmentName, departmentInfo.DepartmentID);
            if (count > 0)
            {
                errorMssg = string.Format("该{0}已经存在！", Enum.GetName(typeof(Model.DepartmentLevel), departmentInfo.DepartmentLevel));
                return false;
            }

            bool re;
            if (departmentInfo.DepartmentID == -1)
            {
                departmentInfo.DepartmentID = DAL.Admin.MSDepartmentInfoDal.Add(departmentInfo);
                re = departmentInfo.DepartmentID > 0;
                if (re)
                {
                    // 根据部门类型添加部门下的岗位
                    List<Model.Admin.MSPositionInfo> positionList = new List<MSPositionInfo>();
                    int maxCode = 0;
                    int minCode = 0;
                    switch (departmentInfo.DepartmentLevel)
                    {
                        //30001-系主任，30002-助理，30003-讲师，
                        case (int)Model.DepartmentLevel.专业: minCode = 30000; maxCode = 40000; break;

                        //40001-班主任，40002-助教，40003-班长，40004-学生
                        case (int)Model.DepartmentLevel.班级: minCode = 40000; maxCode = 50000; break;

                        //20001-院长，
                        case (int)Model.DepartmentLevel.学院: minCode = 20000; maxCode = 30000; break;

                        default: break;
                    }

                    foreach (Model.PositionType item in Enum.GetValues(typeof(Model.PositionType)))
                    {
                        int code = (int)item;
                        if (minCode < code && code < maxCode)
                        {
                            positionList.Add(new Model.Admin.MSPositionInfo
                            {
                                DepartmentID = departmentInfo.DepartmentID,
                                PositionName = item.ToString(),
                                PositionType = code,
                                CreateUser = Model.Identity.LoginUserInfo.UserID,
                                CreateTime = DateTime.Now,
                            });
                        }
                    }
                    DAL.Admin.MSPositionInfoDal.Add(positionList);
                }
            }
            else
            {
                re = DAL.Admin.MSDepartmentInfoDal.UpdateByDepartmentID(departmentInfo);
            }
            if (re)
            {
                // 清空组织架构缓存
                CacheHelper.RemoveCache(Model.CacheKeyName.MS_CacheKey_PositionName.ToString());
                CacheHelper.RemoveCache(Model.CacheKeyName.MS_CacheKey_PositionList.ToString());
            }
            return re;
        }

        public static bool SetDirectorInfo(long departmentId, long directorId, int positionCode, out string errorMsg)
        {
            errorMsg = "";
            //查询部门信息
            Model.Admin.MSDepartmentInfo departmentInfo = DAL.Admin.MSDepartmentInfoDal.GetByDepartmentID(departmentId);

            if (departmentInfo == null)
            {
                errorMsg = "数据不存在，请刷新列表重新操作！";
                return false;
            }

            //查询部门指定岗位信息
            Model.Admin.MSPositionInfo positionInfo = DAL.Admin.MSPositionInfoDal.GetByDepartmentIdAndPositionType(departmentId, positionCode);
            if (positionInfo == null)
            {
                errorMsg = "数据错误，请联系管理员！";
                return false;
            }

            //查询该岗位有没有绑定用户
            List<Model.Admin.MSUserPositionRelation> relationList = DAL.Admin.MSUserPositionRelationDal.GetListByPositionID(positionInfo.PositionID);

            if (relationList != null && relationList.Count > 0)
            {
                Model.Admin.MSUserPositionRelation relationEntity = relationList[0];
                //更新用户ID
                DAL.Admin.MSUserPositionRelationDal.UpdateUserIDByRelationId(relationEntity.RelationID, directorId);
            }
            else
            {
                //未绑定插入
                Model.Admin.MSUserPositionRelation relationEntity = new MSUserPositionRelation()
                {
                    PositionID = positionInfo.PositionID,
                    UserID = directorId,
                    Deleted = 0,
                    CreateTime = DateTime.Now,
                    CreateUser = Model.Identity.LoginUserInfo.UserID
                };
                DAL.Admin.MSUserPositionRelationDal.Add(relationEntity);
            }

            return true;
        }
    }
}

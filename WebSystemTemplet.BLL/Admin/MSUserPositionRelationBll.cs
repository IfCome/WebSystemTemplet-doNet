using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSystemTemplet.BLL.Admin
{
    public static class MSUserPositionRelationBll
    {
        /// <summary>
        /// 添加用户岗位关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool AddUserPositionRelation(Model.Admin.MSUserPositionRelation entity)
        {
            if (entity == null)
            {
                return false;
            }
            return DAL.Admin.MSUserPositionRelationDal.Add(entity);
        }

        /// <summary>
        /// 获取指定部门，指定岗位的人员信息（返回第一条信息）
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="positionType">岗位类型</param>
        /// <returns></returns>
        public static Model.Admin.MSUserInfo GetUserByDepartmentIdAndPositionType(long departmentId, int positionType)
        {
            return DAL.Admin.MSUserPositionRelationDal.GetUserByDepartmentIdAndPositionType(departmentId, positionType);
        }
    }
}

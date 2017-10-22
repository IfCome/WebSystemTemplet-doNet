using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSystemTemplet.BLL.Admin
{
    public static class MSPositionInfoBll
    {
        public static long GetPositionIdByDepartmentIdAndPositionType(long departmentId, int positionType)
        {
            Model.Admin.MSPositionInfo positionInfo = DAL.Admin.MSPositionInfoDal.GetByDepartmentIdAndPositionType(departmentId, positionType);
            return positionInfo == null ? 0 : positionInfo.PositionID;
        }
    }
}

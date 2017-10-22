using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.DAL.Admin
{
    public static class MSPositionInfoDal
    {


        #region 增加



        #endregion

        #region 更新

        #endregion

        #region 查询

        /// <summary>
        /// 获取指定部门下的指定岗位ID
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="positionType">岗位类型</param>
        /// <returns></returns>
        public static Model.Admin.MSPositionInfo GetByDepartmentIdAndPositionType(long departmentId, int positionType)
        {
            var sql = @"
                        SELECT
                                [PositionID]
                                ,[DepartmentID]
                                ,[PositionType]
                                ,[PositionName]
                                ,[CreateTime]
                                ,[CreateUser]                      
                        FROM [MSPositionInfo] WITH (NOLOCK)
                        WHERE 
                            [DepartmentID] = @DepartmentID
                            AND [PositionType] = @PositionType
                            AND [Deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentID", Value = departmentId });
            parameters.Add(new SqlParameter() { ParameterName = "@PositionType", Value = positionType });

            var dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Model.Admin.MSPositionInfo()
                {
                    PositionID = Converter.TryToInt64(row["PositionID"], -1),
                    DepartmentID = Converter.TryToInt64(row["DepartmentID"], -1),
                    PositionType = Converter.TryToInt32(row["PositionType"], -1),
                    PositionName = Converter.TryToString(row["PositionName"], string.Empty),
                    CreateTime = Converter.TryToDateTime(row["CreateTime"], DateTime.MinValue),
                    CreateUser = Converter.TryToInt64(row["CreateUser"], -1)
                };
            }
            else
            {
                return null;
            }
        }

        #endregion
        
        #region 删除

        #endregion
        
    }
}

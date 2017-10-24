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

        /// <summary>
        /// 增加实体列表（非批量拷贝方式，适用于小数据量的写入）
        /// </summary>
        public static void Add(List<Model.Admin.MSPositionInfo> entities)
        {
            entities.ForEach(entity =>
            {
                Add(entity);
            });
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="databaseConnectionString">数据库链接字符串</param>
        /// <param name="entity">实体</param>
        public static bool Add(Model.Admin.MSPositionInfo entity)
        {
            var sql = @"
                        INSERT INTO [MSPositionInfo]
                               (
                                [DepartmentID]
                                ,[PositionType]
                                ,[PositionName]
                                ,[CreateTime]
                                ,[CreateUser]
                               )
                         VALUES
                               (
                                @DepartmentID
                                ,@PositionType
                                ,@PositionName
                                ,@CreateTime
                                ,@CreateUser
                               )
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentID", Value = entity.DepartmentID });
            parameters.Add(new SqlParameter() { ParameterName = "@PositionType", Value = entity.PositionType });
            parameters.Add(new SqlParameter() { ParameterName = "@PositionName", Value = entity.PositionName });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateTime", Value = entity.CreateTime });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateUser", Value = entity.CreateUser });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }



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

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.DAL.Admin
{
    public static class MSDepartmentInfoDal
    {
        #region 增加

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="entity">实体</param>
        public static bool Add(Model.Admin.MSDepartmentInfo entity)
        {
            var sql = @"
                        INSERT INTO [MSDepartmentInfo]
                               (
                                [DepartmentName]
                                ,[DepartmentLevel]
                                ,[ParentID]
                                ,[CreateTime]
                                ,[CreateUser]
                                ,[Deleted]

                               )
                         VALUES
                               (
                                @DepartmentName
                                ,@DepartmentLevel
                                ,@ParentID
                                ,@CreateTime
                                ,@CreateUser
                                ,@Deleted
                               )
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentName", Value = entity.DepartmentName });
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentLevel", Value = entity.DepartmentLevel });
            parameters.Add(new SqlParameter() { ParameterName = "@ParentID", Value = entity.ParentID });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateTime", Value = entity.CreateTime });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateUser", Value = entity.CreateUser });
            parameters.Add(new SqlParameter() { ParameterName = "@Deleted", Value = entity.Deleted });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }


        #endregion

        #region 更新

        /// <summary>
        /// 通过DepartmentID更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public static bool UpdateByDepartmentID(Model.Admin.MSDepartmentInfo entity)
        {
            var sql = @"
                        UPDATE [MSDepartmentInfo]
                        SET
                            [DepartmentName] = @DepartmentName
                            ,[DepartmentLevel] = @DepartmentLevel
                            ,[ParentID] = @ParentID
                            ,[UpdateTime] = @UpdateTime
                            ,[UpdateUser] = @UpdateUser
                        WHERE 
                            [DepartmentID] = @DepartmentID
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentID", Value = entity.DepartmentID });
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentName", Value = entity.DepartmentName });
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentLevel", Value = entity.DepartmentLevel });
            parameters.Add(new SqlParameter() { ParameterName = "@ParentID", Value = entity.ParentID });
            parameters.Add(new SqlParameter() { ParameterName = "@UpdateTime", Value = entity.UpdateTime });
            parameters.Add(new SqlParameter() { ParameterName = "@UpdateUser", Value = entity.UpdateUser });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }


        #endregion

        #region 查询

        /// <summary>
        /// 查询所有实体列表（不存在时，返回null）
        /// </summary>
        /// <param name="databaseConnectionString">数据库链接字符串</param>
        public static List<Model.Admin.MSDepartmentInfo> GetAllDepartmentNameAndId()
        {
            var sql = @"
                        SELECT 
                                [DepartmentID]
                                ,[DepartmentName]                   
                        FROM [MSDepartmentInfo] WITH (NOLOCK)
                        WHERE [Deleted] = 0
                    ";

            var dataTable = SqlHelper.ExecuteDataTable(sql);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.AsEnumerable().Select(row => new Model.Admin.MSDepartmentInfo()
                {
                    DepartmentID = Converter.TryToInt64(row["DepartmentID"], -1),
                    DepartmentName = Converter.TryToString(row["DepartmentName"], string.Empty)
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.Admin.MSDepartmentInfo> GetPageListByCondition(SqlParams baseParams, out int allCount)
        {
            var sqlBase = @"
                        WITH Virtual_T AS
                        (
	                        SELECT 
                                    [DepartmentID]
                                    ,[DepartmentName]
                                    ,[DepartmentLevel]
                                    ,[ParentID]
                                    ,[CreateTime]
                                    ,[CreateUser]
                                    ,[UpdateTime]
                                    ,[UpdateUser]
                                    ,[Deleted]
                                    ,ROW_NUMBER() OVER (ORDER BY DepartmentName) AS [RowNumber] 
	                        FROM [MSDepartmentInfo] WITH (NOLOCK)
                            WHERE [Deleted] = 0
                            {0}
                        )
                    ";

            //条件查询部分
            var sqlWhere = "";
            var parameters = new List<SqlParameter>();
            if (baseParams.hasParam("DepartmentLevel"))
            {
                sqlWhere += " AND DepartmentLevel = @DepartmentLevel ";
                parameters.Add(new SqlParameter() { ParameterName = "@DepartmentLevel", Value = baseParams.getParam("DepartmentLevel") });
            }
            if (baseParams.hasParam("keyWords"))
            {
                sqlWhere += " AND DepartmentName LIKE @DepartmentName  ";
                parameters.Add(new SqlParameter() { ParameterName = "@DepartmentName", Value = "%" + baseParams.getParam("keyWords") + "%" });
            }
            parameters.Add(new SqlParameter() { ParameterName = "@PageSize", Value = baseParams.PageSize });
            parameters.Add(new SqlParameter() { ParameterName = "@PageIndex", Value = baseParams.PageIndex });

            sqlBase = string.Format(sqlBase, sqlWhere);

            //记录总数计算
            var countParameters = new List<SqlParameter>();
            parameters.ForEach(h => countParameters.Add(new SqlParameter() { ParameterName = h.ParameterName, Value = h.Value }));
            var sqlCount = sqlBase + " SELECT COUNT(*) CNT FROM Virtual_T ";
            allCount = Converter.TryToInt32(SqlHelper.ExecuteScalar(sqlCount, countParameters.ToArray()));

            if (allCount == 0)
            {
                return null;
            }
            var sqlPage = sqlBase + " SELECT * FROM Virtual_T WHERE @PageSize * (@PageIndex - 1) < RowNumber AND RowNumber <= @PageSize * @PageIndex ";
            var dataTable = SqlHelper.ExecuteDataTable(sqlPage, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.AsEnumerable().Select(row => new Model.Admin.MSDepartmentInfo()
                {
                    DepartmentID = Converter.TryToInt64(row["DepartmentID"], -1),
                    DepartmentName = Converter.TryToString(row["DepartmentName"], string.Empty),
                    DepartmentLevel = Converter.TryToInt32(row["DepartmentLevel"], -1),
                    ParentID = Converter.TryToInt64(row["ParentID"], -1),
                    CreateTime = Converter.TryToDateTime(row["CreateTime"], DateTime.MinValue),
                    CreateUser = Converter.TryToInt64(row["CreateUser"], -1),
                    UpdateTime = Converter.TryToDateTime(row["UpdateTime"], DateTime.MinValue),
                    UpdateUser = Converter.TryToInt64(row["UpdateUser"], -1),
                    Deleted = Converter.TryToByte(row["Deleted"], 0),
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定部门名称的数量
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns></returns>
        public static int GetCountByDepartmentNameAndId(string departmentName, long departmentId)
        {
            var sql = @"
                        SELECT COUNT(*) FROM [MSDepartmentInfo] WITH (NOLOCK)
                        WHERE 
                            [DepartmentName] = @DepartmentName
                            AND [deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentName", Value = departmentName });

            if (departmentId != -1)
            {
                sql += " AND [DepartmentId] <> @DepartmentId ";
                parameters.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = departmentId });
            }

            var count = SqlHelper.ExecuteScalar(sql, parameters.ToArray());

            return Converter.TryToInt32(count);
        }

        #endregion

        #region 聚合

        #endregion

        #region 删除


        #endregion

    }
}
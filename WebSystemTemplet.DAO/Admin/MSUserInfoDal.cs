using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.DAL.Admin
{
    /// <summary>
    /// MSUserInfo
    /// </summary>
    public class MSUserInfo
    {
        #region 增加

        #endregion

        #region 更新

        #endregion

        #region 查询
        /// <summary>
        /// 通过RealName查询实体列表（不存在时，返回null）
        /// </summary>
        /// <param name="databaseConnectionString">数据库链接字符串</param>
        /// <param name="wherePart">条件部分</param>
        public static List<Model.Admin.MSUserInfo> GetPageListByCondition(SqlParams baseParams, out int allCount)
        {
            var sqlBase = @"
                        WITH t
                             AS (SELECT mupr.userid,
                                        mpi.positionname,
                                        mpi.PositionType
                                 FROM   msuserpositionrelation AS mupr
                                        LEFT JOIN mspositioninfo AS mpi
                                               ON mpi.positionid = mupr.positionid AND mpi.Deleted = 0
                                 WHERE mupr.Deleted = 0
                                ),
                             tup
                             AS (SELECT UserID,
                                        Stuff((SELECT '，' + positionname
                                               FROM   t a
                                               WHERE  b.userid = a.userid
                                               FOR xml path('')), 1, 1, '') PositionName
                                 FROM   t b
                                 GROUP  BY userid),
                             Virtual_T
                             AS (SELECT mui.[UserID],
                                        mui.[UserName],
                                        mui.[RealName],
                                        mui.[Password],
                                        mui.[RoleID],
                                        mui.[SchoolID],
                                        mui.[MajorID],
                                        mui.[ClassID],
                                        mui.[Telephone],
                                        mui.[IconUrl],
                                        mui.[QQ],
                                        mui.[Email],
                                        mui.[Remark],
                                        mui.[LastLoginTime],
                                        mui.[CreateTime],
                                        mui.[CreateUser],
                                        mui.[UpdateTime],
                                        mui.[UpdateUser],
                                        mui.[Deleted],
                                        tup.PositionName,
                                        Row_number() OVER (ORDER BY [RealName]) AS [RowNumber]
                                 FROM   msuserinfo AS mui
                                        LEFT JOIN tup
                                               ON tup.userid = mui.userid
                                 WHERE mui.Deleted=0
                                        {0}
                            )
                    ";

            //条件查询部分
            var sqlWhere = "";
            var parameters = new List<SqlParameter>();
            if (baseParams.hasParam("PositionType"))
            {
                sqlWhere += " AND mui.userid IN (SELECT t.userid FROM t WHERE t.PositionType = @PositionType) ";
                parameters.Add(new SqlParameter() { ParameterName = "@PositionType", Value = baseParams.getParam("PositionType") });
            }
            if (baseParams.hasParam("roleId"))
            {
                sqlWhere += " AND mui.roleId = @roleId ";
                parameters.Add(new SqlParameter() { ParameterName = "@roleId", Value = baseParams.getParam("roleId") });
            }
            if (baseParams.hasParam("majorId"))
            {
                sqlWhere += " AND mui.majorId = @majorId ";
                parameters.Add(new SqlParameter() { ParameterName = "@majorId", Value = baseParams.getParam("majorId") });
            }
            if (baseParams.hasParam("keyWords"))
            {
                sqlWhere += " AND mui.realName LIKE @realName  ";
                parameters.Add(new SqlParameter() { ParameterName = "@realName", Value = "%" + baseParams.getParam("keyWords") + "%" });
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
                return dataTable.AsEnumerable().Select(row => new Model.Admin.MSUserInfo()
                {
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    UserName = Converter.TryToString(row["UserName"], string.Empty),
                    RealName = Converter.TryToString(row["RealName"], string.Empty),
                    Password = Converter.TryToString(row["Password"], string.Empty),
                    RoleID = Converter.TryToInt64(row["RoleID"], -1),
                    SchoolID = Converter.TryToInt64(row["SchoolID"], -1),
                    MajorID = Converter.TryToInt64(row["MajorID"], -1),
                    ClassID = Converter.TryToInt64(row["ClassID"], -1),
                    Telephone = Converter.TryToString(row["Telephone"], string.Empty),
                    IconUrl = Converter.TryToString(row["IconUrl"], string.Empty),
                    QQ = Converter.TryToString(row["QQ"], string.Empty),
                    Email = Converter.TryToString(row["Email"], string.Empty),
                    Remark = Converter.TryToString(row["Remark"], string.Empty),
                    LastLoginTime = Converter.TryToDateTime(row["LastLoginTime"], Convert.ToDateTime("1900-01-01")),
                    CreateTime = Converter.TryToDateTime(row["CreateTime"], Convert.ToDateTime("1900-01-01")),
                    CreateUser = Converter.TryToInt64(row["CreateUser"], -1),
                    UpdateTime = Converter.TryToDateTime(row["UpdateTime"], Convert.ToDateTime("1900-01-01")),
                    UpdateUser = Converter.TryToInt64(row["UpdateUser"], -1),
                    Deleted = Converter.TryToByte(row["Deleted"], 0),
                    PositionName = Converter.TryToString(row["PositionName"], string.Empty),

                }).ToList();
            }
            else
            {
                return null;
            }

        }

        #endregion

        #region 聚合

        #endregion

        #region 删除

        #endregion

        #region 判断

        #endregion

        #region 其他

        #endregion
    }
}




using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.DAL.Admin
{
    public class MSSystemOperateLogDal
    {
        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="entity">实体</param>
        public static bool Add(Model.Admin.MSSystemOperateLog entity)
        {
            var sql = @"
                        INSERT INTO [MSSystemOperateLog]
                               (
                                [LogID]
                                ,[UserID]
                                ,[LogType]
                                ,[LogTile]
                                ,[LogMsg]
                                ,[LogDetail]
                                ,[OperateTime]
                                ,[IpAddress]
                               )
                         VALUES
                               (
                                @LogID
                                ,@UserID
                                ,@LogType
                                ,@LogTile
                                ,@LogMsg
                                ,@LogDetail
                                ,@OperateTime
                                ,@IpAddress
                               )
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@LogID", Value = entity.LogID });
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = entity.UserID });
            parameters.Add(new SqlParameter() { ParameterName = "@LogType", Value = entity.LogType });
            parameters.Add(new SqlParameter() { ParameterName = "@LogTile", Value = entity.LogTile });
            parameters.Add(new SqlParameter() { ParameterName = "@LogMsg", Value = entity.LogMsg });
            parameters.Add(new SqlParameter() { ParameterName = "@LogDetail", Value = entity.LogDetail });
            parameters.Add(new SqlParameter() { ParameterName = "@OperateTime", Value = DateTime.Now });
            parameters.Add(new SqlParameter() { ParameterName = "@IpAddress", Value = entity.IpAddress });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }


        /// <summary>
        /// 通过UserID查询实体列表（不存在时，返回null）
        /// </summary>
        public static List<Model.Admin.MSSystemOperateLog> GetTop10ListByUserID(long userId)
        {
            var sql = @"
                    SELECT TOP 10
	                    [LogID],
	                    [UserID],
	                    [LogType],
	                    [LogTile],
	                    [LogMsg],
	                    [LogDetail],
	                    [OperateTime],
	                    [IpAddress]
                    FROM MSSystemOperateLog WITH (NOLOCK)
                    WHERE [UserID] = @UserID
	                       AND LogType <> 501 --排除其它类型的操作
                    ORDER BY [OperateTime] DESC
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = userId });

            var dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.AsEnumerable().Select(row => new Model.Admin.MSSystemOperateLog()
                {
                    LogID = Converter.TryToString(row["LogID"], string.Empty),
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    LogType = Converter.TryToInt32(row["LogType"], -1),
                    LogTile = Converter.TryToString(row["LogTile"], string.Empty),
                    LogMsg = Converter.TryToString(row["LogMsg"], string.Empty),
                    LogDetail = Converter.TryToString(row["LogDetail"], string.Empty),
                    OperateTime = Converter.TryToDateTime(row["OperateTime"], Convert.ToDateTime("1900-01-01")),
                    IpAddress = Converter.TryToString(row["IpAddress"], string.Empty)
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取最近10条log
        /// </summary>
        public static List<Model.Admin.MSSystemOperateLog> GetTop10List()
        {
            var sql = @"
                        SELECT TOP 10
	                        mlg.[LogID],
	                        mlg.[UserID],
	                        mlg.[LogType],
	                        mlg.[LogTile],
	                        mlg.[LogMsg],
	                        mlg.[LogDetail],
	                        mlg.[OperateTime],
	                        mlg.[IpAddress],
	                        mi.UserName,
	                        mi.RealName
                        FROM MSSystemOperateLog AS mlg WITH (NOLOCK)
                        INNER JOIN MSUserInfo mi WITH (NOLOCK)
	                        ON mlg.UserID = mi.UserID
	                        AND mi.deleted = 0
	                        AND mi.LogType <> 501 --排除其它类型的操作
                        ORDER BY mlg.[OperateTime] DESC
                    ";
            var dataTable = SqlHelper.ExecuteDataTable(sql, null);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.AsEnumerable().Select(row => new Model.Admin.MSSystemOperateLog()
                {
                    LogID = Converter.TryToString(row["LogID"], string.Empty),
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    LogType = Converter.TryToInt32(row["LogType"], -1),
                    LogTile = Converter.TryToString(row["LogTile"], string.Empty),
                    LogMsg = Converter.TryToString(row["LogMsg"], string.Empty),
                    LogDetail = Converter.TryToString(row["LogDetail"], string.Empty),
                    OperateTime = Converter.TryToDateTime(row["OperateTime"], Convert.ToDateTime("1900-01-01")),
                    IpAddress = Converter.TryToString(row["IpAddress"], string.Empty),
                    RealName = Converter.TryToString(row["RealName"], string.Empty),
                    UserName = Converter.TryToString(row["UserName"], string.Empty),

                }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}

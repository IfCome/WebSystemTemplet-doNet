using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.DAL.Admin
{
    public class MSUserPositionRelationDal
    {

        #region 增加

        /// <summary>
        /// 增加实体
        /// </summary>
        public static bool Add(Model.Admin.MSUserPositionRelation entity)
        {
            var sql = @"
                        INSERT INTO [MSUserPositionRelation]
                               (
                                [UserID]
                                ,[PositionID]
                                ,[CreateTime]
                                ,[CreateUser]
                                ,[Deleted]

                               )
                         VALUES
                               (
                                @UserID
                                ,@PositionID
                                ,@CreateTime
                                ,@CreateUser
                                ,@Deleted
                               )
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = entity.UserID });
            parameters.Add(new SqlParameter() { ParameterName = "@PositionID", Value = entity.PositionID });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateTime", Value = entity.CreateTime });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateUser", Value = entity.CreateUser });
            parameters.Add(new SqlParameter() { ParameterName = "@Deleted", Value = entity.Deleted });


            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }

        #endregion

        #region 更新

        #endregion

        #region 查询

        /// <summary>
        /// 获取指定部门，指定岗位的人员信息（返回第一条信息）
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="positionType">岗位类型</param>
        /// <returns></returns>
        public static Model.Admin.MSUserInfo GetUserByDepartmentIdAndPositionType(long departmentId, int positionType)
        {
            var sql = @"
                        SELECT
	                        mui.[UserId],
	                        mui.[UserName],
	                        mui.[RealName]
                        FROM [MSUserInfo] AS mui
                        INNER JOIN MSUserPositionRelation AS mupr
	                        ON mupr.UserId = mui.UserId
	                        AND mupr.deleted = 0
                        INNER JOIN MSPositionInfo AS mpi
	                        ON mpi.PositionId = mupr.PositionId
	                        AND mpi.deleted = 0
                        WHERE mui.deleted = 0
                            AND mpi.PositionType = @PositionType
                            AND mpi.DepartmentId = @DepartmentId
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@DepartmentId", Value = departmentId });
            parameters.Add(new SqlParameter() { ParameterName = "@PositionType", Value = positionType });

            var dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Model.Admin.MSUserInfo()
                {
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    UserName = Converter.TryToString(row["UserName"], string.Empty),
                    RealName = Converter.TryToString(row["RealName"], string.Empty),
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过PositionID查询实体列表（不存在时，返回null）
        /// </summary>
        public static List<Model.Admin.MSUserPositionRelation> GetListByPositionID(long positionId)
        {
            var sql = @"
                        SELECT 
                                [RelationID]
                                ,[UserID]
                                ,[PositionID]
                                ,[CreateTime]
                                ,[CreateUser]                       
                        FROM [MSUserPositionRelation] WITH (NOLOCK)
                        WHERE 
                            [PositionID] = @PositionID
                            AND [Deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@PositionID", Value = positionId });

            var dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.AsEnumerable().Select(row => new Model.Admin.MSUserPositionRelation()
                {
                    RelationID = Converter.TryToInt64(row["RelationID"], -1),
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    PositionID = Converter.TryToInt64(row["PositionID"], -1),
                    CreateTime = Converter.TryToDateTime(row["CreateTime"], DateTime.MinValue),
                    CreateUser = Converter.TryToInt64(row["CreateUser"], -1)
                }).ToList();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 通过PositionID更新UserID
        /// </summary>
        public static bool UpdateUserIDByRelationId(long relationId, long userId)
        {
            var sql = @"
                        UPDATE [MSUserPositionRelation]
                            SET
                                [UserID] = @UserID
                            WHERE [RelationID] = @RelationID AND [Deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@RelationID", Value = relationId });
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = userId });

            int i = SqlHelper.ExecuteNonQuery( sql, parameters.ToArray());
            return i > 0 ? true : false;
        }


        #endregion

        #region 删除

        #endregion
    }
}

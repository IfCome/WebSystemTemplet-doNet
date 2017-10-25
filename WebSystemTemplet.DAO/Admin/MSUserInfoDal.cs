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
    public class MSUserInfoDal
    {
        #region 增加
        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="databaseConnectionString">数据库链接字符串</param>
        /// <param name="entity">实体</param>
        public static long Add(Model.Admin.MSUserInfo entity)
        {
            var sql = @"
                        INSERT INTO [MSUserInfo]
                               (
                                [UserName]
                                ,[RealName]
                                ,[Password]
                                ,[RoleID]
                                ,[SchoolID]
                                ,[MajorID]
                                ,[ClassID]
                                ,[Gender]
                                ,[Telephone]
                                ,[QQ]
                                ,[Email]
                                ,[Remark]
                                ,[CreateTime]
                                ,[CreateUser]
                                ,[Deleted]

                               )
                         VALUES
                               (
                                @UserName
                                ,@RealName
                                ,@Password
                                ,@RoleID
                                ,@SchoolID
                                ,@MajorID
                                ,@ClassID
                                ,@Gender
                                ,@Telephone
                                ,@QQ
                                ,@Email
                                ,@Remark
                                ,@CreateTime
                                ,@CreateUser
                                ,@Deleted
                               );SELECT SCOPE_IDENTITY();
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = entity.UserID });
            parameters.Add(new SqlParameter() { ParameterName = "@UserName", Value = entity.UserName });
            parameters.Add(new SqlParameter() { ParameterName = "@RealName", Value = entity.RealName });
            parameters.Add(new SqlParameter() { ParameterName = "@Password", Value = entity.Password });
            parameters.Add(new SqlParameter() { ParameterName = "@RoleID", Value = entity.RoleID });
            parameters.Add(new SqlParameter() { ParameterName = "@SchoolID", Value = entity.SchoolID });
            parameters.Add(new SqlParameter() { ParameterName = "@MajorID", Value = entity.MajorID });
            parameters.Add(new SqlParameter() { ParameterName = "@ClassID", Value = entity.ClassID });
            parameters.Add(new SqlParameter() { ParameterName = "@Gender", Value = entity.Gender });
            parameters.Add(new SqlParameter() { ParameterName = "@Telephone", Value = entity.Telephone });
            parameters.Add(new SqlParameter() { ParameterName = "@QQ", Value = entity.QQ });
            parameters.Add(new SqlParameter() { ParameterName = "@Email", Value = entity.Email });
            parameters.Add(new SqlParameter() { ParameterName = "@Remark", Value = entity.Remark });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateTime", Value = entity.CreateTime });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateUser", Value = entity.CreateUser });
            parameters.Add(new SqlParameter() { ParameterName = "@Deleted", Value = entity.Deleted });


            return Converter.TryToInt64(SqlHelper.ExecuteScalar(sql, parameters.ToArray()));
        }

        #endregion

        #region 更新

        /// <summary>
        /// 通过ID更新LastLoginTime
        /// </summary>
        public static bool UpdateLastLoginTimeByID(DateTime lastLoginTime, long userId)
        {
            var sql = @"
                        UPDATE [MSUserInfo]
                        SET [LastLoginTime] = @LastLoginTime
                        WHERE [UserId] = @UserId
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@LastLoginTime", Value = lastLoginTime });
            parameters.Add(new SqlParameter() { ParameterName = "@UserId", Value = userId });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }

        /// <summary>
        /// 通过ID更新密码
        /// </summary>
        public static bool UpdataPasswordByID(long userId, string password)
        {
            var sql = @"
                        UPDATE [MSUserInfo] SET [PassWord] = @PassWord
                        WHERE 
                            [UserID] = @UserID
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@PassWord", Value = password });
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = userId });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }

        /// <summary>
        /// 通过UserID更新实体
        /// </summary>
        public static bool UpdataUserInfoByID(Model.Admin.MSUserInfo entity)
        {
            var sql = @"
                        UPDATE [MSUserInfo]
                            SET
                                [RealName] = @RealName
                                ,[SchoolID] = @SchoolID
                                ,[MajorID] = @MajorID
                                ,[ClassID] = @ClassID
                                ,[Gender] = @Gender
                                ,[Telephone] = @Telephone
                                ,[IconUrl] = @IconUrl
                                ,[QQ] = @QQ
                                ,[Email] = @Email
                                ,[Remark] = @Remark
                                ,[UpdateTime] = @UpdateTime
                                ,[UpdateUser] = @UpdateUser
                            WHERE 
                                [UserID] = @UserID
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = entity.UserID });
            parameters.Add(new SqlParameter() { ParameterName = "@RealName", Value = entity.RealName });
            parameters.Add(new SqlParameter() { ParameterName = "@SchoolID", Value = entity.SchoolID });
            parameters.Add(new SqlParameter() { ParameterName = "@MajorID", Value = entity.MajorID });
            parameters.Add(new SqlParameter() { ParameterName = "@ClassID", Value = entity.ClassID });
            parameters.Add(new SqlParameter() { ParameterName = "@Gender", Value = entity.Gender });
            parameters.Add(new SqlParameter() { ParameterName = "@Telephone", Value = entity.Telephone });
            parameters.Add(new SqlParameter() { ParameterName = "@IconUrl", Value = entity.IconUrl });
            parameters.Add(new SqlParameter() { ParameterName = "@QQ", Value = entity.QQ });
            parameters.Add(new SqlParameter() { ParameterName = "@Email", Value = entity.Email });
            parameters.Add(new SqlParameter() { ParameterName = "@Remark", Value = entity.Remark });
            parameters.Add(new SqlParameter() { ParameterName = "@UpdateTime", Value = entity.UpdateTime });
            parameters.Add(new SqlParameter() { ParameterName = "@UpdateUser", Value = entity.UpdateUser });

            int i = SqlHelper.ExecuteNonQuery( sql, parameters.ToArray());
            return i > 0 ? true : false;
        }




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
                                        mui.[Gender],
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
                    Gender = Converter.TryToByte(row["Gender"], 1),
                    Telephone = Converter.TryToString(row["Telephone"], string.Empty),
                    IconUrl = Converter.TryToString(row["IconUrl"], string.Empty),
                    QQ = Converter.TryToString(row["QQ"], string.Empty),
                    Email = Converter.TryToString(row["Email"], string.Empty),
                    Remark = Converter.TryToString(row["Remark"], string.Empty),
                    LastLoginTime = Converter.TryToDateTime(row["LastLoginTime"], DateTime.MinValue),
                    CreateTime = Converter.TryToDateTime(row["CreateTime"], DateTime.MinValue),
                    CreateUser = Converter.TryToInt64(row["CreateUser"], -1),
                    UpdateTime = Converter.TryToDateTime(row["UpdateTime"], DateTime.MinValue),
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

        /// <summary>
        /// 通过UserName和password查询实体（不存在时，返回null）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码（MD5加密后）</param>
        /// <returns></returns>
        public static Model.Admin.MSUserInfo GetInfoByUserNameAndPwd(string userName, string password)
        {
            var sql = @"
                        SELECT
	                        [UserId],
	                        [UserName],
	                        [RealName],
	                        [Password],
	                        [RoleId],
	                        [SchoolId],
	                        [MajorId],
	                        [ClassId],
                            [Gender],
	                        [Telephone],
	                        [IconUrl],
	                        [QQ],
	                        [Email],
	                        [Remark],
	                        [LastLoginTime]
                        FROM [MSUserInfo]
                        WHERE [UserName] = @UserName
                            AND [PassWord] = @PassWord
                            AND [deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserName", Value = userName });
            parameters.Add(new SqlParameter() { ParameterName = "@PassWord", Value = password });

            var dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Model.Admin.MSUserInfo()
                {
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    UserName = Converter.TryToString(row["UserName"], string.Empty),
                    RealName = Converter.TryToString(row["RealName"], string.Empty),
                    Password = Converter.TryToString(row["Password"], string.Empty),
                    RoleID = Converter.TryToInt64(row["RoleID"], -1),
                    SchoolID = Converter.TryToInt64(row["SchoolID"], -1),
                    MajorID = Converter.TryToInt64(row["MajorID"], -1),
                    ClassID = Converter.TryToInt64(row["ClassID"], -1),
                    Gender = Converter.TryToByte(row["Gender"], 1),
                    Telephone = Converter.TryToString(row["Telephone"], string.Empty),
                    IconUrl = Converter.TryToString(row["IconUrl"], string.Empty),
                    QQ = Converter.TryToString(row["QQ"], string.Empty),
                    Email = Converter.TryToString(row["Email"], string.Empty),
                    Remark = Converter.TryToString(row["Remark"], string.Empty),
                    LastLoginTime = Converter.TryToDateTime(row["LastLoginTime"], DateTime.MinValue),
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过UserID查询实体（不存在时，返回null）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static Model.Admin.MSUserInfo GetUserInfoByID(long userId)
        {
            var sql = @"
                        SELECT
                                [UserID]
                                ,[UserName]
                                ,[RealName]
                                ,[Password]
                                ,[RoleID]
                                ,[SchoolID]
                                ,[MajorID]
                                ,[ClassID]
                                ,[Gender]
                                ,[Telephone]
                                ,[IconUrl]
                                ,[QQ]
                                ,[Email]
                                ,[Remark]
                                ,[LastLoginTime]                         
                        FROM [MSUserInfo] WITH (NOLOCK)
                        WHERE 
                            [UserID] = @UserID
                            AND [deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserID", Value = userId });

            var dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Model.Admin.MSUserInfo()
                {
                    UserID = Converter.TryToInt64(row["UserID"], -1),
                    UserName = Converter.TryToString(row["UserName"], string.Empty),
                    RealName = Converter.TryToString(row["RealName"], string.Empty),
                    Password = Converter.TryToString(row["Password"], string.Empty),
                    RoleID = Converter.TryToInt64(row["RoleID"], -1),
                    SchoolID = Converter.TryToInt64(row["SchoolID"], -1),
                    MajorID = Converter.TryToInt64(row["MajorID"], -1),
                    ClassID = Converter.TryToInt64(row["ClassID"], -1),
                    Gender = Converter.TryToByte(row["Gender"], 1),
                    Telephone = Converter.TryToString(row["Telephone"], string.Empty),
                    IconUrl = Converter.TryToString(row["IconUrl"], string.Empty),
                    QQ = Converter.TryToString(row["QQ"], string.Empty),
                    Email = Converter.TryToString(row["Email"], string.Empty),
                    Remark = Converter.TryToString(row["Remark"], string.Empty),
                    LastLoginTime = Converter.TryToDateTime(row["LastLoginTime"], DateTime.MinValue),
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定用户名的用户量
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static int GetCountByUserName(string userName)
        {
            var sql = @"
                        SELECT COUNT(*) FROM [MSUserInfo] WITH (NOLOCK)
                        WHERE 
                            [UserName] = @UserName
                            AND [deleted] = 0
                    ";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserName", Value = userName });

            var count = SqlHelper.ExecuteScalar(sql, parameters.ToArray());

            return Converter.TryToInt32(count);
        }

        /// <summary>
        /// 根据条件查询所有用户基本信息
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public static List<Model.Admin.MSUserInfo> GetBaseInfoListByCondition(SqlParams baseParams)
        {
            var sqlBase = @"
                            SELECT [UserID],
                                [UserName],
                                [RealName],
                                [Password],
                                [RoleID],
                                [SchoolID],
                                [MajorID],
                                [ClassID],
                                [Gender],
                                [Telephone],
                                [IconUrl],
                                [QQ],
                                [Email],
                                [Remark],
                                [LastLoginTime],
                                [CreateTime],
                                [CreateUser]
                            FROM   msuserinfo 
                            WHERE  [Deleted] = 0
                                {0}
                            ORDER BY [RealName]
                    ";

            //条件查询部分
            var sqlWhere = "";
            var parameters = new List<SqlParameter>();
            if (baseParams.hasParam("roleId"))
            {
                sqlWhere += " AND roleId = @roleId ";
                parameters.Add(new SqlParameter() { ParameterName = "@roleId", Value = baseParams.getParam("roleId") });
            }
            if (baseParams.hasParam("majorId"))
            {
                sqlWhere += " AND majorId = @majorId ";
                parameters.Add(new SqlParameter() { ParameterName = "@majorId", Value = baseParams.getParam("majorId") });
            }
            if (baseParams.hasParam("classId"))
            {
                sqlWhere += " AND classId = @classId ";
                parameters.Add(new SqlParameter() { ParameterName = "@classId", Value = baseParams.getParam("classId") });
            }
            if (baseParams.hasParam("keyWords"))
            {
                sqlWhere += " AND realName LIKE @realName  ";
                parameters.Add(new SqlParameter() { ParameterName = "@realName", Value = "%" + baseParams.getParam("keyWords") + "%" });
            }

            sqlBase = string.Format(sqlBase, sqlWhere);

            var dataTable = SqlHelper.ExecuteDataTable(sqlBase, parameters.ToArray());

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
                    Gender = Converter.TryToByte(row["Gender"], 1),
                    Telephone = Converter.TryToString(row["Telephone"], string.Empty),
                    IconUrl = Converter.TryToString(row["IconUrl"], string.Empty),
                    QQ = Converter.TryToString(row["QQ"], string.Empty),
                    Email = Converter.TryToString(row["Email"], string.Empty),
                    Remark = Converter.TryToString(row["Remark"], string.Empty),
                    LastLoginTime = Converter.TryToDateTime(row["LastLoginTime"], DateTime.MinValue),
                    CreateTime = Converter.TryToDateTime(row["CreateTime"], DateTime.MinValue),
                    CreateUser = Converter.TryToInt64(row["CreateUser"], -1),
                }).ToList();
            }
            else
            {
                return null;
            }

        }        

        #endregion

        #region 删除

        /// <summary>
        /// 通过ID删除
        /// </summary>
        public static bool DeleteByID(long userId)
        {
            var sql = @"
                        UPDATE [MSUserInfo] SET [deleted] = 1 WHERE [UserId] = @UserId
                        UPDATE [MSUserPositionRelation] SET [deleted] = 1 WHERE [UserId] = @UserId
                    ";
            sql = string.Format(SqlHelper.tranSqlFormat, sql);
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@UserId", Value = userId });

            int i = SqlHelper.ExecuteNonQuery(sql, parameters.ToArray());
            return i > 0 ? true : false;
        }


        #endregion

    }
}




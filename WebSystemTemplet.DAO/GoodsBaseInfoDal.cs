﻿using WebSystemTemplet.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WebSystemTemplet.DAL
{
    public class GoodsBaseInfoDal
    {
        #region 增加
        /// <summary>
        /// 增加后返回ID提供给下一个页面进行更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string AddGoodsInfo(Model.GoodsBaseInfo entity)
        {
            var sql = @"
                        INSERT INTO [GoodsBaseInfo]
                               (
                                GoodsName 
                                ,Price
                                ,Describe
                                ,Category
                                ,CreateUserID
                                ,CreateTime               
                                ,IsDelete
                                ,IsHot
                               )
                         VALUES
                               (
                                @GoodsName 
                                ,@Price
                                ,@Describe
                                ,@Category
                                ,@CreateUserID
                                ,@CreateTime
                                ,0
                                ,0
                               );SELECT SCOPE_IDENTITY()";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@GoodsName", Value = entity.GoodsName });
            parameters.Add(new SqlParameter() { ParameterName = "@Price", Value = entity.Price });
            parameters.Add(new SqlParameter() { ParameterName = "@Describe", Value = entity.Describe });
            parameters.Add(new SqlParameter() { ParameterName = "@Category", Value = entity.Category });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateUserID", Value = entity.CreateUserID });
            parameters.Add(new SqlParameter() { ParameterName = "@CreateTime", Value = entity.CreateTime });
            try
            {
                return SqlHelper.ExecuteScalar(sql, parameters.ToArray()).ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 删除
        public static bool DeleteGoodsInfo(string id)
        {
            var sql = @"UPDATE dbo.GoodsBaseInfo SET IsDelete=1 WHERE ID=@ID";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@ID", Value = id });
            try
            {
                return SqlHelper.ExecuteNonQuery(sql, parameters.ToArray()) > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 修改
        public static bool UpDateGoodsInfo(Model.GoodsBaseInfo entity)
        {
            var sql = @"UPDATE [GoodsBaseInfo] SET {0} WHERE ID=@ID";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { ParameterName = "@ID", Value = entity.ID });

            string sqlPart = string.Empty;
            if (!string.IsNullOrEmpty(entity.GoodsName))
            {
                parameters.Add(new SqlParameter() { ParameterName = "@GoodsName", Value = entity.GoodsName });
                sqlPart += "GoodsName=@GoodsName,";
            }
            if (!string.IsNullOrEmpty(entity.Price))
            {
                parameters.Add(new SqlParameter() { ParameterName = "@Price", Value = entity.Price });
                sqlPart += "Price=@Price,";
            }
            if (!string.IsNullOrEmpty(entity.Describe))
            {
                parameters.Add(new SqlParameter() { ParameterName = "@Describe", Value = entity.Describe });
                sqlPart += "Describe=@Describe,";
            }
            if (!string.IsNullOrEmpty(entity.Category))
            {
                parameters.Add(new SqlParameter() { ParameterName = "@Category", Value = entity.Category });
                sqlPart += "Category=@Category,";
            }
            if (!string.IsNullOrEmpty(entity.ShowIcons))
            {
                parameters.Add(new SqlParameter() { ParameterName = "@ShowIcons", Value = entity.ShowIcons });
                sqlPart += "ShowIcons=@ShowIcons,";
            }
            if (!string.IsNullOrEmpty(entity.DetailIcons))
            {
                parameters.Add(new SqlParameter() { ParameterName = "@DetailIcons", Value = entity.DetailIcons });
                sqlPart += "DetailIcons=@DetailIcons,";
            }
            if (string.IsNullOrEmpty(sqlPart))
            {
                //没变化默认为更新成功
                return true;
            }
            else
            {
                sqlPart = sqlPart.Remove(sqlPart.Length - 1);
            }
            sql = string.Format(sql, sqlPart);

            try
            {
                return SqlHelper.ExecuteNonQuery(sql, parameters.ToArray()) > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 查询
        public static List<Model.GoodsBaseInfo> GetList(int pageSize, int currentPage, string keyWords, int category, string huodongstate, int ishot, int jiexiaotype, out int allCount)
        {
            var sql = @"
                        WITH Virtual_T AS
                        (
	                           SELECT 
                                   G.ID
                                   ,G.GoodsName
                                   ,G.Describe
                                   ,G.Price
                                   ,G.Category
                                   ,C.CategoryName
                                   ,G.ShowIcons
                                   ,G.DetailIcons
                                   ,G.CreateTime
                                   ,G.CreateUserID
                                   ,State=CASE WHEN State IS NULL THEN '0'
                                    ELSE State END
                                   ,ROW_NUMBER() OVER (ORDER BY G.CreateTime DESC) AS [RowNumber] 
                                   ,ZhongChouCount=(SELECT COUNT(1) FROM OrderInfo WHERE HuoDongID=H.ID)
                                   ,FinishedTime
                                   ,H.ID AS HuoDongID
                             FROM GoodsBaseInfo G WITH (NOLOCK) JOIN CategoryInfo C WITH (NOLOCK)
                             ON G.Category=C.ID LEFT JOIN dbo.HuodongInfo H
                             ON G.ID=H.GoodsID
                            WHERE G.IsDelete=0 AND C.IsDelete=0 AND  (State !=40 OR State IS NULL)  {0}
                        )
                        SELECT * FROM Virtual_T 
                        WHERE @PageSize * (@CurrentPage - 1) < RowNumber AND RowNumber <= @PageSize * @CurrentPage {1}
                    ";

            //条件查询部分
            string sqlWhere1 = "";
            string sqlWhere2 = "";
            var parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlWhere1 += "AND GoodsName LIKE @GoodsName";
                parameters.Add(new SqlParameter() { ParameterName = "@GoodsName", Value = "%" + keyWords + "%" });
            }
            if (category != 0)
            {
                sqlWhere1 += "AND Category =@Category";
                parameters.Add(new SqlParameter() { ParameterName = "@Category", Value = category });
            }
            if (!string.IsNullOrEmpty(huodongstate))
            {
                if (huodongstate == "0")
                {
                    sqlWhere1 += " AND State IS NULL";
                }
                else
                {
                    sqlWhere1 += " AND State=@State";
                }
                parameters.Add(new SqlParameter() { ParameterName = "@State", Value = huodongstate });
            }
            else
            {
                sqlWhere1 += " AND (State!=40 OR State IS NULL)";
            }
            if (ishot == 1)
            {
                sqlWhere1 += " AND IsHot=1";
            }
            //更改逻辑  所有正在众筹的商品都是即将揭晓
            //if (jiexiaotype == 1)
            //{
            //    sqlWhere2 += " ZhongChouCount>=(0.6*Price)";
            //}
            sql = string.Format(sql, sqlWhere1,sqlWhere2);
            parameters.Add(new SqlParameter() { ParameterName = "@PageSize", Value = pageSize });
            parameters.Add(new SqlParameter() { ParameterName = "@CurrentPage", Value = currentPage });

            //记录总数计算
            var countParameters = new List<SqlParameter>();
            parameters.ForEach(h => countParameters.Add(new SqlParameter() { ParameterName = h.ParameterName, Value = h.Value }));
            var sqlCount = string.Format("SELECT COUNT(*) CNT FROM [GoodsBaseInfo] G LEFT JOIN HuodongInfo H ON G.ID=H.GoodsID  WHERE G.IsDelete=0 {0} ", sqlWhere1);
            allCount = Converter.TryToInt32(SqlHelper.ExecuteScalar(sqlCount, countParameters.ToArray()));

            if (allCount == 0)
            {
                return null;
            }

            DataTable dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.AsEnumerable().Select(row => new Model.GoodsBaseInfo()
                {
                    ID = Converter.TryToInt32(row["ID"], -1),
                    GoodsName = Converter.TryToString(row["GoodsName"], string.Empty),
                    Category = Converter.TryToString(row["CategoryName"], string.Empty),
                    CreateUserID = Converter.TryToString(row["CreateUserID"], string.Empty),
                    Price = Converter.TryToString(row["Price"], string.Empty),
                    Describe = Converter.TryToString(row["Describe"], string.Empty),
                    DetailIcons = Converter.TryToString(row["DetailIcons"], string.Empty),
                    ShowIcons = Converter.TryToString(row["ShowIcons"], string.Empty),
                    CreateTime = Converter.TryToString(row["CreateTime"], DateTime.MinValue.ToString()),
                    State = Converter.TryToInt32(row["State"], -1),
                    ZhongChouCount = Converter.TryToInt32(row["ZhongChouCount"], 0),
                    FinishedTime = Converter.TryToDateTime(row["FinishedTime"], DateTime.MinValue),
                    HuoDongID = Converter.TryToInt64(row["HuoDongID"], 0),
                }).ToList();
            }
            return null;
        }

        public static Model.GoodsBaseInfo GetGoodsInfoByID(string id)
        {
            var sql = @"SELECT 
		                            G.ID
	                               ,G.GoodsName
	                               ,G.Describe
	                               ,G.Price
	                               ,G.Category
	                               ,G.ShowIcons
	                               ,G.DetailIcons
	                               ,G.CreateTime
	                               ,G.CreateUserID 
								   ,C.ParentId
                                   ,C.CategoryName
                                   ,H.ID AS HuoDongID
                                   ,ZhongChouCount=(SELECT Count(1) FROM OrderInfo where huodongid=H.ID)
                                FROM dbo.GoodsBaseInfo G JOIN dbo.CategoryInfo C
                                ON C.ID=G.Category LEFT JOIN HuoDongInfo H
                                ON G.ID=H.GoodsID
                                WHERE G.IsDelete=0 AND C.IsDelete=0 AND G.ID=@ID AND (H.State!=40 OR H.State IS NULL)
                              ";
            var parameters = new List<SqlParameter>();
            sql = string.Format(sql);
            parameters.Add(new SqlParameter() { ParameterName = "@ID", Value = id });
            DataTable dataTable = SqlHelper.ExecuteDataTable(sql, parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Model.GoodsBaseInfo()
                {
                    ID = Converter.TryToInt32(row["ID"], -1),
                    GoodsName = Converter.TryToString(row["GoodsName"], string.Empty),
                    CreateUserID = Converter.TryToString(row["CreateUserID"], string.Empty),
                    Price = Converter.TryToString(row["Price"], string.Empty),
                    Describe = Converter.TryToString(row["Describe"], string.Empty),
                    DetailIcons = Converter.TryToString(row["DetailIcons"], string.Empty),
                    ShowIcons = Converter.TryToString(row["ShowIcons"], string.Empty),
                    CreateTime = Converter.TryToString(row["CreateTime"], DateTime.MinValue.ToString()),
                    Category = Converter.TryToString(row["Category"], string.Empty),
                    CategoryName = Converter.TryToString(row["CategoryName"], string.Empty),
                    ParentId = Converter.TryToString(row["ParentId"], string.Empty),
                    ZhongChouCount = Converter.TryToInt32(row["ZhongChouCount"], 0),
                    HuoDongID = Converter.TryToInt64(row["HuoDongID"], 0)
                };
            }
            return null;
        }
        #endregion
    }
}

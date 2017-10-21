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
    public static class MSDepartmentInfoDal
    {
        #region 增加

        #endregion

        #region 更新

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

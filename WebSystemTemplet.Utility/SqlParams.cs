using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSystemTemplet.Utility
{
    public class SqlParams
    {
        private Dictionary<string, object> dicParams;

        /// <summary>
        /// 每页数据条数 ，默认 10 条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页码 默认第一页
        /// </summary>
        public int PageIndex { get; set; }

        public SqlParams()
        {
            dicParams = new Dictionary<string, object>();
            this.PageSize = 10;
            this.PageIndex = 1;
        }

        /// <summary>
        /// 是否又改参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns></returns>
        public bool hasParam(string paramName)
        {
            return dicParams.ContainsKey(paramName.ToLower());
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns></returns>
        public object getParam(string paramName)
        {
            paramName = paramName.ToLower();
            if (dicParams.ContainsKey(paramName))
            {
                return dicParams[paramName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        public void addParam(string paramName, object paramValue)
        {
            dicParams.Add(paramName.ToLower(), paramValue);
        }

        /// <summary>
        /// 添加有效参数（null 或 空字符串 会被过滤）
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>

        public void addUsefulParam(string paramName, string paramValue)
        {
            paramName = paramName.ToLower();
            if (paramValue == null || paramValue.ToString().IsNullOrWhiteSpace())
            {
                return;
            }
            dicParams.Add(paramName, paramValue);
        }

        /// <summary>
        /// 添加有效参数（ -1 会被过滤）
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        public void addUsefulParam(string paramName, long paramValue)
        {
            paramName = paramName.ToLower();
            if (paramValue == -1)
            {
                return;
            }
            dicParams.Add(paramName, paramValue);
        }
    }
}

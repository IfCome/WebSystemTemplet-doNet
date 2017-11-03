using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSystemTemplet.Model
{
    public enum PositionType
    {
        系统管理员 = 10001,
        配置管理员 = 10002,

        院长 = 20001,

        系主任 = 30001,
        助理 = 30002,
        讲师 = 30003,

        班主任 = 40001,
        助教 = 40002,
        班长 = 40003,
        学生 = 40004
    }

    public enum RoleType
    {
        教职工 = 1,
        学生 = 2,
        管理员 = 3
    }
    public enum DepartmentLevel
    {
        学院 = 1,
        专业 = 2,
        班级 = 3
    }

    public enum CacheKeyName
    {
        MS_CacheKey_DepartmentName,   //部门(ID-名称)dic
        MS_CacheKey_DepartmentList,   //部门列表
    }

    public enum LogType
    {
        登录 = 101,
        访问 = 201,
        操作 = 301,
        异常 = 401,
        其它 = 501
    }
}

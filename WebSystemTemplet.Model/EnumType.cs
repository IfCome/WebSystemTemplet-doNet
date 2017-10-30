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
        MS_CacheKey_PositionName,   // 职位名称dic
        MS_CacheKey_PositionList,   // 职位列表
    }
}

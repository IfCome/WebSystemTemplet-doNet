using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.UI.Filters;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class MSDepartmentInfoController : PCBaseController
    {

        [HttpGet]
        public ActionResult GetDepartmentListByParentId(long parentId)
        {
            List<Model.Admin.MSDepartmentInfo> departmentList = BLL.Admin.MSDepartmentInfoBll.GetAllDepartmentInfoParentId(parentId) ?? new List<Model.Admin.MSDepartmentInfo>();

            return Json(new
            {
                Message = "OK",
                Rows = departmentList.Select(d => new
                {
                    d.DepartmentID,
                    d.DepartmentName,
                    d.ParentID
                })
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 组织架构概览
        /// </summary>
        [ActionLogFilter(InitLogMsg = "访问组织架构概览页面")]
        public ActionResult DepartmentTree()
        {
            // 查询学院
            List<Model.Admin.MSDepartmentInfo> schoolList = BLL.Admin.MSDepartmentInfoBll.GetAllDepartmentInfoParentId(0) ?? new List<Model.Admin.MSDepartmentInfo>();
            if (schoolList != null && schoolList.Count > 0)
            {
                Models.Admin.DepartmentTreeModelOut schoolNode = new Models.Admin.DepartmentTreeModelOut();
                schoolNode.name = schoolList[0].DepartmentName;

                Model.Admin.MSUserInfo directorUserInfo = BLL.Admin.MSUserPositionRelationBll.GetUserByDepartmentIdAndPositionType(schoolList[0].DepartmentID, (int)Model.PositionType.院长);
                schoolNode.directorName = "院长：" + (directorUserInfo == null ? "--" : directorUserInfo.RealName);

                schoolNode.children = new List<Models.Admin.DepartmentTreeModelOut>();

                // 查询专业
                List<Model.Admin.MSDepartmentInfo> majorList = BLL.Admin.MSDepartmentInfoBll.GetAllDepartmentInfoParentId(schoolList[0].DepartmentID) ?? new List<Model.Admin.MSDepartmentInfo>();
                if (majorList != null && majorList.Count > 0)
                {
                    foreach (Model.Admin.MSDepartmentInfo majorItem in majorList)
                    {
                        Models.Admin.DepartmentTreeModelOut majorNode = new Models.Admin.DepartmentTreeModelOut();
                        majorNode.name = majorItem.DepartmentName;

                        directorUserInfo = BLL.Admin.MSUserPositionRelationBll.GetUserByDepartmentIdAndPositionType(majorItem.DepartmentID, (int)Model.PositionType.系主任);
                        majorNode.directorName = "系主任：" + (directorUserInfo == null ? "--" : directorUserInfo.RealName);

                        majorNode.children = new List<Models.Admin.DepartmentTreeModelOut>();

                        // 查询班级
                        List<Model.Admin.MSDepartmentInfo> classList = BLL.Admin.MSDepartmentInfoBll.GetAllDepartmentInfoParentId(majorItem.DepartmentID) ?? new List<Model.Admin.MSDepartmentInfo>();
                        if (classList != null && classList.Count > 0)
                        {
                            foreach (Model.Admin.MSDepartmentInfo classItem in classList)
                            {
                                Models.Admin.DepartmentTreeModelOut classNode = new Models.Admin.DepartmentTreeModelOut();
                                classNode.name = classItem.DepartmentName;

                                directorUserInfo = BLL.Admin.MSUserPositionRelationBll.GetUserByDepartmentIdAndPositionType(classItem.DepartmentID, (int)Model.PositionType.班主任);
                                classNode.directorName = "班主任：" + (directorUserInfo == null ? "--" : directorUserInfo.RealName);

                                majorNode.children.Add(classNode);
                            }
                        }
                        schoolNode.children.Add(majorNode);
                    }
                }
                // 将对象转化为json字符串传到前端
                ViewBag.DepartmentTreeNode = JsonEntityExchange<object>.ConvertEntityToJson(schoolNode);
            }
            return View("~/Views/Admin/MSDepartmentInfo/DepartmentTree.cshtml");
        }

        #region 专业管理

        [ActionLogFilter(InitLogMsg = "访问专业列表页面")]
        public ActionResult MajorList()
        {
            return View("~/Views/Admin/MSDepartmentInfo/MajorList.cshtml");
        }
        
        [HttpGet]
        public ActionResult GetMajorList(string pageIndex, string pageSize, string keyWords)
        {
            SqlParams sqlParams = new SqlParams();
            sqlParams.PageIndex = Converter.TryToInt32(pageIndex, 1);
            sqlParams.PageSize = Converter.TryToInt32(pageSize, sqlParams.PageSize);
            sqlParams.addUsefulParam("keyWords", keyWords);
            sqlParams.addUsefulParam("DepartmentLevel", (int)Model.DepartmentLevel.专业);

            int allCount = 0;
            List<Model.Admin.MSDepartmentInfo> departmentList = BLL.Admin.MSDepartmentInfoBll.GetAllMSDepartmentInfoPageList(sqlParams, out allCount);

            return Json(new
            {
                Rows = departmentList.Select(d =>
                {
                    Model.Admin.MSUserInfo directiorUser = BLL.Admin.MSUserPositionRelationBll.GetUserByDepartmentIdAndPositionType(d.DepartmentID, (int)Model.PositionType.系主任);
                    return new
                    {
                        d.DepartmentID,
                        d.DepartmentName,
                        ParentName = BLL.Admin.MSDepartmentInfoBll.GetDepartmentNameById(d.ParentID).IfEmptyToString("--"),
                        DirectorName = directiorUser == null ? "--" : directiorUser.RealName,// 系主任
                        DirectorId = directiorUser == null ? 0 : directiorUser.UserID,// 系主任
                    };
                }),
                AllCount = allCount
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionLogFilter(InitLogMsg = "保存专业信息")]
        [HttpPost]
        public ActionResult SaveMajorCallBack(string departmentId, string departmentName)
        {
            if (departmentName.IsNullOrWhiteSpace())
            {
                return Json(new { Message = "专业名称无效" });
            }
            departmentName = departmentName.Trim();
            string msg;
            Model.Admin.MSDepartmentInfo departmentInfo = new Model.Admin.MSDepartmentInfo()
            {
                DepartmentID = Converter.TryToInt64(departmentId, -1),
                DepartmentName = departmentName,
                DepartmentLevel = (int)Model.DepartmentLevel.专业,
                ParentID = 1, //学院ID
                Deleted = 0,
                CreateTime = DateTime.Now,
                CreateUser = Model.Identity.LoginUserInfo.UserID,
                UpdateTime = DateTime.Now,
                UpdateUser = Model.Identity.LoginUserInfo.UserID,
            };
            if (BLL.Admin.MSDepartmentInfoBll.SaveDepartmentInfo(departmentInfo, out msg))
            {
                msg = "OK";
            }
            return Json(new { Message = msg });
        }

        #endregion

        #region 班级管理

        [ActionLogFilter(InitLogMsg = "访问班级列表页面")]
        public ActionResult ClassList()
        {
            List<Model.Admin.MSDepartmentInfo> departmentList = BLL.Admin.MSDepartmentInfoBll.GetAllDepartmentInfoByLevel((int)Model.DepartmentLevel.专业);
            ViewBag.DepartmentList = departmentList;
            return View("~/Views/Admin/MSDepartmentInfo/ClassList.cshtml");
        }

        [HttpGet]
        public ActionResult GetClassList(string pageIndex, string pageSize, long majorId, string keyWords)
        {
            SqlParams sqlParams = new SqlParams();
            sqlParams.PageIndex = Converter.TryToInt32(pageIndex, 1);
            sqlParams.PageSize = Converter.TryToInt32(pageSize, sqlParams.PageSize);
            sqlParams.addUsefulParam("keyWords", keyWords);
            sqlParams.addUsefulParam("ParentId", majorId);
            sqlParams.addUsefulParam("DepartmentLevel", (int)Model.DepartmentLevel.班级);

            int allCount = 0;
            List<Model.Admin.MSDepartmentInfo> departmentList = BLL.Admin.MSDepartmentInfoBll.GetAllMSDepartmentInfoPageList(sqlParams, out allCount);

            return Json(new
            {
                Rows = departmentList.Select(d =>
                {
                    Model.Admin.MSUserInfo directiorUser = BLL.Admin.MSUserPositionRelationBll.GetUserByDepartmentIdAndPositionType(d.DepartmentID, (int)Model.PositionType.班主任);
                    return new
                    {
                        d.DepartmentID,
                        d.DepartmentName,
                        MajorId = d.ParentID,
                        ParentName = BLL.Admin.MSDepartmentInfoBll.GetDepartmentNameById(d.ParentID).IfEmptyToString("--"),
                        DirectorName = directiorUser == null ? "--" : directiorUser.RealName,// 班主任姓名
                        DirectorId = directiorUser == null ? 0 : directiorUser.UserID,// 班主任 ID
                    };
                }),
                AllCount = allCount
            }, JsonRequestBehavior.AllowGet);
        }

        [ActionLogFilter(InitLogMsg = "保存班级信息")]
        [HttpPost]
        public ActionResult SaveClassCallBack(string departmentId, long majorId, string departmentName)
        {
            if (departmentName.IsNullOrWhiteSpace())
            {
                return Json(new { Message = "班级名称无效" });
            }
            departmentName = departmentName.Trim();
            string msg;
            Model.Admin.MSDepartmentInfo departmentInfo = new Model.Admin.MSDepartmentInfo()
            {
                DepartmentID = Converter.TryToInt64(departmentId, -1),
                DepartmentName = departmentName,
                DepartmentLevel = (int)Model.DepartmentLevel.班级,
                ParentID = majorId, //专业ID
                Deleted = 0,
                CreateTime = DateTime.Now,
                CreateUser = Model.Identity.LoginUserInfo.UserID,
                UpdateTime = DateTime.Now,
                UpdateUser = Model.Identity.LoginUserInfo.UserID,
            };
            if (BLL.Admin.MSDepartmentInfoBll.SaveDepartmentInfo(departmentInfo, out msg))
            {
                msg = "OK";
            }
            return Json(new { Message = msg });
        }


        #endregion

        /// <summary>
        /// 设置系主任/班主任/班长
        /// </summary>
        [ActionLogFilter(InitLogMsg = "设置系主任/班主任/班长")]
        [HttpPost]
        public ActionResult SetDirectorInfo(long departmentId, long directorId, int positionCode)
        {
            string msg = "设置失败！";
            if (BLL.Admin.MSDepartmentInfoBll.SetDirectorInfo(departmentId, directorId, positionCode, out msg))
            {
                msg = "OK";
            }
            return Json(new { Message = msg });
        }
    }
}
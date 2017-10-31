using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult DepartmentList()
        {
            return View("~/Views/Admin/MSDepartmentInfo/DepartmentList.cshtml");
        }


        public ActionResult DepartmentTree()
        {
            return View("~/Views/Admin/MSDepartmentInfo/DepartmentTree.cshtml");
        }

        #region 专业管理

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
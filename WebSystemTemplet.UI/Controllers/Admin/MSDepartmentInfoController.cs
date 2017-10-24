﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class MSDepartmentInfoController : PCBaseController
    {
        // GET: MSDepartmentInfo
        public ActionResult DepartmentList()
        {
            return View("~/Views/Admin/MSDepartmentInfo/DepartmentList.cshtml");
        }

        public ActionResult MajorList()
        {
            return View("~/Views/Admin/MSDepartmentInfo/MajorList.cshtml");
        }

        public ActionResult GetMajorList(string pageIndex, string pageSize, string keyWords)
        {
            SqlParams sqlParams = new SqlParams();
            sqlParams.PageIndex = Converter.TryToInt32(pageIndex, 1);
            sqlParams.PageSize = Converter.TryToInt32(pageSize, sqlParams.PageSize);
            sqlParams.addUsefulParam("keyWords", keyWords);
            sqlParams.addUsefulParam("DepartmentLevel", (int)Model.DepartmentLevel.专业);

            int allCount = 0;
            List<Model.Admin.MSDepartmentInfo> departmentList = BLL.Admin.MSDepartmentInfoBll.GetAllMSDepartmentInfoList(sqlParams, out allCount);

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
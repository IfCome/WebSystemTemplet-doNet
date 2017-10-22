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

    }
}
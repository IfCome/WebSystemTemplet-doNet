using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSystemTemplet.Model;
using WebSystemTemplet.Utility;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class MSUserInfoController : PCBaseController
    {
        // GET: MSUserInfo
        public ActionResult TeacherList()
        {
            return View("~/Views/Admin/MSUserInfo/TeacherList.cshtml");
        }

        [HttpGet]
        public ActionResult GetTeacherList(Models.Admin.GetUserInfoListIn InModel)
        {
            SqlParams sqlParams = new SqlParams();
            sqlParams.PageIndex = Converter.TryToInt32(InModel.PageIndex, 1);
            sqlParams.PageSize = Converter.TryToInt32(InModel.PageSize, sqlParams.PageSize);
            sqlParams.addUsefulParam("roleId", (int)RoleType.教职工);
            sqlParams.addUsefulParam("PositionType", InModel.PositionType);
            sqlParams.addUsefulParam("majorId", InModel.MajorId);
            sqlParams.addUsefulParam("keyWords", InModel.KeyWords);

            int allCount = 0;
            List<Model.Admin.MSUserInfo> userInfoList = BLL.Admin.MSUserInfoBll.GetAllUserInfoList(sqlParams, out allCount);
            return Json(new
            {
                Rows = userInfoList.Select(u => new
                {
                    u.UserID,
                    u.UserName,
                    u.RealName,
                    MajorName = "hahah",
                    u.PositionName,
                }),
                AllCount = allCount
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TeacherAddPage()
        {
            return View("~/Views/Admin/MSUserInfo/TeacherAdd.cshtml");
        }
    }
}
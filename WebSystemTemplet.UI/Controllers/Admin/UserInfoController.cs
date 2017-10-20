﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using WebSystemTemplet.Utility;
using WebSystemTemplet.Model;

namespace WebSystemTemplet.UI.Controllers.Admin
{
    public class UserInfoController : PCBaseController
    {
        //
        // GET: /UserInfo/

        // 用户管理页面
        public ActionResult IndexPage(string keyWords)
        {
            ViewBag.KeyWords = Converter.TryToString(keyWords, "");
            return View();
        }

        // 查看用户信息
        public ActionResult SeeUserInfo(long id)
        {
            // 查询用户信息
            Model.UserInfo userInfo = BLL.BackgroundUserBll.GetSingleUserInfo(id);
            if (userInfo == null)
            {
                return Content("该用户不存在！！");
            }

            // 查询该用户的操作日志
            List<Model.BackgroundUserInfo_log> logList = BLL.BackgroundUserBll_log.GetSingleUserTop10Logs(id);
            ViewBag.LogList = logList;
            ViewBag.IsSelf = (userInfo.ID == Identity.LoginUserInfo.ID);

            return View(userInfo);
        }

        // 添加用户页面
        public ActionResult AddUser()
        {
            if (Identity.LoginUserInfo.RoleType != 10)
            {
                return Content("只有管理员才能添加用户");
            }
            return View();
        }

        // 提交用户信息
        [HttpPost]
        public ActionResult AddUserCallBack(Models.Admin.BackGroundUserInfoModelIn InModel)
        {
            string errorType = "";
            string msg = "OK";
            if (Identity.LoginUserInfo.RoleType != 10)
            {
                errorType = "alert";
                msg = "只有管理员才能添加用户";
            }
            // 验证参数
            else if (string.IsNullOrWhiteSpace(InModel.UserName))
            {
                errorType = "UserName";
                msg = "请输入用户名";
            }
            else if (BLL.BackgroundUserBll.IsExistUserName(InModel.UserName))
            {
                errorType = "UserName";
                msg = "该用户名已存在";
            }

            else if (string.IsNullOrWhiteSpace(InModel.Password))
            {
                errorType = "Password";
                msg = "请输入密码";
            }
            else if (InModel.Password != InModel.RePassword)
            {
                errorType = "RePassword";
                msg = "两次输入的密码不一致";
            }
            else
            {
                // 添加用户
                Model.UserInfo userInfo = new Model.UserInfo()
                {
                    UserName = InModel.UserName,
                    PassWord = Security.getMD5ByStr(InModel.Password),
                    RealName = InModel.RealName ?? "",
                    RoleType = Converter.TryToInt32(InModel.RoleType, 20),
                    Phone = InModel.Phone ?? "",
                    Email = InModel.Email ?? "",
                    QQ = InModel.QQ ?? "",
                    HeadIcon = InModel.HeadIcon ?? "",
                    CreateTime = DateTime.Now
                };
                if (BLL.BackgroundUserBll.AddUserInfo(userInfo) == false)
                {
                    errorType = "alert";
                    msg = "添加失败，请重试";
                }
                else
                {
                    // 记录日志
                    string logTitle = "添加后台用户";
                    string logMsg = string.Format("添加用户信息：用户名【{0}】，角色【{1}】", userInfo.UserName, userInfo.RoleType == 10 ? "管理员" : "普通用户");
                    BLL.BackgroundUserBll_log.AddLog(logTitle, logMsg, Request.UserHostAddress);
                }
            }
            return Json(new { Message = msg, ErrorType = errorType });
        }


        // 编辑用户页面
        public ActionResult EditUser(long id)
        {
            if (Identity.LoginUserInfo.RoleType != 10)
            {
                return Content("只有管理员才能编辑用户");
            }
            // 查询用户信息
            Model.UserInfo userInfo = BLL.BackgroundUserBll.GetSingleUserInfo(id);
            if (userInfo == null)
            {
                return Content("该用户不存在！！");
            }
            ViewBag.IsSelf = false;
            return View(userInfo);
        }


        [HttpPost]
        public ActionResult EditUserCallBack(Models.Admin.BackGroundUserInfoModelIn InModel)
        {
            string errorType = "";
            string msg = "OK";
            //if (Identity.LoginUserInfo.RoleType != 10)
            //{
            //    errorType = "alert";
            //    msg = "只有管理员才能修改用户信息";
            //}
            // 验证参数
            if (InModel.ID == 0)
            {
                errorType = "alert";
                msg = "用户信息无效";
            }
            else
            {
                // 查询用户原信息
                Model.UserInfo oldUserInfo = BLL.BackgroundUserBll.GetSingleUserInfo(InModel.ID);

                // 整理用户新信息
                Model.UserInfo newUserInfo = new Model.UserInfo()
                {
                    ID = InModel.ID,
                    RoleType = Converter.TryToInt32(InModel.RoleType, 20),
                    RealName = InModel.RealName ?? "",
                    Phone = InModel.Phone ?? "",
                    Email = InModel.Email ?? "",
                    QQ = InModel.QQ ?? "",
                    HeadIcon = InModel.HeadIcon ?? ""
                };

                if (BLL.BackgroundUserBll.UpdateUserInfo(newUserInfo) == false)
                {
                    errorType = "alert";
                    msg = "保存失败，请刷新用户列表重试";
                }
                else
                {
                    // 比较新旧用户信息，整理日志内容
                    string diffContent = BLL.BackgroundUserBll.GetDiffContent(newUserInfo, oldUserInfo);
                    // 记录日志
                    string logTitle = "修改后台用户";
                    string logMsg = string.Format("修改用户信息：用户名【{0}】，修改信息【{1}】", oldUserInfo.UserName, diffContent);
                    BLL.BackgroundUserBll_log.AddLog(logTitle, logMsg, Request.UserHostAddress);
                }
            }
            return Json(new { Message = msg, ErrorType = errorType });
        }


        [HttpGet]
        public ActionResult GetUserInfoList(Models.Admin.GetUserInfoListIn InModel)
        {
            InModel.PageSize = Converter.TryToInt32(InModel.PageSize, 10);
            InModel.PageIndex = Converter.TryToInt32(InModel.PageIndex, 1);

            List<Model.UserInfo> userInfoList = new List<Model.UserInfo>();
            int allCount = 0;
            userInfoList = BLL.BackgroundUserBll.GetAllUserInfoList((int)InModel.PageSize, (int)InModel.PageIndex, (int)InModel.RoleType, InModel.KeyWords, out allCount);
            return Json(new
            {
                Rows = userInfoList.Select(u => new
                {
                    u.ID,
                    u.UserName,
                    u.RealName,
                    RoleType = u.RoleType == 10 ? "管理员" : "普通员工",
                    CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    LastLoginTime = u.LastLoginTime == DateTime.MinValue ? "从未登陆" : u.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                }),
                AllCount = allCount
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadIcon()
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { Message = "请选择文件！" });
            }
            HttpPostedFileBase imgFile = Request.Files[0];  // 获取上传文件
            if (imgFile.ContentLength == 0)
            {

                return Json(new { Message = "请选择文件！" });
            }
            #region 校验文件格式是否正确

            // 根据文件后缀名判断文件类型
            String extendname = imgFile.FileName.Substring(imgFile.FileName.LastIndexOf('.') + 1).ToLower();
            if (extendname != "jpg" && extendname != "jpeg" && extendname != "bmp" &&
                extendname != "gif" && extendname != "tiff" && extendname != "png")
            {
                return Json(new { Message = "文件类型错误！" });
            }

            #endregion

            // 上传图片到服务器
            FileUpToImg file = new FileUpToImg(imgFile, UploadImageFolderType.UserIcons.ToString(), "_t", 150, 150);


            if (file.IsSuccess == false)
            {
                return Json(new { Message = file.ErrorInfo });
            }
            String imgUrl = file.ThumbImageUrl;
            return Json(new { Message = "OK", ImgUrl = imgUrl });
        }

        // 添加用户页面
        public ActionResult DeleteUser(long id)
        {
            if (Identity.LoginUserInfo.RoleType != 10)
            {
                return Json(new { Message = "只有管理员才能编辑用户" });
            }
            // 查询用户信息
            Model.UserInfo userInfo = BLL.BackgroundUserBll.GetSingleUserInfo(id);
            if (userInfo == null)
            {
                return Json(new { Message = "该用户不存在或已被删除！！" });
            }
            if (BLL.BackgroundUserBll.DeleteSingleUserInfo(id))
            {
                // 记录日志
                string logTitle = "删除后台用户";
                string logMsg = string.Format("删除用户信息：用户名【{0}】，角色【{1}】", userInfo.UserName, userInfo.RoleType == 10 ? "管理员" : "普通用户");
                BLL.BackgroundUserBll_log.AddLog(logTitle, logMsg, Request.UserHostAddress);
                return Json(new { Message = "OK" });
            }
            else
            {
                return Json(new { Message = "删除失败！" });
            }

        }


        // 查看个人资料
        public ActionResult MyDetails()
        {
            // 查询用户信息
            Model.UserInfo userInfo = BLL.BackgroundUserBll.GetSingleUserInfo(Identity.LoginUserInfo.ID);
            if (userInfo == null)
            {
                return Content("该用户不存在！！");
            }

            // 查询该用户的操作日志
            List<Model.BackgroundUserInfo_log> logList = BLL.BackgroundUserBll_log.GetSingleUserTop10Logs(Identity.LoginUserInfo.ID);
            ViewBag.LogList = logList;
            ViewBag.IsSelf = true;
            return View("~/Views/UserInfo/SeeUserInfo.cshtml", userInfo);
        }

        // 编辑个人资料
        public ActionResult EditMyDetails()
        {
            // 查询用户信息
            Model.UserInfo userInfo = BLL.BackgroundUserBll.GetSingleUserInfo(Identity.LoginUserInfo.ID);
            if (userInfo == null)
            {
                return Content("该用户不存在！！");
            }
            ViewBag.IsSelf = true;
            if (Identity.LoginUserInfo.RoleType == 10)
            {
                ViewBag.IsSelf = false; // 管理员不需要做这些限制
            }
            return View("~/Views/UserInfo/EditUser.cshtml", userInfo);
        }

        // 修改密码
        public ActionResult EditPassword(string oldPassword, string newPassword, string rePassword)
        {
            string errorType = "";
            string msg = "OK";

            // 验证参数
            if (Identity.LoginUserInfo.PassWord != Security.getMD5ByStr(oldPassword))
            {
                errorType = "oldPassword";
                msg = "原密码错误";
            }
            else if (newPassword != rePassword)
            {
                errorType = "rePassword";
                msg = "两次输入的密码不一致";
            }
            else
            {
                if (BLL.BackgroundUserBll.UpdateUserPassword(Identity.LoginUserInfo.ID, newPassword) == false)
                {
                    errorType = "alert";
                    msg = "修改失败，请稍后重试";
                }
                else
                {
                    // 记录日志
                    string logTitle = "修改登陆密码";
                    string logMsg = "修改登录密码";
                    BLL.BackgroundUserBll_log.AddLog(logTitle, logMsg, Request.UserHostAddress);
                }
            }
            return Json(new { Message = msg, ErrorType = errorType });
        }
    }
}

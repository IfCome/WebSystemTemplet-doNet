using System.Web;

namespace WebSystemTemplet.UI.Models.Admin
{
    public class MSUserInfoModel
    {
        private string userid;
        public string UserId
        {
            get
            {
                var temp = string.IsNullOrEmpty(userid) ? string.Empty : userid;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { userid = value; }
        }

        private string username;
        public string UserName
        {
            get
            {
                var temp = string.IsNullOrEmpty(username) ? string.Empty : username;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { username = value; }
        }

        private string realname;
        public string RealName
        {
            get
            {
                var temp = string.IsNullOrEmpty(realname) ? string.Empty : realname;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { realname = value; }
        }

        private string password;
        public string Password
        {
            get
            {
                var temp = string.IsNullOrEmpty(password) ? string.Empty : password;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { password = value; }
        }

        private string roleid;
        public string RoleId
        {
            get
            {
                var temp = string.IsNullOrEmpty(roleid) ? string.Empty : roleid;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { roleid = value; }
        }

        private string isPresident;
        public string IsPresident
        {
            get
            {
                var temp = string.IsNullOrEmpty(isPresident) ? string.Empty : isPresident;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { isPresident = value; }
        }

        private string majorid;
        public string MajorId
        {
            get
            {
                var temp = string.IsNullOrEmpty(majorid) ? string.Empty : majorid;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { majorid = value; }
        }

        private string classid;
        public string ClassId
        {
            get
            {
                var temp = string.IsNullOrEmpty(classid) ? string.Empty : classid;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { classid = value; }
        }

        /// <summary>
        /// 性别：0-女 1-男
        /// </summary>
        private byte gender = 1;
        public byte Gender
        {
            get
            {
                return (byte)(gender == 0 ? 0 : 1);
            }
            set
            {
                gender = value;
            }
        }


        private string telephone;
        public string Telephone
        {
            get
            {
                var temp = string.IsNullOrEmpty(telephone) ? string.Empty : telephone;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { telephone = value; }
        }

        private string iconurl;
        public string IconUrl
        {
            get
            {
                var temp = string.IsNullOrEmpty(iconurl) ? string.Empty : iconurl;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { iconurl = value; }
        }

        private string qq;
        public string QQ
        {
            get
            {
                var temp = string.IsNullOrEmpty(qq) ? string.Empty : qq;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { qq = value; }
        }

        private string email;
        public string Email
        {
            get
            {
                var temp = string.IsNullOrEmpty(email) ? string.Empty : email;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { email = value; }
        }

        private string remark;
        public string Remark
        {
            get
            {
                var temp = string.IsNullOrEmpty(remark) ? string.Empty : remark;
                temp = HttpUtility.UrlDecode(temp);
                return temp;
            }
            set { remark = value; }
        }
    }
}
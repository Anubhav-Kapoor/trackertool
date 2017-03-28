using ProjectTracker.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Task_and_Leave_Tracker
{
    public partial class TaskManagerAPI : System.Web.UI.Page
    {
        static UserDetailsBLL userBll = new UserDetailsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region Sign-in
        [System.Web.Services.WebMethod]
        public static String Login(String ntid, String password)
        {
         
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
          try
            {
            DataTable dt = userBll.ViewUserDetailsBLL(ntid);
            String Ntid = dt.Rows[0]["Ntid"].ToString();
            String Password = dt.Rows[0]["Password"].ToString();

            if (Ntid == ntid && Password == password)
            {

                resultObject.Response.Status = "Success";
                resultObject.Response.Reason = "Welcome!!";

            }
            else if (Ntid != ntid || Password != password)
            {
                resultObject.Response.Status = "Fail";
                resultObject.Response.Reason = "Username Or Password is Incorrect";
            }
            // else
            //  {
            //      Response.Redirect("SignUp.aspx");
            //  }
        }
               catch (Exception ex)
            {
                resultObject.Response.Status = "Fail";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion
    }
}
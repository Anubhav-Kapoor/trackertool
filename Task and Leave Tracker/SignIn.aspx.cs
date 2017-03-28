using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectTracker.BLL;
using System.Data;
using System.Web.Script.Serialization;

namespace Task_and_Leave_Tracker
{
    public partial class SignIn : System.Web.UI.Page
    {
        static UserDetailsBLL userBll = new UserDetailsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [System.Web.Services.WebMethod]
        public static String Login(String ntid, String password)
        {
           
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            DataTable dt = userBll.ViewUserDetailsBLL(ntid);
            String Ntid = dt.Rows[0]["Ntid"].ToString();
            String Password = dt.Rows[0]["Password"].ToString();

            if(Ntid==ntid && Password==password)
            {
                
                resultObject.Response.Status = "Success";
                resultObject.Response.Reason = "Welcome!!";

            }
            else if (Ntid != ntid ||Password != password)
            {
                resultObject.Response.Status = "Fail";
                resultObject.Response.Reason = "Username Or Password is Incorrect";
            }
          // else
          //  {
          //      Response.Redirect("SignUp.aspx");
          //  }
            return oSerializer.Serialize(resultObject);
        }
    }
}
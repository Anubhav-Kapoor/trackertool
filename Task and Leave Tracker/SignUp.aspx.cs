using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectTracker.BLL;
using System.Web.Script.Serialization;
using System.Web.Services;


namespace Task_and_Leave_Tracker
{
    public partial class SignUp : System.Web.UI.Page
    {

        static UserDetailsBLL userBll = new UserDetailsBLL();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

       [System.Web.Services.WebMethod]
        public static String CreateAccount(String ntid, String firstName, String lastName, String roleId, String phone, String email, String password)
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            Boolean value = userBll.ViewUserExistDetailsBLL(ntid);
           try{
           if (!value)
             {

                 if (ntid != "" && firstName != "" && lastName != "" && roleId != "" && phone != "" && email != "" && password != "")
                 {
                     int result = userBll.InsertUserDetailsBLL(ntid, firstName, lastName, roleId, phone, email, password);

                     if (result > 0)
                     {
                         resultObject.Response.Status = "Success";
                         resultObject.Response.Reason = "You are successfully registered.";
                         
                     }
                     else
                     {
                         resultObject.Response.Status = "Fail";
                         resultObject.Response.Reason = "Please try to register with different email id";
                     }
                 }
                 else
                 {
                     resultObject.Response.Status = "Fail";
                     resultObject.Response.Reason = "Input Data invalid.";

                 }
           }
             
             else
             {
                 resultObject.Response.Status = "Fail";
                 resultObject.Response.Reason = "User Already exists!!";
             }

            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Fail";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }




    }
}
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

        #region Sign-up
        [System.Web.Services.WebMethod]
        public static String CreateAccount(String ntid, String firstName, String lastName, String roleId, String phone, String email, String password)
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            Boolean value = userBll.ViewUserExistDetailsBLL(ntid);
            try
            {
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
        #endregion

        [System.Web.Services.WebMethod]
        public static String GetUserDetails()
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();

            try
            {

                string name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Fail";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
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
                if (ntid != "" && password != "")
                {
                    DataTable dt = userBll.ViewUserDetailsBLL(ntid);
                    String Ntid = dt.Rows[0]["Ntid"].ToString();
                    String Password = dt.Rows[0]["Password"].ToString();
                    String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                    string hashedPassword = Security.HashSHA1(password + UserGuid);


                    if (Ntid == ntid && Password == hashedPassword)
                    {

                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Welcome!!";

                    }
                    else
                    {
                        resultObject.Response.Status = "Fail";
                        resultObject.Response.Reason = "Username Or Password is Incorrect";
                    }

                }
                else
                {
                    resultObject.Response.Status = "Empty";
                    resultObject.Response.Reason = "Enter your details !!";
                }
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

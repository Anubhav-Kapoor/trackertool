using ProjectTracker.BLL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Globalization;


namespace Task_and_Leave_Tracker
{
    public partial class TaskManagerAPI : System.Web.UI.Page
    {
        static UserDetailsBLL userBll = new UserDetailsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Mail Method
        [System.Web.Services.WebMethod]
        public static String SendingMail(String mailId, String from, String subject, String body)
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(mailId);
                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;
                SmtpClient smtpClient = new SmtpClient("mailin.owenscorning.com");
                smtpClient.Send(mailMessage);

                resultObject.Response.Status = "Success";
                resultObject.Response.Reason = "Email has been sent!!";

            }


            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Could not sent the email" + ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion

        #region Sign-up
        [System.Web.Services.WebMethod]
        public static String CreateAccount(String ntid, String firstName, String lastName, String roleId, String phone, String email, String password)
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            Boolean value = userBll.CheckUserExistDetailsBLL(ntid);
            try
            {
                if (!value)
                {

                    if (ntid != "" && firstName != "" && lastName != "" && roleId != "" && phone != "" && email != "" && password != "")
                    {
                        int result = userBll.InsertUserDetailsBLL(ntid, firstName, lastName, roleId, phone, email, password);

                        if (result > 0)
                        {

                            String from = "bhawneet.singh@owenscorning.com";
                            String subject = "Welcome to Tracker Tool";
                            String body = "Dear " + firstName + " " + lastName + ",<br/>" + " <br />Thanks for registering with TrackerTool" + "<br />Please note your login details:" + "<br />NTID: " + ntid + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";

                            SendingMail(email, from, subject, body);

                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "You are successfully registered.";

                        }
                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = "Your Account Is Not Created!!";
                        }
                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "Input Data invalid.";

                    }
                }

                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "User Already exists!!";
                }

            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region Get User Details
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
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


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
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid);
                    if (dt.Rows.Count > 0)
                    {

                        String Password = dt.Rows[0]["Password"].ToString();
                        String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                        string hashedPassword = Security.HashSHA1(password + UserGuid);

                        if (Password == hashedPassword)
                        {

                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "User Authentication Success";


                        }
                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = "Username Or Password is Incorrect!!!";
                        }

                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "User does not exist!!!";
                    }
                }
                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "Username or Password cannot be empty!!!";
                }
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region  ForgotPassword
        [System.Web.Services.WebMethod]
        public static String ForgotPassword(String ntid)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                if (ntid != "")
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid);
                    String FirstName = dt.Rows[0]["FirstName"].ToString();
                    String LastName = dt.Rows[0]["LastName"].ToString();
                    String RoleId = dt.Rows[0]["RoleId"].ToString();
                    String PhoneNo = dt.Rows[0]["PhoneNo"].ToString();
                    String EmailId = dt.Rows[0]["EmailId"].ToString();
                    String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                    String Password = Membership.GeneratePassword(12, 9);
                    String hashedPwd = Security.HashSHA1(Password + UserGuid);
                    int result = userBll.UpdateUserDetailsBLL(ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, hashedPwd, UserGuid);
                    if (result > 0)
                    {
                        String from = "bhawneet.singh@owenscorning.com";
                        String subject = "Tracker Tool - Password Reset";
                        String body = "Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your Temporary Password for TrackerTool is : " + Password + "<br />Please change your password after login. " + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";

                        SendingMail(EmailId, from, subject, body);

                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Your Password Has Been Reset";
                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = " User Details Are Not Updated!!";
                    }

                }

                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "Enter Your Correct Ntid !!";
                }
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region  ChangePassword
        [System.Web.Services.WebMethod]
        public static String ChangePassword(String ntid, String currentPassword, String newPassword)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                if (ntid != "" && currentPassword != "" && newPassword != "")
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid);

                    String OldPassword = dt.Rows[0]["Password"].ToString();
                    String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                    String hashedPwd = Security.HashSHA1(currentPassword + UserGuid);

                    String FirstName = dt.Rows[0]["FirstName"].ToString();
                    String LastName = dt.Rows[0]["LastName"].ToString();
                    String RoleId = dt.Rows[0]["RoleId"].ToString();
                    String PhoneNo = dt.Rows[0]["PhoneNo"].ToString();
                    String EmailId = dt.Rows[0]["EmailId"].ToString();

                    if (OldPassword == hashedPwd)
                    {
                        Guid userGuid = System.Guid.NewGuid();

                        // Hash the newPassword together with our unique userGuid
                        String hashedNewPassword = Security.HashSHA1(newPassword + userGuid.ToString());
                        int result = userBll.UpdateUserDetailsBLL(ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, hashedNewPassword, userGuid.ToString());

                        if (result > 0)
                        {
                            String from = "bhawneet.singh@owenscorning.com";
                            String subject = "Tracker Tool -  Password Changed";
                            String body = "Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your Password for TrackerTool has been Changed!!" + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";

                            SendingMail(EmailId, from, subject, body);
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "Your Password Has Been Changed.";

                        }
                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = " Your Password Is Not Changed";
                        }

                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "Passwords Do Not Match!!";
                    }


                }

                else
                {
                    resultObject.Response.Status = "Empty";
                    resultObject.Response.Reason = "Enter All The details !!";
                }
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region Create Task
        [System.Web.Services.WebMethod]
        public static String CreateTask(String taskDesc, String expiryDate, String createdBy, String assignedTo, String status, String taskName, String startDate)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                DateTime createdDate = DateTime.Now;

                if (taskDesc != "" && createdDate != null && expiryDate != null && createdBy != "" && assignedTo != "" && status != "" && taskName != "" && startDate != null)
                {
                    try
                    {


                        int result = userBll.InsertTaskDetailsBLL(taskDesc, createdDate, expiryDate, createdBy, assignedTo, status, taskName, startDate);

                        if (result > 0)
                        {
                            List<Task> taskList = new List<Task>();
                            DataTable dt = userBll.ViewTaskDetailsByPMBLL(createdBy);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Task t = new Task();
                                t.taskId = Convert.ToInt32(dt.Rows[i]["TaskId"]);
                                t.taskDesc = dt.Rows[i]["Taskdesc"].ToString();
                                t.createdDate = Convert.ToString(dt.Rows[i]["Created_Date"]);
                                t.expiryDate = Convert.ToDateTime(dt.Rows[i]["Expiry_Date"]).ToString("dd/MMM/yyyy");
                                t.createdBy = dt.Rows[i]["CreatedBy"].ToString();
                                t.assignedTo = dt.Rows[i]["AssignedTo"].ToString();
                                t.status = dt.Rows[i]["Status"].ToString();
                                t.taskName = dt.Rows[i]["TaskName"].ToString();
                                t.startDate = Convert.ToDateTime(dt.Rows[i]["Start_Date"]).ToString("dd/MMM/yyyy");
                                taskList.Add(t);
                            }
                            resultObject.Response.taskObject = oSerializer.Serialize(taskList);
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "New Task Is Created!!";
                        }
                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = "Task is Not Created. Try again!!";
                        }

                    }

                    catch (Exception ex)
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "Error :  " + ex.Message;
                    }
                }
                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "Fill All The Details";
                }
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region Get User List
        [System.Web.Services.WebMethod]
        public static String GetUserList()
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();



            try
            {
                DataTable dt = userBll.GetDetailsForTMBLL();

                if (dt.Rows.Count > 0)
                {
                    List<User> userList = new List<User>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        User t = new User();
                        t.ntid = dt.Rows[i]["Ntid"].ToString();
                        t.firstName = dt.Rows[i]["FirstName"].ToString();
                        t.lastName = dt.Rows[i]["LastName"].ToString();
                        t.name = t.firstName + " " + t.lastName;
                        userList.Add(t);
                    }
                    resultObject.Response.userObject = oSerializer.Serialize(userList);
                    resultObject.Response.Status = "Success";
                    resultObject.Response.Reason = "Users are added!!";
                }
                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "Users are not added!!";
                }

            }

            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Error :  " + ex.Message;
            }

            return oSerializer.Serialize(resultObject);

        }
        #endregion


        #region Show Data On Page Load
        [System.Web.Services.WebMethod]
        public static String ShowData(String ntid)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); //Retrieving User List
            if (dt.Rows.Count > 0)
            {
                List<User> userList = new List<User>();
                User u = new User();
                u.ntid = dt.Rows[0]["Ntid"].ToString();
                u.firstName = dt.Rows[0]["FirstName"].ToString();
                u.lastName = dt.Rows[0]["LastName"].ToString();
                u.roleId = dt.Rows[0]["RoleId"].ToString();
                u.phoneNo = dt.Rows[0]["PhoneNo"].ToString();
                u.emailId = dt.Rows[0]["EmailId"].ToString();

                userList.Add(u);
                resultObject.Response.userObject = oSerializer.Serialize(userList);

                DataTable dt1 = new DataTable();
                List<Task> taskList = new List<Task>();
                if (u.roleId == "201")                                   //For PM
                {
                    dt1 = userBll.ViewTaskDetailsByPMBLL(u.ntid);        //Retrieving Task List Created By PM
                }

                else if (u.roleId == "200")                              //For TM
                {
                    dt1 = userBll.ViewTaskDetailsByTMBLL(u.ntid);        //Retrieving Task List Assigned By PM to TM
                }

                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        Task t = new Task();
                        t.taskId = Convert.ToInt32(dt1.Rows[i]["TaskId"]);
                        t.taskDesc = dt1.Rows[i]["Taskdesc"].ToString();
                        t.createdDate = Convert.ToString(dt1.Rows[i]["Created_Date"]);
                        t.expiryDate = Convert.ToDateTime(dt1.Rows[i]["Expiry_Date"]).ToString("dd/MMM/yyyy");
                        t.createdBy = dt1.Rows[i]["CreatedBy"].ToString();
                        t.assignedTo = dt1.Rows[i]["AssignedTo"].ToString();
                        t.status = dt1.Rows[i]["Status"].ToString();
                        t.taskName = dt1.Rows[i]["TaskName"].ToString();
                        t.startDate = Convert.ToDateTime(dt1.Rows[i]["Start_Date"]).ToString("dd/MMM/yyyy");
                        taskList.Add(t);
                    }
                    resultObject.Response.taskObject = oSerializer.Serialize(taskList);
                    resultObject.Response.Status = "Success";
                    resultObject.Response.Reason = "Data Retreived!!!";

                }
                else
                {
                    resultObject.Response.Status = "Success";
                    resultObject.Response.Reason = "No Task Found!!!";
                }

                DataTable dt2 = new DataTable();
                List<Leave> leaveList = new List<Leave>();
                if (u.roleId == "200")
                {
                    dt2 = userBll.ViewLeaveDetailsByTMBLL(u.ntid);         // Retrieving Leave details For TM
                }
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        Leave l = new Leave();
                        l.leaveId = Convert.ToInt32(dt1.Rows[i]["LeaveId"]);
                        l.leaveDesc = dt1.Rows[i]["Leavedesc"].ToString();
                        l.fromDate = Convert.ToString(dt1.Rows[i]["ToDate"]);
                        l.toDate = Convert.ToDateTime(dt1.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy");
                        l.appliedBy = dt1.Rows[i]["AppliedBy"].ToString();
                        l.leaveType = dt1.Rows[i]["LeaveType"].ToString();
                        l.status = dt1.Rows[i]["Status"].ToString();

                        leaveList.Add(l);
                    }
                    resultObject.Response.leaveObject = oSerializer.Serialize(leaveList);
                    resultObject.Response.Status = "Success";
                    resultObject.Response.Reason = "Data Retreived!!!";
                }
                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "No Data Retrieved!!";
                }
            }
            else
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "User Does'nt Exist!!!";
            }
            return oSerializer.Serialize(resultObject);

        }
        #endregion

        #region Update Task
        [System.Web.Services.WebMethod]
        public static String UpdateTask(String ntid, int taskId, String taskDesc, String expiryDate, String assignedTo, String taskName, String status)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                int result = 0;
                if (ntid != null)
                {
                    if (taskId != 0 && taskDesc != null && taskName != null && expiryDate != null && assignedTo != null && taskName != null)
                    {
                        result = userBll.UpdateTaskDetailsBLL(taskId, taskDesc, expiryDate, assignedTo, taskName); //Updating Task List
                    }

                    else if (status != null)
                    {
                        result = userBll.UpdateTaskStatusBLL(taskId, status); //Updating Task List Based on Status
                    }

                    if (result > 0)
                    {

                        DataTable dt1 = userBll.ViewUserDetailsByNtidBLL(ntid); //Retrieving User Details
                        User u = null;
                        if (dt1.Rows.Count > 0)
                        {
                            List<User> userList = new List<User>();
                            u = new User();
                            u.ntid = dt1.Rows[0]["Ntid"].ToString();
                            userList.Add(u);
                            resultObject.Response.userObject = oSerializer.Serialize(userList);
                        }

                        DataTable dt = null;

                        if (u.roleId == "201")                        //For PM
                        {
                            dt = userBll.ViewTaskDetailsByPMBLL(u.ntid);        //Retrieving Updated Task List Created By PM
                        }

                        else if (u.roleId == "200")                   //For TM
                        {

                            dt = userBll.ViewTaskDetailsByTMBLL(u.ntid);        //Retrieving Updated Task List Assigned By PM to TM
                        }
                        List<Task> taskList = new List<Task>();
                        if (dt.Rows.Count > 0)
                        {

                            Task t = new Task();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                t.taskId = Convert.ToInt32(dt.Rows[i]["TaskId"]);
                                t.taskDesc = dt.Rows[i]["Taskdesc"].ToString();
                                t.createdDate = Convert.ToString(dt.Rows[i]["Created_Date"]);
                                t.expiryDate = Convert.ToString(dt.Rows[i]["Expiry_Date"]);
                                t.createdBy = dt.Rows[i]["CreatedBy"].ToString();
                                t.assignedTo = dt.Rows[i]["AssignedTo"].ToString();
                                t.status = dt.Rows[i]["Status"].ToString();
                                t.taskName = dt.Rows[i]["TaskName"].ToString();
                                t.startDate = Convert.ToString(dt.Rows[i]["Start_Date"]);
                                taskList.Add(t);
                            }


                            resultObject.Response.taskObject = oSerializer.Serialize(taskList);
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "Task Details Has Been Retrieved!!";
                        }

                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = "Task Details Are Not Retrieved!!";
                        }
                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "Task Details Are Not Updated!!";
                    }
                }
                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "Enter All the Details!!";
                }

            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


    }
}

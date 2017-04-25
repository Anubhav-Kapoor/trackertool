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
        static UserDetailsBLL userBll = new UserDetailsBLL(); // Intializing UserDetailsBLL Instance
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region API Method -  Mail Method
        [System.Web.Services.WebMethod]
        public static Boolean SendingMail(String[] mailId, String from, String subject, String body, String emailPM)
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            Boolean value = false;
            try
            {
                if (mailId != null && (mailId.Length) > 0 && !(String.IsNullOrWhiteSpace(from)) && !(String.IsNullOrWhiteSpace(subject)) && !(String.IsNullOrWhiteSpace(body)) && !(String.IsNullOrWhiteSpace(emailPM)))
                {
                    MailMessage mailMessage = new MailMessage();
                    foreach (String m in mailId)
                    {
                        mailMessage.To.Add(m);
                    }

                    mailMessage.From = new MailAddress(from);
                    mailMessage.Subject = subject;
                    mailMessage.CC.Add(emailPM);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = body;
                    SmtpClient smtpClient = new SmtpClient("mailin.owenscorning.com");
                    smtpClient.Send(mailMessage);

                    resultObject.Response.Status = "Success";
                    resultObject.Response.Reason = "Welcome To Tracker Tool!!";
                    value = true;
                }
                else
                {

                    throw new EmailNotSentError("Could not sent the email");
                }
            }
            catch (EmailNotSentError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return value;
        }
        #endregion


        #region API Method - Sign-up
        [System.Web.Services.WebMethod]
        public static String CreateAccount(String ntid, String firstName, String lastName, String roleId, String phone, String email, String password)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            Boolean value = userBll.CheckUserExistDetailsBLL(ntid); // Checking User Exists Or Not
            try
            {
                if (!value)
                {
                    if (!(String.IsNullOrWhiteSpace(ntid)) && !(String.IsNullOrWhiteSpace(firstName)) && !(String.IsNullOrWhiteSpace(lastName)) && !(String.IsNullOrWhiteSpace(roleId)) && !(String.IsNullOrWhiteSpace(phone)) && !(String.IsNullOrWhiteSpace(email)) && !(String.IsNullOrWhiteSpace(password)))
                    {
                        int result = userBll.InsertUserDetailsBLL(ntid, firstName, lastName, roleId, phone, email, password); // Inserting User Details

                        if (result > 0)
                        {
                            String from = "bhawneet.singh@owenscorning.com";
                            String subject = "Welcome to Tracker Tool";
                            String body = "Dear " + firstName + " " + lastName + ",<br/>" + " <br />Thanks for registering with TrackerTool" + "<br />Please note your login details:" + "<br />NTID: " + ntid + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";
                            String[] to = { email };
                            String emailPM = null;
                            Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To User   
                            if (val)
                            {
                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "User Created !!";
                            }
                        }
                    }
                    else
                    {
                        throw new DataNotFoundError("Input data is invalid.");
                    }
                }

            }
            catch (InsertionError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (DataNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UserAlreadyExistsError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (EmailNotSentError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region  API Method - Get User Details
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


        #region  API Method -  Sign-in
        [System.Web.Services.WebMethod]
        public static String Login(String ntid, String password)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                if (!(String.IsNullOrWhiteSpace(ntid)) && !(String.IsNullOrWhiteSpace(password)))
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); // Retreiving User Details Through Ntid
                    if (dt.Rows.Count > 0)
                    {
                        String Password = dt.Rows[0]["Password"].ToString();
                        String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                        string hashedPassword = Security.HashSHA1(password + UserGuid);

                        if (Password == hashedPassword)
                        {
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "User authentication success";
                        }
                        else
                        {
                            throw new AuthenticationError("Username Or Password is incorrect!!");
                        }
                    }
                    else
                    {
                        throw new UserNotFoundError("User does'nt exist!!");
                    }
                }
                else
                {
                    throw new DataNotFoundError("Username or Password cannot be empty!!");
                }
            }
            catch (UserNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (AuthenticationError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (DataNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }

            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method -  ForgotPassword
        [System.Web.Services.WebMethod]
        public static String ForgotPassword(String ntid)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                if (!(String.IsNullOrWhiteSpace(ntid)))
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); // Retreiving User Details Through Ntid
                    if (dt.Rows.Count > 0)
                    {
                        String FirstName = dt.Rows[0]["FirstName"].ToString();
                        String LastName = dt.Rows[0]["LastName"].ToString();
                        String RoleId = dt.Rows[0]["RoleId"].ToString();
                        String PhoneNo = dt.Rows[0]["PhoneNo"].ToString();
                        String EmailId = dt.Rows[0]["EmailId"].ToString();
                        String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                        String Password = Membership.GeneratePassword(12, 9);
                        String hashedPwd = Security.HashSHA1(Password + UserGuid);
                        int result = userBll.UpdateUserDetailsBLL(ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, hashedPwd, UserGuid); // Updating User Details
                        if (result > 0)
                        {
                            String from = "bhawneet.singh@owenscorning.com";
                            String subject = "Tracker Tool - Password Reset";
                            String body = "Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your Temporary Password for TrackerTool is : " + Password + "<br />Please change your password after login. " + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";
                            String[] to = { EmailId };
                            String emailPM = null;
                            Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To User  
                            if (val)
                            {
                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "Your temporary password has been sent!!";
                            }
                        }
                    }
                }
                else
                {
                    throw new DataNotFoundError("Enter your correct ntid !!");
                }
            }
            catch (EmailNotSentError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UpdationError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (DataNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UserNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method -   ChangePassword
        [System.Web.Services.WebMethod]
        public static String ChangePassword(String ntid, String currentPassword, String newPassword)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                if (!(String.IsNullOrWhiteSpace(ntid)) && !(String.IsNullOrWhiteSpace(currentPassword)) && !(String.IsNullOrWhiteSpace(newPassword)))
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid);  // Retreiving User Details Through Ntid
                    if (dt.Rows.Count > 0)
                    {
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
                            int result = userBll.UpdateUserDetailsBLL(ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, hashedNewPassword, userGuid.ToString());  // Updating User Details

                            if (result > 0)
                            {
                                String from = "bhawneet.singh@owenscorning.com";
                                String subject = " Tracker Tool -  Password Changed ";
                                String body = " Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your Password for TrackerTool has been Changed!!" + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";
                                String[] to = { EmailId };
                                String emailPM = null;
                                Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To User
                                if (val)
                                {
                                    resultObject.Response.Status = "Success";
                                    resultObject.Response.Reason = "Your password has been changed!!";
                                }
                            }
                        }
                        else
                        {
                            throw new DataNotFoundError("Passwords do not match!!");
                        }
                    }
                }
                else
                {
                    throw new DataNotFoundError("Enter all the details!!");
                }
            }
            catch (EmailNotSentError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UpdationError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (DataNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UserNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method -  Create Task
        [System.Web.Services.WebMethod]
        public static String CreateTask(String taskDesc, String expiryDate, String createdBy, String assignedTo, String status, String taskName, String startDate)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                DateTime createdDate = DateTime.Now;

                if (!(String.IsNullOrWhiteSpace(taskDesc)) && createdDate != null && !(String.IsNullOrWhiteSpace(expiryDate)) && !(String.IsNullOrWhiteSpace(createdBy)) && !(String.IsNullOrWhiteSpace(assignedTo)) && !(String.IsNullOrWhiteSpace(status)) && !(String.IsNullOrWhiteSpace(taskName)) && !(String.IsNullOrWhiteSpace(startDate)))
                {
                    int result = userBll.InsertTaskDetailsBLL(taskDesc, createdDate, expiryDate, createdBy, assignedTo, status, taskName, startDate);  // Inserting Task Details 

                    if (result > 0)
                    {
                        List<Task> taskList = new List<Task>();                     // Initializing TaskList Instance
                        DataTable dt = userBll.ViewTaskDetailsByPMBLL(createdBy);  // Retreiving Task Details Through PM
                        Task t = null;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            t = new Task();                                   // Initializing Task Instance
                            t.taskId = Convert.ToInt32(dt.Rows[i]["TaskId"]);
                            t.taskDesc = dt.Rows[i]["Taskdesc"].ToString();
                            t.createdDate = Convert.ToString(dt.Rows[i]["Created_Date"]);
                            t.expiryDate = Convert.ToDateTime(dt.Rows[i]["Expiry_Date"]).ToString("dd/MMM/yyyy");
                            t.createdBy = dt.Rows[i]["CreatedBy"].ToString();
                            t.assignedTo = dt.Rows[i]["AssignedTo"].ToString();
                            t.status = dt.Rows[i]["Status"].ToString();
                            t.taskName = dt.Rows[i]["TaskName"].ToString();
                            t.startDate = Convert.ToDateTime(dt.Rows[i]["Start_Date"]).ToString("dd/MMM/yyyy");
                            taskList.Add(t);                                        // Adding Task Instance To TaskList
                        }
                        resultObject.Response.taskObject = oSerializer.Serialize(taskList); // Assigning TaskList to Response Class Instance[taskObject]

                        DataTable dt4 = userBll.ViewUserDetailsByNtidBLL(createdBy);
                        User u2 = null;
                        if (dt4.Rows.Count > 0)
                        {
                            u2 = new User();
                            u2.emailId = dt4.Rows[0]["EmailId"].ToString();
                        }
                        DataTable dt3 = userBll.ViewUserDetailsByNtidBLL(assignedTo);
                        User u1 = null;
                        if (dt3.Rows.Count > 0)
                        {
                            u1 = new User();
                            u1.emailId = dt3.Rows[0]["EmailId"].ToString();
                            u1.firstName = dt3.Rows[0]["FirstName"].ToString();
                            u1.lastName = dt3.Rows[0]["LastName"].ToString();
                        }

                        String from = "bhawneet.singh@owenscorning.com";
                        String subject = "Welcome to Tracker Tool";
                        String body = "Dear " + u1.firstName + " " + u1.lastName + ",<br/>" + "<br/> <table border ='3'> <tr> <th  align='left' > Task Desc </th> &nbsp;&nbsp;  <td> " + t.taskDesc + "</td></tr>  <tr> <th> Task Start Date </th> &nbsp;&nbsp;  <td> " + t.startDate + "</td></tr>  <tr> <th> Task End Date </th> &nbsp;&nbsp;  <td> " + t.expiryDate + "</td></tr> <tr> <th> Status </th> &nbsp;&nbsp;  <td> " + t.status + "</td></tr> </table> " + " <br/> <br/> " + "Thanks and Regards" + "<br />Tracker Tool Admin";
                        String emailPM = u2.emailId;
                        String[] to = { u1.emailId };
                        Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To User[Task Creation]
                        if (val)
                        {
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "New task is created!!";
                        }
                    }
                }
                else
                {
                    throw new DataNotFoundError("Input data is invalid!!");
                }
            }
            catch (InsertionError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (DataNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (EmailNotSentError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method - Get User List
        [System.Web.Services.WebMethod]
        public static String GetUserList()
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                DataTable dt = userBll.GetDetailsForTMBLL(); // Retreiving Details For TM

                if (dt.Rows.Count > 0)
                {
                    List<User> userList = new List<User>(); // Initializing UserList Instance
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        User t = new User();            // Initializing User Instance
                        t.ntid = dt.Rows[i]["Ntid"].ToString();
                        t.firstName = dt.Rows[i]["FirstName"].ToString();
                        t.lastName = dt.Rows[i]["LastName"].ToString();
                        t.name = t.firstName + " " + t.lastName;
                        userList.Add(t);
                    }
                    resultObject.Response.userObject = oSerializer.Serialize(userList);// Assigning UserList to Response Class Instance[userObject]
                    resultObject.Response.Status = "Success";
                    resultObject.Response.Reason = "Users are added in list!!";
                }

            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }

            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }

            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method -  Show Data On Page Load
        [System.Web.Services.WebMethod]
        public static String ShowData(String ntid)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); //Retrieving User List
            try
            {
                if (dt.Rows.Count > 0)
                {
                    User u = new User();                               // Initializing User Instance
                    u.ntid = dt.Rows[0]["Ntid"].ToString();
                    u.firstName = dt.Rows[0]["FirstName"].ToString();
                    u.lastName = dt.Rows[0]["LastName"].ToString();
                    u.roleId = dt.Rows[0]["RoleId"].ToString();
                    u.phoneNo = dt.Rows[0]["PhoneNo"].ToString();
                    u.emailId = dt.Rows[0]["EmailId"].ToString();
                    resultObject.Response.userObject = oSerializer.Serialize(u);// Assigning UserList to Response Class Instance[userObject]

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
                            Task t = new Task();                            // Initializing Task Instance
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
                        resultObject.Response.taskObject = oSerializer.Serialize(taskList); // Assigning TaskList to Response Class Instance[taskObject]
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Task data retreived";
                    }
                    else
                    {
                        resultObject.Response.Status = "Success";
                        //  resultObject.Response.Reason = "No Task Found!!!";
                    }
                    DataTable dt2 = new DataTable();
                    List<Leave> leaveList = new List<Leave>();                   // Initializing LeaveList Instance               
                    if (u.roleId == "200")
                    {
                        dt2 = userBll.ViewLeaveDetailsByTMBLL(u.ntid);          // Retrieving Leave details For TM
                    }
                    else if (u.roleId == "201")
                    {
                        dt2 = userBll.ViewLeaveDetailsByPMBLL();                // Retrieving Pending Leaves for PM
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            Leave l = new Leave();                               // Initializing Leave Instance 
                            l.leaveId = Convert.ToInt32(dt2.Rows[i]["LeaveId"]);
                            l.leaveDesc = dt2.Rows[i]["Leavedesc"].ToString();
                            l.fromDate = Convert.ToDateTime(dt2.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy");
                            l.toDate = Convert.ToDateTime(dt2.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy");
                            l.appliedBy = dt2.Rows[i]["AppliedBy"].ToString();
                            l.leaveType = dt2.Rows[i]["LeaveType"].ToString();
                            l.status = dt2.Rows[i]["Status"].ToString();
                            leaveList.Add(l);                                     // Adding Leave Instance to LeaveList
                        }
                        resultObject.Response.leaveObject = oSerializer.Serialize(leaveList); // Assigning LeaveList to Response Class Instance[leaveObject]
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Applied leave details are retreived!!!";
                    }
                    else
                    {
                        resultObject.Response.Status = "Success";
                        // resultObject.Response.Reason = "No Data Retrieved!!";
                    }
                }
            }
            catch (UserNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method -  Update Task
        [System.Web.Services.WebMethod]
        public static String UpdateTask(String ntid, int taskId, String taskDesc, String expiryDate, String assignedTo, String taskName, String status)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                int result = 0;
                if (!(String.IsNullOrWhiteSpace(ntid)))
                {
                    if (taskId != 0 && !(String.IsNullOrWhiteSpace(taskDesc)) && !(String.IsNullOrWhiteSpace(taskName)) && !(String.IsNullOrWhiteSpace(expiryDate)) && !(String.IsNullOrWhiteSpace(assignedTo)))
                    {
                        result = userBll.UpdateTaskDetailsBLL(taskId, taskDesc, expiryDate, assignedTo, taskName); //Updating Task List    
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Task data updated!!";

                    }

                    else if (!(String.IsNullOrWhiteSpace(status)))
                    {
                        result = userBll.UpdateTaskStatusBLL(taskId, status); //Updating Task List Based on Status
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Task " + status + " !!";
                    }

                    if (result > 0)
                    {
                        DataTable dt1 = userBll.ViewUserDetailsByNtidBLL(ntid); //Retrieving User Details
                        User u = null;
                        if (dt1.Rows.Count > 0)
                        {
                            u = new User();                                 // Initializing User Instance
                            u.ntid = dt1.Rows[0]["Ntid"].ToString();
                            u.roleId = dt1.Rows[0]["roleId"].ToString();

                            resultObject.Response.userObject = oSerializer.Serialize(u); // Assigning UserList to Response Class Instance[userObject]
                        }

                        DataTable dt = null;

                        if (u.roleId == "201")                                   //For PM
                        {
                            dt = userBll.ViewTaskDetailsByPMBLL(u.ntid);        //Retrieving Updated Task List Created By PM
                        }

                        else if (u.roleId == "200")                               //For TM
                        {
                            dt = userBll.ViewTaskDetailsByTMBLL(u.ntid);        //Retrieving Updated Task List Assigned By PM to TM
                        }
                        List<Task> taskList = new List<Task>();                // Initializing TaskList Instance
                        if (dt.Rows.Count > 0)
                        {
                            Task t = null;                                       // Declaring Task Instance 
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                t = new Task();                                 // Initializing Task Instance
                                t.taskId = Convert.ToInt32(dt.Rows[i]["TaskId"]);
                                t.taskDesc = dt.Rows[i]["Taskdesc"].ToString();
                                t.createdDate = Convert.ToString(dt.Rows[i]["Created_Date"]);
                                t.expiryDate = Convert.ToString(dt.Rows[i]["Expiry_Date"]);
                                t.createdBy = dt.Rows[i]["CreatedBy"].ToString();
                                t.assignedTo = dt.Rows[i]["AssignedTo"].ToString();
                                t.status = dt.Rows[i]["Status"].ToString();
                                t.taskName = dt.Rows[i]["TaskName"].ToString();
                                t.startDate = Convert.ToString(dt.Rows[i]["Start_Date"]);
                                taskList.Add(t);                                                 //Adding Task Instance To TaskList
                            }

                            resultObject.Response.taskObject = oSerializer.Serialize(taskList);   // Assigning TaskList to Response Class Instance[taskObject]

                            DataTable dt3 = userBll.ViewUserDetailsByNtidBLL(t.assignedTo);
                            User u1 = null;
                            if (dt3.Rows.Count > 0)
                            {
                                u1 = new User();
                                u1.emailId = dt3.Rows[0]["EmailId"].ToString();
                                u1.firstName = dt3.Rows[0]["FirstName"].ToString();
                                u1.lastName = dt3.Rows[0]["LastName"].ToString();
                            }

                            DataTable dt4 = userBll.ViewUserDetailsByNtidBLL(t.createdBy);
                            User u2 = null;
                            if (dt4.Rows.Count > 0)
                            {
                                u2 = new User();
                                u2.emailId = dt4.Rows[0]["EmailId"].ToString();
                            }

                            String from = "bhawneet.singh@owenscorning.com";
                            String subject = "Welcome to Tracker Tool";
                            String body = "Dear " + u1.firstName + " " + u1.lastName + ",<br/>" + "<br/> <table border ='3'> <tr> <th  align='left' > Task Desc </th> &nbsp;&nbsp;  <td> " + t.taskDesc + "</td></tr>  <tr> <th> Task Start Date </th> &nbsp;&nbsp;  <td> " + t.startDate + "</td></tr>  <tr> <th> Task End Date </th> &nbsp;&nbsp;  <td> " + t.expiryDate + "</td></tr>  <tr> <th align='left'> Status </th> &nbsp;&nbsp;  <td> " + status + "</td></tr> </table> " + " <br/> " + "Thanks and Regards" + "<br />Tracker Tool Admin";
                            String[] to = { u1.emailId, u2.emailId };
                            String emailPM = null;
                            Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To Both[Updated Task]

                        }
                        else
                        {
                            resultObject.Response.Status = "Success";
                            //resultObject.Response.Reason = "No task found!!";
                        }
                    }
                }
                else
                {
                    throw new DataNotFoundError("Enter all the details!!");
                }

            }
            catch (UpdationError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (EmailNotSentError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method - Update Leave Details
        [System.Web.Services.WebMethod]
        public static String UpdateLeaveDetails(String ntid, int leaveId, String status)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            int result = 0;
            try
            {
                if (!(string.IsNullOrWhiteSpace(ntid)))
                {
                    if (!(string.IsNullOrWhiteSpace(status)) && leaveId != 0)
                    {
                        result = userBll.UpdateLeaveStatusBLL(leaveId, status); //Updating Leave List Based on Status
                    }
                    if (result > 0)
                    {
                        
                        DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); //Retrieving User Details
                        User u = null;
                        if (dt.Rows.Count > 0)
                        {
                            u = new User();                                     // Initializing User Instance
                            u.ntid = dt.Rows[0]["Ntid"].ToString();
                            u.roleId = dt.Rows[0]["roleId"].ToString();
                            resultObject.Response.userObject = oSerializer.Serialize(u);
                        }
                        DataTable dt1 = new DataTable();
                        List<Leave> leaveList = new List<Leave>();              // Initializing LeaveList Instance
                        if (u.roleId == "200")
                        {
                            dt1 = userBll.ViewLeaveDetailsByTMBLL(u.ntid);         // Retrieving Leave details For TM
                        }
                        else if (u.roleId == "201")
                        {
                            dt1 = userBll.ViewLeaveDetailsByPMBLL();         // Retrieving Pending Leaves for PM
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            Leave l = null;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                l = new Leave();                          // Initializing Leave Instance
                                l.leaveId = Convert.ToInt32(dt1.Rows[i]["LeaveId"]);
                                l.leaveDesc = dt1.Rows[i]["Leavedesc"].ToString();
                                l.fromDate = Convert.ToDateTime(dt1.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy");
                                l.toDate = Convert.ToDateTime(dt1.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy");
                                l.appliedBy = dt1.Rows[i]["AppliedBy"].ToString();
                                l.leaveType = dt1.Rows[i]["LeaveType"].ToString();
                                l.status = dt1.Rows[i]["Status"].ToString();
                                leaveList.Add(l);                               // Adding Leave Instance
                            }
                            resultObject.Response.leaveObject = oSerializer.Serialize(leaveList);  // Assigning LeaveList to Response Class Instance[leaveObject]

                            DataTable dt5 = userBll.ViewUserDetailsByLeaveIdBLL(leaveId);  // Retreiving Details of TM
                            User u2 = null; // For TM
                            if (dt5.Rows.Count > 0)
                            {
                                u2 = new User();
                                u2.firstName = dt5.Rows[0]["FirstName"].ToString();
                                u2.lastName = dt5.Rows[0]["LastName"].ToString();
                                u2.emailId = dt5.Rows[0]["EmailId"].ToString();
                                u2.firstName = dt5.Rows[0]["FirstName"].ToString();
                                u2.lastName = dt5.Rows[0]["LastName"].ToString();
                            }

                            DataTable dt4 = userBll.ViewAllUserDetailsBLL(); // Retreiving Details of PM              
                            User u1 = null; // For PM
                            if (dt4.Rows.Count > 0)
                            {
                                u1 = new User();
                                u1.emailId = dt4.Rows[0]["EmailId"].ToString();
                                u1.roleId = dt4.Rows[0]["RoleId"].ToString();                              

                                String from = "bhawneet.singh@owenscorning.com";
                                String subject = "Welcome to Tracker Tool";
                                String body = "Dear " + u2.firstName + " " + u2.lastName + ",<br/>" + "<br/> <table border ='3'> <tr> <th  align='left' > Leave Id </th> &nbsp;&nbsp;  <td> " + l.leaveId + "</td></tr>  <tr> <th  align='left' > Leave Desc </th> &nbsp;&nbsp;  <td> " + l.leaveDesc + "</td></tr>  <tr> <th  align='left' > From Date </th> &nbsp;&nbsp;  <td> " + l.fromDate + "</td></tr>  <tr> <th align='left'> To Date </th> &nbsp;&nbsp;  <td> " + l.toDate + "</td></tr> <tr> <th align='left'> Leave Type </th> &nbsp;&nbsp;  <td> " + l.leaveType + "</td></tr> <tr> <th align='left'> Status </th> &nbsp;&nbsp;  <td> " + status + "</td></tr>  </table> " + " <br/> " + "Thanks and Regards" + "<br/> Tracker Tool Admin";
                                String[] to = { u2.emailId };
                                String emailPM = u1.emailId;
                                Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To Both[Leave Applied]
                                if (val)
                                {
                                    resultObject.Response.Status = "Success";
                                    resultObject.Response.Reason = "Email has been sent !!";
                                }
                            }                        
                        }
                        else
                        {
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "No pending leaves found!!";
                        }
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Leave " + status + "!!";
                    }
                }
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UserNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (UpdationError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
            }
            return oSerializer.Serialize(resultObject);
        }
        #endregion


        #region API Method - Apply Leave
        [System.Web.Services.WebMethod]
        public static String ApplyLeave(String fromDate, String toDate, String leaveType, String appliedBy, String leaveDesc, String status)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            int result = 0;

            try
            {
                DateTime createdDate = DateTime.Now;

                if (!(string.IsNullOrWhiteSpace(fromDate)) && createdDate != null && !(string.IsNullOrWhiteSpace(toDate)) && !(string.IsNullOrWhiteSpace(appliedBy)) && !(string.IsNullOrWhiteSpace(status)) && !(string.IsNullOrWhiteSpace(leaveDesc)))
                {
                    result = userBll.InsertLeaveDetailsBLL(leaveDesc, fromDate, toDate, appliedBy, leaveType, status);

                    if (result > 0)
                    {
                        List<Leave> leaveList = new List<Leave>();  //Initializing LeaveList Instance
                        DataTable dt = userBll.ViewLeaveDetailsByTMBLL(appliedBy);
                        Leave l = null;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            l = new Leave();                  //Initializing Leave Instance
                            l.leaveId = Convert.ToInt32(dt.Rows[i]["LeaveId"]);
                            l.leaveDesc = dt.Rows[i]["Leavedesc"].ToString();
                            l.fromDate = Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy");
                            l.toDate = Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy");
                            l.appliedBy = dt.Rows[i]["AppliedBy"].ToString();
                            l.leaveType = dt.Rows[i]["LeaveType"].ToString();
                            l.status = dt.Rows[i]["Status"].ToString();
                            leaveList.Add(l);                       // Adding Leave Instance To LeaveList
                        }
                        resultObject.Response.leaveObject = oSerializer.Serialize(leaveList);// Assigning LeaveList to Response Class Instance[leaveObject]

                        DataTable dt5 = userBll.ViewUserDetailsByNtidBLL(appliedBy);  // Retreiving Details of TM
                        User u2 = null; // For TM
                        if (dt5.Rows.Count > 0)
                        {
                            u2 = new User();
                            u2.firstName = dt5.Rows[0]["FirstName"].ToString();
                            u2.lastName = dt5.Rows[0]["LastName"].ToString();
                            u2.emailId = dt5.Rows[0]["EmailId"].ToString();
                        }

                        DataTable dt4 = userBll.ViewAllUserDetailsBLL(); // Retreiving Details of PM              
                        User u1 = null; // For PM
                        if (dt4.Rows.Count > 0)
                        {
                            u1 = new User();
                            u1.emailId = dt4.Rows[0]["EmailId"].ToString();
                            u1.roleId = dt4.Rows[0]["RoleId"].ToString();
                            u1.firstName = dt4.Rows[0]["FirstName"].ToString();
                            u1.lastName = dt4.Rows[0]["LastName"].ToString();

                            String from = u2.emailId;
                            String subject = "Welcome to Tracker Tool";
                            String body = "Dear " + u1.firstName + " " + u1.lastName + ",<br/>" + "<br/> <table border ='3'> <tr> <th  align='left' > Leave Id </th> &nbsp;&nbsp;  <td> " + l.leaveId + "</td></tr>  <tr> <th  align='left' > Leave Desc </th> &nbsp;&nbsp;  <td> " + l.leaveDesc + "</td></tr>  <tr> <th  align='left' > From Date </th> &nbsp;&nbsp;  <td> " + l.fromDate + "</td></tr>  <tr> <th align='left'> To Date </th> &nbsp;&nbsp;  <td> " + l.toDate + "</td></tr> <tr> <th align='left'> Leave Type </th> &nbsp;&nbsp;  <td> " + l.leaveType + "</td></tr> <tr> <th align='left'> Status </th> &nbsp;&nbsp;  <td> " + status + "</td></tr>  </table> " + " <br/> " + "Thanks and Regards" + "<br/>" + u2.firstName + " " + u2.lastName;
                            String[] to = { u1.emailId };
                            String emailPM = null;
                            Boolean val = SendingMail(to, from, subject, body, emailPM); // Sending Mail To PM[Leave Applied]
                            if (val)
                            {
                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "Email has been sent !!";
                            }

                        }
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Leave applied successfully!!!";
                    }
                }
                else
                {
                    throw new DataNotFoundError("Fill all the details");
                }
            }
            catch (InsertionError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (DataNotFoundError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
            }
            catch (RetreivalError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.msg;
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

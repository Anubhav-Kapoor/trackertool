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
        public static Boolean SendingMail(String mailId, String from, String subject, String body)
        {

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            Boolean value = false;
            try
            {
                if (!(String.IsNullOrWhiteSpace(mailId)) && !(String.IsNullOrWhiteSpace(from)) && !(String.IsNullOrWhiteSpace(subject)) && !(String.IsNullOrWhiteSpace(body)))
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
                    //resultObject.Response.Reason = "Email has been sent!!";
                    value = true;
                }

                else
                {
                    throw new EmailNotSentError(Convert.ToString(resultObject.Response.Reason));
                }
            }
            catch (EmailNotSentError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Could not sent the email";

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

                    if (ntid != "" && firstName != "" && lastName != "" && roleId != "" && phone != "" && email != "" && password != "")
                    {
                        int result = userBll.InsertUserDetailsBLL(ntid, firstName, lastName, roleId, phone, email, password); // Inserting User Details

                        if (result > 0)
                        {

                            String from = "bhawneet.singh@owenscorning.com";
                            String subject = "Welcome to Tracker Tool";
                            String body = "Dear " + firstName + " " + lastName + ",<br/>" + " <br />Thanks for registering with TrackerTool" + "<br />Please note your login details:" + "<br />NTID: " + ntid + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";

                           Boolean val = SendingMail(email, from, subject, body); // Sending Mail To User

                            if (val)
                            {
                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "You have successfully registered with Tracker Tool.";
                            }
                            else
                            {
                                throw new EmailNotSentError(Convert.ToString(resultObject.Response.Reason));
                            }

                        }
                        else
                        {
                            throw new InsertionError(Convert.ToString(resultObject.Response.Reason));
                        }
                    }
                    else
                    {
                        throw new DataNotFoundError(Convert.ToString(resultObject.Response.Reason));
                    }
                }

                else
                {
                    throw new UserAlreadyExistsError(Convert.ToString(resultObject.Response.Reason));
                }

            }
            catch (InsertionError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Your Account Is Not Created!!";
            }
            catch (DataNotFoundError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Input data is invalid.";
            }
            catch (UserAlreadyExistsError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "User exists already. Try with different ntid!!";
            }
            catch (EmailNotSentError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Could not sent the email!!";
            }
            catch (CustomExceptionsError ex)
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
                throw new CustomExceptionsError(Convert.ToString(resultObject.Response.Reason));
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
                            resultObject.Response.Reason = "User Authentication Success";
                        }
                        else
                        {
                            throw new AuthenticationError(Convert.ToString(resultObject.Response.Reason));
                        }

                    }
                    else
                    {
                        throw new UserNotFoundError(Convert.ToString(resultObject.Response.Reason));
                    }
                }
                else
                {
                    throw new DataNotFoundError(Convert.ToString(resultObject.Response.Reason));
                }
            }
            catch (UserNotFoundError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "User does not exist!!!";

            }
            catch (AuthenticationError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Username Or Password is Incorrect!!!";
            }
            catch (DataNotFoundError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Username or Password cannot be empty!!!";
            }
            catch (CustomExceptionsError ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
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
                if (ntid != "")
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); // Retreiving User Details Through Ntid
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

                        Boolean val = SendingMail(EmailId, from, subject, body); // Sending Mail To User

                        if (val)
                        {
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "You are successfully registered.";
                        }
                        else
                        {
                            throw new EmailNotSentError(Convert.ToString(resultObject.Response.Reason));
                        }
                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = " User Details Are Not Updated!!";
                        throw new UpdationError(Convert.ToString(resultObject.Response.Reason));
                    }

                }
                else
                {
                    resultObject.Response.Status = "Failure";
                    resultObject.Response.Reason = "Enter Your Correct Ntid !!";
                    throw new DataNotFoundError(Convert.ToString(resultObject.Response.Reason));
                }
            }
            catch (EmailNotSentError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Could not sent the email!!!";
                throw new CustomExceptionsError(Convert.ToString(resultObject.Response.Reason));
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
                throw new CustomExceptionsError(Convert.ToString(resultObject.Response.Reason));
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
                if (ntid != "" && currentPassword != "" && newPassword != "")
                {
                    DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid);  // Retreiving User Details Through Ntid

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
                            String subject = "Tracker Tool -  Password Changed";
                            String body = "Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your Password for TrackerTool has been Changed!!" + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";

                            Boolean val = SendingMail(EmailId, from, subject, body); // Sending Mail To User

                            if (val)
                            {
                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "You are successfully registered.";
                            }
                            else
                            {
                                throw new EmailNotSentError(Convert.ToString(resultObject.Response.Reason));
                            }
                        }
                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = " Your Password Is Not Changed";
                            throw new UpdationError(Convert.ToString(resultObject.Response.Reason));
                        }
                    }
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "Passwords Do Not Match!!";
                        throw new DataNotFoundError(Convert.ToString(resultObject.Response.Reason));
                    }
                }
                else
                {
                    resultObject.Response.Status = "Empty";
                    resultObject.Response.Reason = "Enter All The details!!";
                    throw new DataNotFoundError(Convert.ToString(resultObject.Response.Reason));
                }
            }
            catch (EmailNotSentError)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = "Could not sent the email!!";
                throw new CustomExceptionsError(Convert.ToString(resultObject.Response.Reason));
            }
            catch (Exception ex)
            {
                resultObject.Response.Status = "Failure";
                resultObject.Response.Reason = ex.Message;
                throw new CustomExceptionsError(Convert.ToString(resultObject.Response.Reason));
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

                if (taskDesc != "" && createdDate != null && expiryDate != null && createdBy != "" && assignedTo != "" && status != "" && taskName != "" && startDate != null)
                {
                    try
                    {


                        int result = userBll.InsertTaskDetailsBLL(taskDesc, createdDate, expiryDate, createdBy, assignedTo, status, taskName, startDate);  // Inserting Task Details 

                        if (result > 0)
                        {
                            List<Task> taskList = new List<Task>();                     // Initializing TaskList Instance
                            DataTable dt = userBll.ViewTaskDetailsByPMBLL(createdBy);  // Retreiving Task Details Through PM

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Task t = new Task();                                   // Initializing Task Instance
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
                    resultObject.Response.Reason = "Invalid Input Data!!";
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


        #region API Method -  Show Data On Page Load
        [System.Web.Services.WebMethod]
        public static String ShowData(String ntid)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            DataTable dt = userBll.ViewUserDetailsByNtidBLL(ntid); //Retrieving User List
            if (dt.Rows.Count > 0)
            {

                User u = new User();                               // Initializing User Instance
                u.ntid = dt.Rows[0]["Ntid"].ToString();
                u.firstName = dt.Rows[0]["FirstName"].ToString();
                u.lastName = dt.Rows[0]["LastName"].ToString();
                u.roleId = dt.Rows[0]["RoleId"].ToString();
                u.phoneNo = dt.Rows[0]["PhoneNo"].ToString();
                u.emailId = dt.Rows[0]["EmailId"].ToString();

                resultObject.Response.userObject = oSerializer.Serialize(u); // Assigning UserList to Response Class Instance[userObject]

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
                    //resultObject.Response.Reason = "Data Retreived!!!";

                }
                else
                {
                    resultObject.Response.Status = "Success";
                    //resultObject.Response.Reason = "No Task Found!!!";
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

                        leaveList.Add(l);
                    }
                    resultObject.Response.leaveObject = oSerializer.Serialize(leaveList); // Assigning LeaveList to Response Class Instance[leaveObject]
                    resultObject.Response.Status = "Success";
                    //resultObject.Response.Reason = "Data Retreived!!!";
                }
                else
                {
                    resultObject.Response.Status = "Success";
                    //resultObject.Response.Reason = "No Data Retrieved!!";
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
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Task "+status+"!!";

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

                            Task t = null;                              // Declaring Task Instance 
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                t = new Task(); // Initializing Task Instance
                                t.taskId = Convert.ToInt32(dt.Rows[i]["TaskId"]);
                                t.taskDesc = dt.Rows[i]["Taskdesc"].ToString();
                                t.createdDate = Convert.ToString(dt.Rows[i]["Created_Date"]);
                                t.expiryDate = Convert.ToString(dt.Rows[i]["Expiry_Date"]);
                                t.createdBy = dt.Rows[i]["CreatedBy"].ToString();
                                t.assignedTo = dt.Rows[i]["AssignedTo"].ToString();
                                t.status = dt.Rows[i]["Status"].ToString();
                                t.taskName = dt.Rows[i]["TaskName"].ToString();
                                t.startDate = Convert.ToString(dt.Rows[i]["Start_Date"]);
                                taskList.Add(t);                                   //Adding Task Instance To TaskList
                            }


                            resultObject.Response.taskObject = oSerializer.Serialize(taskList);   // Assigning TaskList to Response Class Instance[taskObject]
                            resultObject.Response.Status = "Success";
                            //resultObject.Response.Reason = "Task Details Has Been Retrieved!!";
                        }

                        else
                        {
                            resultObject.Response.Status = "Success";
                            //resultObject.Response.Reason = "Task Details Are Not Retrieved!!";
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


        #region API Method - Update Leave Details
        [System.Web.Services.WebMethod]
        public static String UpdateLeaveDetails(String ntid, int leaveId, String status)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            int result = 0;
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
                    else
                    {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "No Data Retrieved!!";
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
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            Leave l = new Leave();                          // Initializing Leave Instance
                            l.leaveId = Convert.ToInt32(dt1.Rows[i]["LeaveId"]);
                            l.leaveDesc = dt1.Rows[i]["Leavedesc"].ToString();
                            l.fromDate = Convert.ToDateTime(dt1.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy");
                            l.toDate = Convert.ToDateTime(dt1.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy");
                            l.appliedBy = dt1.Rows[i]["AppliedBy"].ToString();
                            l.leaveType = dt1.Rows[i]["LeaveType"].ToString();
                            l.status = dt1.Rows[i]["Status"].ToString();

                            leaveList.Add(l);                               // Addiing Leave Instance
                        }
                        resultObject.Response.leaveObject = oSerializer.Serialize(leaveList);  // Assigning LeaveList to Response Class Instance[leaveObject]
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "Data Retreived!!!";
                    }
                    else
                    {
                        resultObject.Response.Status = "Success";
                        resultObject.Response.Reason = "No Pending Leaves Found!!";
                    }
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
                resultObject.Response.Reason = "User Does'nt Exist!!";
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

                if (fromDate != "" && createdDate != null && toDate != "" && appliedBy != "" && status != "" && leaveDesc != "")
                {
                    try
                    {

                        result = userBll.InsertLeaveDetailsBLL(leaveDesc, fromDate, toDate, appliedBy, leaveType, status);

                        if (result > 0)
                        {
                            List<Leave> leaveList = new List<Leave>();  //Initializing LeaveList Instance
                            DataTable dt = userBll.ViewLeaveDetailsByTMBLL(appliedBy);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Leave l = new Leave();                  //Initializing Leave Instance
                                l.leaveId = Convert.ToInt32(dt.Rows[i]["LeaveId"]);
                                l.leaveDesc = dt.Rows[i]["Leavedesc"].ToString();
                                l.fromDate = Convert.ToDateTime(dt.Rows[i]["FromDate"]).ToString("dd/MMM/yyyy");
                                l.toDate = Convert.ToDateTime(dt.Rows[i]["ToDate"]).ToString("dd/MMM/yyyy");
                                l.appliedBy = dt.Rows[i]["AppliedBy"].ToString();
                                l.leaveType = dt.Rows[i]["LeaveType"].ToString();
                                l.status = dt.Rows[i]["Status"].ToString();


                                leaveList.Add(l);                       // Adding Leave Instance To LeaveList
                            }
                            resultObject.Response.leaveObject = oSerializer.Serialize(leaveList);// Assigning TaskList to Response Class Instance[taskObject]
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "Leave Applied Successfully!!!";
                        }
                        else
                        {
                            resultObject.Response.Status = "Failure";
                            resultObject.Response.Reason = "Unable to apply Leave. Please Try again!!!";
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

    }

}

﻿using ProjectTracker.BLL;
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

                            try
                            {
                                MailMessage mailMessage = new MailMessage();
                                mailMessage.To.Add(email);
                                mailMessage.From = new MailAddress("bhawneet.singh@owenscorning.com");
                                mailMessage.Subject = "Welcome to Tracker Tool";
                                mailMessage.IsBodyHtml = true;
                                mailMessage.Body = "Dear " + firstName + " " + lastName + ",<br/>" + " <br />Thanks for registering with TrackerTool" + "<br />Please note your login details:" + "<br />NTID: " + ntid + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";
                                SmtpClient smtpClient = new SmtpClient("mailin.owenscorning.com");
                                smtpClient.Send(mailMessage);

                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "Email has been sent!!";

                            }


                            catch (Exception ex)
                            {
                                resultObject.Response.Status = "Fail";
                                resultObject.Response.Reason = "Could not sent the email" + ex.Message;
                            }
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
                    if (dt.Rows.Count > 0)
                    {

                        String Ntid = dt.Rows[0]["Ntid"].ToString();
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
                    else {
                        resultObject.Response.Status = "Failure";
                        resultObject.Response.Reason = "User does not exist!!!";
                    }
                }
                else
                {
                    resultObject.Response.Status = "Fail";
                    resultObject.Response.Reason = "Username or Password cannot be empty!!!";
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
                    DataTable dt = userBll.ViewUserDetailsBLL(ntid);
                    String FirstName = dt.Rows[0]["FirstName"].ToString();
                    String LastName = dt.Rows[0]["LastName"].ToString();
                    String RoleId = dt.Rows[0]["RoleId"].ToString();
                    String PhoneNo = dt.Rows[0]["PhoneNo"].ToString();
                    String EmailId = dt.Rows[0]["EmailId"].ToString();
                    String UserGuid = dt.Rows[0]["UserGuid"].ToString();
                    String Password = Membership.GeneratePassword(12, 9);
                    int result = userBll.UpdateUserDetailsBLL(ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password, UserGuid);
                    if (result > 0)
                    {

                        try
                        {
                            MailMessage mailMessage = new MailMessage();
                            mailMessage.To.Add(EmailId);
                            mailMessage.From = new MailAddress("bhawneet.singh@owenscorning.com");
                            mailMessage.Subject = "Tracker Tool - Password Reset";
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Body = "Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your Temporary Password for TrackerTool is : " + Password + "<br />Please change your password after login. " + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";
                            SmtpClient smtpClient = new SmtpClient("mailin.owenscorning.com");
                            smtpClient.Send(mailMessage);

                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "Email has been sent!!";

                        }


                        catch (Exception ex)
                        {
                            resultObject.Response.Status = "Fail";
                            resultObject.Response.Reason = "Could not sent the email" + ex.Message;
                        }

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


        #region  ChangePassword
        [System.Web.Services.WebMethod]
        public static String ChangePassword(String ntid, String currentPassword, String newPassword)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
            try
            {
                if (ntid != "")
                {
                    DataTable dt = userBll.ViewUserDetailsBLL(ntid);

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

                            try
                            {
                                MailMessage mailMessage = new MailMessage();
                                mailMessage.To.Add(EmailId);
                                mailMessage.From = new MailAddress("bhawneet.singh@owenscorning.com");
                                mailMessage.Subject = "Tracker Tool - Password Changed";
                                mailMessage.IsBodyHtml = true;
                                mailMessage.Body = "Dear " + FirstName + " " + LastName + ",<br /><br />" + "Your New Password for TrackerTool is : " + newPassword + "<br />Please change your password after login. " + "<br /><br />Thanks and Regards" + "<br />Tracker Tool Admin";
                                SmtpClient smtpClient = new SmtpClient("mailin.owenscorning.com");
                                smtpClient.Send(mailMessage);

                                resultObject.Response.Status = "Success";
                                resultObject.Response.Reason = "Email has been sent!!";

                            }


                            catch (Exception ex)
                            {
                                resultObject.Response.Status = "Fail";
                                resultObject.Response.Reason = "Could not sent the email" + ex.Message;
                            }

                        }

                    }
                    else
                    {
                        resultObject.Response.Status = "Fail";
                        resultObject.Response.Reason = "Passwords Do Not Match!!";
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

               
                        

                if (taskDesc != "" && createdDate != null && expiryDate != null && createdBy != "" && assignedTo != "" && status != "" && taskName!="" && startDate!=null)
                {
                    try
                    {
                        
                        
                        int result = userBll.InsertTaskDetailsBLL(taskDesc, createdDate, expiryDate, createdBy, assignedTo, status, taskName, startDate);

                        if (result > 0)
                        {
                            List<Task> taskList = new List<Task>();
                            DataTable dt = userBll.ViewTaskDetailsBLL(createdBy);
                            
                            for (int i = 0; i < dt.Rows.Count;i++ )
                            {
                                Task t = new Task();
                                t.taskId = Convert.ToInt32(dt.Rows[i]["TaskId"]);
                                t.taskDesc = dt.Rows[i]["Taskdesc"].ToString();
                                t.createdDate = Convert.ToDateTime(dt.Rows[i]["Created_Date"]);
                                t.expiryDate = Convert.ToDateTime(dt.Rows[i]["Expiry_Date"]).ToString("dd/MMM/yyyy");
                                t.createdBy = dt.Rows[i]["CreatedBy"].ToString();
                                t.assignedTo = dt.Rows[i]["AssignedTo"].ToString();
                                t.status = dt.Rows[i]["Status"].ToString();
                                t.taskname = dt.Rows[i]["TaskName"].ToString();
                                t.startDate = Convert.ToDateTime(dt.Rows[i]["Start_Date"]).ToString("dd/MMM/yyyy");
                                taskList.Add(t);
                            }
                            resultObject.Response.taskObject = oSerializer.Serialize(taskList);
                            resultObject.Response.Status = "Success";
                            resultObject.Response.Reason = "New Task Is Created!!";
                        }
                        else
                        {
                            resultObject.Response.Status = "Fail";
                            resultObject.Response.Reason = "Task is Not Created. Try again!!";
                        }
                       
                    }

                    catch (Exception ex)
                    {
                        resultObject.Response.Status = "Fail";
                        resultObject.Response.Reason = "Error :  " + ex.Message;
                    }
                }
                else
                {
                    resultObject.Response.Status = "Fail";
                    resultObject.Response.Reason = "Fill All The Details";
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

        #region Get User List
        [System.Web.Services.WebMethod]
        public static String GetUserList()
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            RootObjectResponse resultObject = new RootObjectResponse();
            resultObject.Response = new Response();
           

               
                    try
                    {
                        DataTable dt = userBll.ViewIdBLL();

                     if (dt.Rows.Count>0)
                        {
                          List<User> userList = new List<User>();

                          for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                User t = new User();
                                t.ntid = dt.Rows[i]["Ntid"].ToString();
                                t.firstName = dt.Rows[i]["FirstName"].ToString();
                                t.lastName = dt.Rows[i]["LastName"].ToString();
                                t.name = t.firstName +" "+ t.lastName;
                                userList.Add(t);
                            }
                          resultObject.Response.userObject = oSerializer.Serialize(userList);
                          resultObject.Response.Status = "Success";
                          resultObject.Response.Reason = "Users are added!!";
                        }
                        else
                        {
                            resultObject.Response.Status = "Fail";
                            resultObject.Response.Reason = "";
                        }

                    }

                    catch (Exception ex)
                    {
                        resultObject.Response.Status = "Fail";
                        resultObject.Response.Reason = "Error :  " + ex.Message;
                    }              
                       
            return oSerializer.Serialize(resultObject);
            
        }
        #endregion


    }
}

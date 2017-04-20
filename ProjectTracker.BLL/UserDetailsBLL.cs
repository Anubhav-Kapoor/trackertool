using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ProjectTracker.DEL;

namespace ProjectTracker.BLL
{
    public class UserDetailsBLL
    {

        UserDetailsDEL userDEL = new UserDetailsDEL(); //Initializing UserDetailsDEL Instance
        int result = 0;

        #region BLL Method -[Invoking DEL Method] Insert User Details
        public int InsertUserDetailsBLL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
        {
            result = userDEL.InsertUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password);
            if (result > 0)
            {
                Console.WriteLine("User Details are added");
            }
            else
            {
                throw new Task_and_Leave_Tracker.InsertionError("User Details are not added");
            }

            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] View All Users Through Ntid
        public DataTable ViewUserDetailsByNtidBLL(String Ntid)
        {
            DataTable dt = null;
            dt = userDEL.ViewUserDetailsByNtidDEL(Ntid);

            if (dt.Rows.Count > 0)
            {
                Console.WriteLine("User Details are retreived");
            }
            else
            {
                throw new Task_and_Leave_Tracker.RetreivalError("User Details are not added");
            }

            return dt;

        }
        #endregion


        #region BLL Method -[Invoking DEL Method] View Users Through RoleId
        public DataTable GetDetailsForTMBLL()
        {
            DataTable dt = null;
            
                dt = userDEL.GetDetailsForTMDEL();

                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("User Details are retreived");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.RetreivalError("User Details are not added");
                }
            
            return dt;

        }
        #endregion


        //#region BLL Method -[Invoking DEL Method] Delete All Users
        //public int DeleteUserDetailsBLL(String Ntid)
        //{
            
        //        result = userDEL.DeleteUserDetailsDEL(Ntid);

        //        if (result > 0)
        //        {
        //            Console.WriteLine("User Details are deleted");
        //        }
        //        else
        //        {
        //            throw new Task_and_Leave_Tracker.DeletionError("User Details are not deleted");
        //        }


        //    return result;
        //}
        //#endregion


        #region BLL Method -[Invoking DEL Method] Update All User Details
        public int UpdateUserDetailsBLL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password, String userGuid)
        {
           
                result = userDEL.UpdateUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password, userGuid);

                if (result > 0)
                {
                    Console.WriteLine("User Details are updated");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.UpdationError("User Details are not updated");
                }

            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] Check User Exist or Not
        public Boolean CheckUserExistDetailsBLL(String Ntid)
        {
            Boolean value = false;
           
                value = userDEL.CheckUserExistDetailsDEL(Ntid);

                if (value)
                {
                    Console.WriteLine("User exists !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.UserNotFoundError("User does'nt exist !! ");
                }

            return value;
        }
        #endregion
         

        #region BLL Method -[Invoking DEL Method] Insert Task Details
        public int InsertTaskDetailsBLL(String taskDesc, DateTime createdDate, String expiryDate, String createdBy, String assignedTo, String status, String taskName, String startDate)
        {
           
                result = userDEL.InsertTaskDetailsDEL(taskDesc, createdDate, expiryDate, createdBy, assignedTo, status, taskName, startDate);
                if (result > 0)
                {
                    Console.WriteLine("Task is created !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.InsertionError("No task is created !!");
                }
            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] View All Tasks Created By PM
        public DataTable ViewTaskDetailsByTMBLL(String assignedTo)
        {
            DataTable dt = null;
           
                dt = userDEL.ViewTaskDetailsByTMDEL(assignedTo);
                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("Task Details created by PM are retreived !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.RetreivalError("Task Details created by PM are not retreived !!");
                }
            return dt;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] View All Tasks Assigned To TM
        public DataTable ViewTaskDetailsByPMBLL(String createdBy)
        {
            DataTable dt = null;
            
                dt = userDEL.ViewTaskDetailsByPMDEL(createdBy);

                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("Task Details assigned to TM are retreived !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.RetreivalError("Task Details assigned to TM are not retreived !!");
                }
            return dt;
        }
        #endregion


        #region  BLL Method -[Invoking DEL Method] View All Tasks Through TaskId
        public DataTable ViewByIdBLL(int taskId)
        {

            DataTable dt = null;
           
                dt = userDEL.ViewAllTaskDetailsByIdDEL(taskId);
                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("Task Details are retreived !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.RetreivalError("Task Details are not retreived !!");
                }
            return dt;
        }
        #endregion


        //#region BLL Method -[Invoking DEL Method] Delete All Tasks
        //public int DeleteTaskDetailsBLL(int TaskId)
        //{
            
        //        result = userDEL.DeleteTaskDetailsDEL(TaskId);

        //        if (result > 0)
        //        {
        //            Console.WriteLine("Assigned Tasks are deleted !!");
        //        }
        //        else
        //        {
        //            throw new Task_and_Leave_Tracker.d("Assigned Tasks are not deleted !!");
        //        }
        //    return result;
        //}
        //#endregion


        #region BLL Method -[Invoking DEL Method] Update Task Details
        public int UpdateTaskDetailsBLL(int taskId, String taskDesc, String expiryDate, String assignedTo, String taskName)
        {
            
                result = userDEL.UpdateTaskDetailsDEL(taskId, taskDesc, expiryDate, assignedTo, taskName);

                if (result > 0)
                {
                    Console.WriteLine("Task details are updated !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.UpdationError("Task details are not updated !!");
                }
            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] Update Task Details Based on Status
        public int UpdateTaskStatusBLL(int taskId, String status)
        {
            
                result = userDEL.UpdateTaskStatusDEL(taskId, status);

                if (result > 0)
                {
                    Console.WriteLine("Task details based on status are updated !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.UpdationError("Task details are not updated !!");
                }
            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] Insert Leave Details
        public int InsertLeaveDetailsBLL(String leaveDesc, String fromDate, String toDate, String appliedBy, String leaveType, String status)
        {
              result = userDEL.InsertLeaveDetailsDEL(leaveDesc, fromDate, toDate, appliedBy, leaveType, status);

              if (result > 0)
              {
                  Console.WriteLine("Leaves Are Applied Successfully !!");
              }
              else
              {
                  throw new Task_and_Leave_Tracker.InsertionError("Leaves Are Not Applied !!");
              }
            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] Update Leave Details Based on Status
        public int UpdateLeaveStatusBLL(int leaveId, String status)
        {
            
                result = userDEL.UpdateLeaveStatusDEL(leaveId, status);
                if (result > 0)
                {
                    Console.WriteLine(" Applied Leaves Are Updated !!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.UpdationError(" Applied Leaves Are Not Updated !!");
                }
            return result;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] View Leaves Applied By TM
        public DataTable ViewLeaveDetailsByTMBLL(String appliedBy)
        {
            DataTable dt = null;
           
                dt = userDEL.ViewLeaveDetailsByTMDEL(appliedBy);

                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("Leave Details for TM are retreived");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.RetreivalError("Leave Details for TM are not retreived");
                }
           
            return dt;
        }
        #endregion


        #region BLL Method -[Invoking DEL Method] View Pending Leaves For PM
        public DataTable ViewLeaveDetailsByPMBLL()
        {
            DataTable dt = null;
           
                dt = userDEL.ViewLeaveDetailsByPMDEL();

                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("Pending Leaves are retreived!!");
                }
                else
                {
                    throw new Task_and_Leave_Tracker.RetreivalError("Pending Leaves are not retreived!!");
                }
            return dt;
        }
        #endregion

    }
}

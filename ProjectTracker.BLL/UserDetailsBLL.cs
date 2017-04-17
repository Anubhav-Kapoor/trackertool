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

        UserDetailsDEL userDEL = new UserDetailsDEL();
        int result = 0;

        //InsertFunction[User Details]
        public int InsertUserDetailsBLL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
        {
            try
            {
                result = userDEL.InsertUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }

        //ViewFunction[User Details]
        public DataTable ViewUserDetailsByNtidBLL(String Ntid)
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewUserDetailsByNtidDEL(Ntid);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;

        }
        //ViewIdFunction[User Details]
        public DataTable GetDetailsForTMBLL()
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.GetDetailsForTMDEL();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;

        }
        //DeleteFunction[User Details]
        public int DeleteUserDetailsBLL(String Ntid)
        {
            try
            {
                result = userDEL.DeleteUserDetailsDEL(Ntid);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;


        }
        //UpdateFunction[User Details]
        public int UpdateUserDetailsBLL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password, String userGuid)
        {
            try
            {

                result = userDEL.UpdateUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password, userGuid);

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;


        }
        //CheckUserExistFunction[User Details]
        public Boolean CheckUserExistDetailsBLL(String Ntid)
        {
            Boolean value = false;
            try
            {
                value = userDEL.CheckUserExistDetailsDEL(Ntid);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }

            return value;
        }

        //InsertFunction[Task Details]
        public int InsertTaskDetailsBLL(String taskDesc, DateTime createdDate, String expiryDate, String createdBy, String assignedTo, String status, String taskName, String startDate)
        {
            try
            {
                result = userDEL.InsertTaskDetailsDEL(taskDesc, createdDate, expiryDate, createdBy, assignedTo, status, taskName, startDate);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }


        //View Method For TM Function[Task Details]
        public DataTable ViewTaskDetailsByTMBLL(String assignedTo)
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewTaskDetailsByTMDEL(assignedTo);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;
        }


        //View Method For PM Function[Task Details]
        public DataTable ViewTaskDetailsByPMBLL(String createdBy)
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewTaskDetailsByPMDEL(createdBy);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;
        }
        // View By Id[Task Details]
        public DataTable ViewByIdBLL(int taskId)
        {

            DataTable dt = null;
            try
            {
                dt = userDEL.ViewAllTaskDetailsByIdDEL(taskId);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;
        }

        //DeleteFunction[Task Details]
        public int DeleteTaskDetailsBLL(int TaskId)
        {
            try
            {
                result = userDEL.DeleteTaskDetailsDEL(TaskId);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }

        //UpdateFunction[Task Details]
        public int UpdateTaskDetailsBLL(int taskId, String taskDesc, String expiryDate, String assignedTo, String taskName)
        {
            try
            {

                result = userDEL.UpdateTaskDetailsDEL(taskId, taskDesc, expiryDate, assignedTo, taskName);

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }

        //UpdateFunction[Task Details]
        public int UpdateTaskStatusBLL(int taskId, String status)
        {
            try
            {

                result = userDEL.UpdateTaskStatusDEL(taskId, status);

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }

        //Insert Function[Leave Details]
        public int InsertLeaveDetailsBLL(String leaveDesc, String fromDate, String toDate, String appliedBy, String leaveType, String status)
        {
            try
            {
                result = userDEL.InsertLeaveDetailsDEL(leaveDesc, fromDate, toDate, appliedBy, leaveType, status);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }


        //Update Status [Leave Details]
        public int UpdateLeaveStatusBLL(int leaveId, String status)
        {
            try
            {

                result = userDEL.UpdateLeaveStatusDEL(leaveId, status);

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }


            return result;
        }

         // View Leaves Applied By TM [Leave Details]
        public DataTable ViewLeaveDetailsByTMBLL(String appliedBy)
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewLeaveDetailsByTMDEL(appliedBy);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;
        }

          // View Leaves For PM [Leave Details]
        public DataTable ViewLeaveDetailsByPMBLL()
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewLeaveDetailsByPMDEL();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return dt;
        }

    }
}

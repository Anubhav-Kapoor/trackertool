using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Task_and_Leave_Tracker;

namespace ProjectTracker.DEL
{
    public class UserDetailsDEL
    {


        //Initializing Connection String
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\TLT.mdf;Integrated Security=True");

        #region DEL Method - Insert User Details
        public int InsertUserDetailsDEL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
        {
            Guid userGuid = System.Guid.NewGuid();

            // Hash the password together with our unique userGuid
            string hashedPassword = Security.HashSHA1(Password + userGuid.ToString());

            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@Ntid", Ntid);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@RoleId", RoleId);
                cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                cmd.Parameters.AddWithValue("@EmailId", EmailId);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@UserGuid", userGuid);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.InsertionError("User creation failed !!");
            }

            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion


        #region DEL Method - View All Users Through Ntid
        public DataTable ViewUserDetailsByNtidDEL(String Ntid)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View");
                cmd.Parameters.AddWithValue("@Ntid", Ntid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (UserNotFoundError ex)
            {
                throw new Task_and_Leave_Tracker.UserNotFoundError("User does not exist !!");
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion      


        #region DEL Method - View All Users
        public DataTable ViewAllUserDetailsDEL()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ViewAll");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (UserNotFoundError ex)
            {
                throw new Task_and_Leave_Tracker.UserNotFoundError("User does not exist !!");
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        #region DEL Method - View Users Through RoleId
        public DataTable GetDetailsForTMDEL()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select Ntid,FirstName,LastName from tbl_user_details where RoleID='200'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("User data retreival failed !!");
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        //#region DEL Method - Delete All Users
        //public int DeleteUserDetailsDEL(String Ntid)
        //{
        //    int result = 0;
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", "Delete");
        //        cmd.Parameters.AddWithValue("@Ntid", Ntid);
        //        result = cmd.ExecuteNonQuery();
        //        if (result > 0)
        //        {
        //            Console.WriteLine("Users Are Deleted Successfully!!");
        //        }
        //        else
        //        {
        //            throw new Task_and_Leave_Tracker.DeletionError("Users Are Not Deleted Inserted !!");
        //        }

        //    }

        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        //#endregion


        #region DEL Method - Update All User Details
        public int UpdateUserDetailsDEL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password, String userGuid)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@RoleId", RoleId);
                cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                cmd.Parameters.AddWithValue("@EmailId", EmailId);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@UserGuid", userGuid);
                cmd.Parameters.AddWithValue("@Ntid", Ntid);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.UpdationError("User data updation failed !!");
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion


        #region DEL Method - Check User Exist or Not
        public Boolean CheckUserExistDetailsDEL(String Ntid)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View");
                cmd.Parameters.AddWithValue("@Ntid", Ntid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.UserAlreadyExistsError("User exists already. Try with different ntid!!");
            }
            finally
            {
                con.Close();
            }
        }
        #endregion


        #region DEL Method - Insert Task Details
        public int InsertTaskDetailsDEL(String taskDesc, DateTime createdDate, String expiryDate, String createdBy, String assignedTo, String status, String taskName, String startDate)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@Taskdesc ", taskDesc);
                cmd.Parameters.AddWithValue("@Created_Date ", createdDate);
                cmd.Parameters.AddWithValue("@Expiry_Date ", expiryDate);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@AssignedTo", assignedTo);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@TaskName", taskName);
                cmd.Parameters.AddWithValue("@Start_Date", startDate);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.InsertionError(" Task creation failed !!");
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion


        #region DEL Method - View All Tasks Created By PM
        public DataTable ViewTaskDetailsByPMDEL(String createdBy)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View C");
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("Task data retreival failed !!");
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        #region DEL Method - View All Tasks Assigned To TM
        public DataTable ViewTaskDetailsByTMDEL(String assignedTo)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View A");
                cmd.Parameters.AddWithValue("@AssignedTo", assignedTo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("Task data retreival failed !!");
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        #region DEL Method -  View All Tasks Through TaskId
        public DataTable ViewAllTaskDetailsByIdDEL(int taskId)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View ALL");
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("Task data retreival failed!!");

            }

            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        //#region DEL Method - Delete All Tasks
        //public int DeleteTaskDetailsDEL(int TaskId)
        //{
        //    int result = 0;
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", "Delete");
        //        cmd.Parameters.AddWithValue("@TaskId", TaskId);
        //        result = cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        //#endregion


        #region DEL Method - Update Task Details
        public int UpdateTaskDetailsDEL(int taskId, String taskDesc, String expiryDate, String assignedTo, String taskName)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Update T");
                cmd.Parameters.AddWithValue("@Taskdesc ", taskDesc);
                cmd.Parameters.AddWithValue("@Expiry_Date ", expiryDate);
                cmd.Parameters.AddWithValue("@AssignedTo", assignedTo);
                cmd.Parameters.AddWithValue("@TaskName", taskName);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.UpdationError("Task data updation failed!!");
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion


        #region DEL Method - Update Task Details Based on Status
        public int UpdateTaskStatusDEL(int taskId, String status)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Update S");
                cmd.Parameters.AddWithValue("@Status ", status);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.UpdationError("Task status updation failed!!");
            }
            finally
            {
                con.Close();

            }
            return result;
        }
        #endregion


        #region DEL Method - Insert Leave Details
        public int InsertLeaveDetailsDEL(String leaveDesc, String fromDate, String toDate, String appliedBy, String leaveType, String status)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_leave_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@Leavedesc ", leaveDesc);
                cmd.Parameters.AddWithValue("@FromDate ", fromDate);
                cmd.Parameters.AddWithValue("@ToDate ", toDate);
                cmd.Parameters.AddWithValue("@AppliedBy", appliedBy);
                cmd.Parameters.AddWithValue("@LeaveType", leaveType);
                cmd.Parameters.AddWithValue("@Status", status);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.InsertionError("Leave creation failed !!");
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion


        #region DEL Method - Update Leave Details Based on Status
        public int UpdateLeaveStatusDEL(int leaveId, String status)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_leave_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Update S");
                cmd.Parameters.AddWithValue("@Status ", status);
                cmd.Parameters.AddWithValue("@LeaveId", leaveId);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.UpdationError("Leave data updation failed!!");
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion


        #region DEL Method - View Leaves Applied By TM
        public DataTable ViewLeaveDetailsByTMDEL(String appliedBy)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_leave_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View A");
                cmd.Parameters.AddWithValue("@AppliedBy", appliedBy);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("Unable to apply the leave!!");
            }

            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        #region DEL Method - View Leaves Details By LeaveId
       public DataTable ViewLeaveDetailsByLeaveIdDEL(int leaveId)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_leave_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View L");
                cmd.Parameters.AddWithValue("@LeaveId", leaveId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("Unable to view the leave!!");
            }

            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion


        #region DEL Method - View Pending Leaves For PM
        public DataTable ViewLeaveDetailsByPMDEL()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_leave_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View C");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Task_and_Leave_Tracker.RetreivalError("Unable to view all applied leaves!!");
            }

            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion

    }

}




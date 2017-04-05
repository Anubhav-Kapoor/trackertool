using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace ProjectTracker.DEL
{
    public class UserDetailsDEL
    {



        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\TLT.mdf;Integrated Security=True");

        //InsertFunction[User Details]
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

                Console.WriteLine(ex.Message);
                throw;

            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //ViewFunction[User Details]
        public DataTable ViewUserDetailsDEL(String Ntid)
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
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
               

            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        //ViewIdFunction[User Details]
        public DataTable ViewIdDEL()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select Ntid from tbl_user_details where RoleID='PM'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);


            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        //DeleteFunction[User Details]
        public int DeleteUserDetailsDEL(String Ntid)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@Ntid", Ntid);
                result = cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
               
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        //UpdateFunction[User Details]
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

                Console.WriteLine(ex.Message);
                        
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //CheckUserExistFunction[User Details]
        public Boolean ViewUserExistDetailsDEL(String Ntid)
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
                    return false;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;

            }
            finally
            {
                con.Close();
            }

        }


        //InsertFunction[Task Details]
        public int InsertTaskDetailsDEL(String taskDesc, DateTime createdDate, String expiryDate, String createdBy, String assignedTo, String status, String taskName, String startDate)
        {
            int result = 0;
            try
            {
              

                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@TaskId ", 0);
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

                Console.WriteLine(ex.Message);
                throw;

            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //ViewFunction[Task Details]
        public DataTable ViewTaskDetailsDEL(String createdBy)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "View");
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);


            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        //DeleteFunction[Task Details]
        public int DeleteTaskDetailsDEL(int TaskId)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@TaskId", TaskId);
                result = cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //UpdateFunction[Task Details]
        public int UpdateUserDetailsDEL(int taskId, String taskDesc, DateTime createdDate, DateTime expiryDate, String createdBy, String assignedTo, String status, String taskName, DateTime startDate)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_all_tbl_task_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Update");
                cmd.Parameters.AddWithValue("@Taskdesc ", taskDesc);
                cmd.Parameters.AddWithValue("@Created_Date ", createdDate);
                cmd.Parameters.AddWithValue("@Expiry_Date ", expiryDate.Date);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.Parameters.AddWithValue("@AssignedTo", assignedTo);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@TaskName", taskName);
                cmd.Parameters.AddWithValue("@Start_Date", startDate.Date);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                result = cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            finally
            {
                con.Close();
            }
            return result;
        }

    }
}


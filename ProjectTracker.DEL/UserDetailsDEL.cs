using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProjectTracker.DEL
{
    public class UserDetailsDEL
    {



        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\TLT.mdf;Integrated Security=True");

        public int InsertUserDetailsDEL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
        {





            int result = 0;
           try
            {
                using (TLTEntities2 en = new TLTEntities2())
                {

                    tbl_user_details user = new tbl_user_details();

                    user.Ntid = Ntid;
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.RoleId = RoleId;
                    user.PhoneNo = PhoneNo;
                    user.EmailId = EmailId;
                    user.Password = Password;

                    en.tbl_user_details.Add(user);
                    result = en.SaveChanges();
                }
                //con.Open();
                //SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details",con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Action", "Insert");
                //cmd.Parameters.AddWithValue("@Ntid",Ntid);
                //cmd.Parameters.AddWithValue("@FirstName", FirstName);
                //cmd.Parameters.AddWithValue("@LastName", LastName);
                //cmd.Parameters.AddWithValue("@RoleId", RoleId);
                //cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                //cmd.Parameters.AddWithValue("@EmailId", EmailId);
                //cmd.Parameters.AddWithValue("@Password", Password);
                //result = cmd.ExecuteNonQuery();



            }
           catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
           {
               Exception raise = dbEx;
               foreach (var validationErrors in dbEx.EntityValidationErrors)
               {
                   foreach (var validationError in validationErrors.ValidationErrors)
                   {
                       string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                       // raise a new exception nesting  
                       // the current instance as InnerException  
                       raise = new InvalidOperationException(message, raise);
                   }
               }
               throw raise;
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
            catch
            {

                throw;

            }
            finally
            {
                con.Close();
            }
            return dt;
        }

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
            catch
            {

                throw;

            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public int UpdateUserDetailsDEL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
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
                cmd.Parameters.AddWithValue("@Ntid", Ntid);
                result = cmd.ExecuteNonQuery();


            }
            catch
            {

                throw;

            }
            finally
            {
                con.Close();
            }
            return result;
        }

        
        //public Boolean ViewUserExistDetailsDEL(String Ntid)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_all_tbl_user_details", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", "View");
        //        cmd.Parameters.AddWithValue("@Ntid", Ntid);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //            return false;

        //    }
        //    catch
        //    {

        //        throw;

        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
           
           
        //}
               

        

    }
}


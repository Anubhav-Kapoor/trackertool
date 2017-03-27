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
        public int InsertUserDetailsBLL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
        {
            try
            {
                 result = userDEL.InsertUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password);
                
            }
            catch(Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            
            return result;
        }

        public DataTable ViewUserDetailsBLL(String Ntid)
        {
            try
            {
                DataTable dt = userDEL.ViewUserDetailsDEL(Ntid);
                return dt;
            }
            catch
            {

                throw;

            }

        }

        public int DeleteUserDetails(String Ntid)
        {
            try
            {
                int result = userDEL.DeleteUserDetailsDEL(Ntid);
                return result;
            }
            catch
            {

                throw;

            }

           
        }

        public int UpdateUserDetails(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password)
         {
            try
            {

                int result = userDEL.UpdateUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password);
                return result;
            }

            catch
            {

                throw;

            }

          
        }
        //public Boolean ViewUserExistDetailsBLL(String Ntid)
        //{
        //    try
        //    {
        //        Boolean dt = userDEL.ViewUserExistDetailsDEL(Ntid);
        //        return true;
        //    }
        //    catch
        //    {

        //        throw;

        //    }

        //    finally
        //    {

        //        userDEL = null;

        //    }
        //}
    }
}

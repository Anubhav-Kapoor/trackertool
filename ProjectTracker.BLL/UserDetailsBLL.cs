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
            DataTable dt = null;
            try
            {
                 dt = userDEL.ViewUserDetailsDEL(Ntid);
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            } 
            return dt;

        }

        public int DeleteUserDetailsBLL(String Ntid)
        {
            try
            {
                result = userDEL.DeleteUserDetailsDEL(Ntid);
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }


            return result;

           
        }

        public int UpdateUserDetailsBLL(String Ntid, String FirstName, String LastName, String RoleId, String PhoneNo, String EmailId, String Password, String userGuid)
         {
            try
            {

                 result = userDEL.UpdateUserDetailsDEL(Ntid, FirstName, LastName, RoleId, PhoneNo, EmailId, Password, userGuid);
                
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }


            return result;

          
        }
        public Boolean ViewUserExistDetailsBLL(String Ntid)
        {
            Boolean value = false;
            try
            {
                value = userDEL.ViewUserExistDetailsDEL(Ntid);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            return value;
        }
    }
}

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
            catch(Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            
            return result;
        }

        //ViewFunction[User Details]
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
        //ViewIdFunction[User Details]
        public DataTable ViewIdBLL()
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewIdDEL();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

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

            }


            return result;

          
        }
        //CheckUserExistFunction[User Details]
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

            }


            return result;
        }


        //View Method For TM Function[Task Details]
        public DataTable ViewByTMBLL(String assignedTo)
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewByTMDEL(assignedTo);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            return dt;
        }


        //View Method For PM Function[Task Details]
        public DataTable ViewByPMBLL(String createdBy)
        {
            DataTable dt = null;
            try
            {
                dt = userDEL.ViewByPMDEL(createdBy);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

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

            }


            return result;
        }

        //UpdateFunction[Task Details]
        public int UpdateTaskStatusBLL(int taskId, String status)
        {
            try
            {

                result = userDEL.UpdateTaskStatusDEL(taskId,status);

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }


            return result;
        }

    }
}

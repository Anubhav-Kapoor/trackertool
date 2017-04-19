using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_and_Leave_Tracker
{


    public class CustomExceptionsError : Exception
    {
        private String msg;

        public CustomExceptionsError(String msg)
        {
            this.msg = msg;
                     
        }
    }

    public class UserNotFoundError : Exception
    {
        private String msg;

        public UserNotFoundError(String msg)
        {
            this.msg = msg;
        }
    }

    public class EmailNotSentError : Exception
    {
        private String msg;

        public EmailNotSentError(String msg)
        {
            this.msg = msg;
        }
    }

    public class InsertionError : Exception
    {

        private String msg;

        public InsertionError(String msg)
        {
            this.msg = msg;
            Console.WriteLine(msg);
        }
    }

    public class RetreivalError : Exception
    {
        private String msg;

        public RetreivalError(String msg)
        {
            this.msg = msg;
        }
    }

    public class UpdationError : Exception
    {
        private String msg;

        public UpdationError(String msg)
        {
            this.msg = msg;
        }
    }

    public class DataNotFoundError : Exception
    {
        private String msg;

        public DataNotFoundError(String msg)
        {
            this.msg = msg;
        }
    }

    public class UserAlreadyExistsError : Exception
    {
        private String msg;

        public UserAlreadyExistsError(String msg)
        {
            this.msg = msg;
        }
    }

    public class AuthenticationError : Exception
    {
        private String msg;

        public AuthenticationError(String msg)
        {
            this.msg = msg;
        }
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_and_Leave_Tracker
{


    public class CustomExceptionsError : Exception
    {
        public String msg;
        public CustomExceptionsError()
        {
            this.msg = "";

        }

        public CustomExceptionsError(String msg)
            : base(msg)
        {
            this.msg = msg;

        }
    }

    public class UserNotFoundError : Exception
    {
        public String msg;


        public UserNotFoundError()
        {
            this.msg = "";
        }
        public UserNotFoundError(String msg)
            : base(msg)
        {
            this.msg = msg;
        }
    }

    public class EmailNotSentError : Exception
    {
        public String msg;
        public EmailNotSentError()
        {
            this.msg = "";
        }

        public EmailNotSentError(String msg)
            : base(msg)
        {
            this.msg = msg;
        }
    }

    public class InsertionError : Exception
    {

        public String msg;
        public InsertionError()
        {
            this.msg = "";

        }

        public InsertionError(String msg)
            : base(msg)
        {
            this.msg = msg;

        }
    }


    public class RetreivalError : Exception
    {
        public String msg;

        public RetreivalError()
        {
            this.msg = "";
        }
        public RetreivalError(String msg)
            : base(msg)
        {
            this.msg = msg;
        }
    }

    public class DeletionError : Exception
    {
        public String msg;
        public DeletionError()
        {
            this.msg = "";
        }
        public DeletionError(String msg): base(msg)
        {
            this.msg = msg;
        }
    }

    public class UpdationError : Exception
    {
        public String msg;

        public UpdationError()
        {
            this.msg = "";
        }
        public UpdationError(String msg): base(msg)
        {
            this.msg = msg;
        }
    }



    public class DataNotFoundError : Exception
    {
        public String msg;

        public DataNotFoundError()
        {
            this.msg = "";
        }
        public DataNotFoundError(String msg): base(msg)
        {
            this.msg = msg;
        }
    }

    public class UserAlreadyExistsError : Exception
    {
        public String msg;

        public UserAlreadyExistsError()
        {
            this.msg = "";
        }
        public UserAlreadyExistsError(String msg): base(msg)
        {
            this.msg = msg;
        }
    }

    public class AuthenticationError : Exception
    {
        public String msg;

        public AuthenticationError()
        {
            this.msg = "";
        }
        public AuthenticationError(String msg): base(msg)
        {
            this.msg = msg;
        }
    }


}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_and_Leave_Tracker
{


    public class CustomExceptionsError : Exception
    {
        public  String msg;
        public CustomExceptionsError()
        {
            this.msg = "";

        }

        public CustomExceptionsError(String msg)
        {
            this.msg = msg;
                     
        }
    }

    public class UserNotFoundError : Exception
    {
        public String msg;

        public UserNotFoundError(String msg)
        {
            this.msg = msg;
        }
    }

    public class EmailNotSentError : Exception
    {
        public  String msg;

        public EmailNotSentError(String msg)
        {
            this.msg = msg;
        }
    }

    public class InsertionError : Exception
    {

        public  String msg;

        public InsertionError(String msg)
        {
            this.msg = msg;
            Console.WriteLine(msg);
        }
    }

    public class RetreivalError : Exception
    {
        public  String msg;

        public RetreivalError(String msg)
        {
            this.msg = msg;
        }
    }

    public class UpdationError : Exception
    {
        public  String msg;

        public UpdationError(String msg)
        {
            this.msg = msg;
        }
    }

    public class DataNotFoundError : Exception
    {
        public  String msg;

        public DataNotFoundError(String msg)
        {
            this.msg = msg;
        }
    }

    public class UserAlreadyExistsError : Exception
    {
        public  String msg;

        public UserAlreadyExistsError(String msg)
        {
            this.msg = msg;
        }
    }

    public class AuthenticationError : Exception
    {
        public  String msg;

        public AuthenticationError(String msg)
        {
            this.msg = msg;
        }
    }


}
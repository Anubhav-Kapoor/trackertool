﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_and_Leave_Tracker
{
    public class Response
    {
        public string Status { get; set; }
        public string Reason { get; set; }
        public long newid { get; set; }
        public string responseObject { get; set; }
        public string userObject { get; set; }
        public string taskObject { get; set; }
        public string leaveObject { get; set; }
    }

    public class RootObjectResponse
    {
        public Response Response { get; set; }
    }

    public class Leave
    {
        public int leaveId { get; set; }
        public string leaveDesc { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string appliedBy { get; set; }
        public String leaveType { get; set; }
        public string status { get; set; }

    }
    public class Task
    {
        public int taskId { get; set; }
        public string taskDesc { get; set; }
        public string createdDate { get; set; }
        public string expiryDate { get; set; }
        public String createdBy { get; set; }
        public string assignedTo { get; set; }
        public String status { get; set; }
        public string taskName { get; set; }
        public string startDate { get; set; }
    }

    public class User
    {
        public string ntid { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string roleId { get; set; }
        public String phoneNo { get; set; }
        public string emailId { get; set; }
        public String password { get; set; }
        public string userguid { get; set; }

    }
}
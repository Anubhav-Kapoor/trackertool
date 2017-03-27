using System;
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
    }

    public class RootObjectResponse
    {
        public Response Response { get; set; }
    }
}
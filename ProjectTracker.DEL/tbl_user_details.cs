//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectTracker.DEL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_user_details
    {
        public tbl_user_details()
        {
            this.tbl_leave_details = new HashSet<tbl_leave_details>();
            this.tbl_task_details = new HashSet<tbl_task_details>();
            this.tbl_task_details1 = new HashSet<tbl_task_details>();
        }
    
        public string Ntid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleId { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<tbl_leave_details> tbl_leave_details { get; set; }
        public virtual ICollection<tbl_task_details> tbl_task_details { get; set; }
        public virtual ICollection<tbl_task_details> tbl_task_details1 { get; set; }
        public virtual tbl_user_roles tbl_user_roles { get; set; }
    }
}

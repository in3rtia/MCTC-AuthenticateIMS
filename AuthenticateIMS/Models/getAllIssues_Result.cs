//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuthenticateIMS.Models
{
    using System;
    
    public partial class getAllIssues_Result
    {
        public string movement_ID { get; set; }
        public string description_of_items { get; set; }
        public string request_ID { get; set; }
        public int quantity_supplied { get; set; }
        public string IssuerFirstName { get; set; }
        public string IssuerLastName { get; set; }
        public string ReceiverFNAme { get; set; }
        public string ReceiverLNAme { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> expected_return_date { get; set; }
        public Nullable<System.DateTime> return_date { get; set; }
    }
}

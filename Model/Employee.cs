//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wpf_Karaokay.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string CCCD { get; set; }
        public string EmpPhone { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string AccountName { get; set; }
    
        public virtual Account Account { get; set; }
    }
}

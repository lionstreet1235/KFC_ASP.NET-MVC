//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ILoveKFC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACCOUNT_EMPLOYEE
    {
        public string USERNAME_EMPLOYEE { get; set; }
        public string PASSWORD_EMPLOYEE { get; set; }
        public string ID_EMPLOYEE { get; set; }
        public Nullable<int> TYPE_ACCOUNT { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}

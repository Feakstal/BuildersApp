//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuildersApp_Novikov_3ISP11_13.Helper
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int IdEmployee { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public int IdGender { get; set; }
        public System.DateTime DateOfBith { get; set; }
        public System.DateTime DateOfEmployment { get; set; }
        public Nullable<System.DateTime> DateOfDismissal { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int IdPost { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Gender Gender { get; set; }
        public virtual Post Post { get; set; }
    }
}
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
    
    public partial class SellingService
    {
        public int IdSellingService { get; set; }
        public int IdService { get; set; }
        public int Quantity { get; set; }
        public decimal SalesValue { get; set; }
    
        public virtual Service Service { get; set; }
    }
}

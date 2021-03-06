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
    
    public partial class Component
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Component()
        {
            this.OrderComponent = new HashSet<OrderComponent>();
            this.SellingComponent = new HashSet<SellingComponent>();
        }
    
        public int IdComponent { get; set; }
        public string ComponentName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Nullable<int> IdDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderComponent> OrderComponent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellingComponent> SellingComponent { get; set; }
        public virtual Deleted Deleted { get; set; }
    }
}

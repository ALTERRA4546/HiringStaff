//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HiringStaff
{
    using System;
    using System.Collections.Generic;
    
    public partial class Предмет
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Предмет()
        {
            this.Преподаваемые_предметы = new HashSet<Преподаваемые_предметы>();
            this.Преподоваемые_часы = new HashSet<Преподоваемые_часы>();
        }
    
        public int Код_предмета { get; set; }
        public string Наименование { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Преподаваемые_предметы> Преподаваемые_предметы { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Преподоваемые_часы> Преподоваемые_часы { get; set; }
    }
}

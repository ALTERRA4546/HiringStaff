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
    
    public partial class Преподаваемые_предметы
    {
        public int Код_преподоваемого_предмета { get; set; }
        public int Код_учителя { get; set; }
        public int Код_предмета { get; set; }
    
        public virtual Предмет Предмет { get; set; }
        public virtual Сотрудник Сотрудник { get; set; }
    }
}

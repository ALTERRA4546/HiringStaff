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
    
    public partial class Преподоваемые_часы
    {
        public int Код_преподаваемых_часов { get; set; }
        public int Код_предмета { get; set; }
        public int Код_класса { get; set; }
        public System.DateTime Дата_распределения_часов { get; set; }
        public int Количество_часов { get; set; }
    
        public virtual Классы Классы { get; set; }
        public virtual Предмет Предмет { get; set; }
    }
}

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
    
    public partial class Зарплата
    {
        public int Код_зарплаты { get; set; }
        public int Код_должности { get; set; }
        public int Стаж { get; set; }
        public double Зарплата1 { get; set; }
    
        public virtual Должность Должность { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HiringStaffEntities : DbContext
    {
        public HiringStaffEntities()
            : base("name=HiringStaffEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Авторизация> Авторизация { get; set; }
        public virtual DbSet<График_работы> График_работы { get; set; }
        public virtual DbSet<Документы> Документы { get; set; }
        public virtual DbSet<Должность> Должность { get; set; }
        public virtual DbSet<Зарплата> Зарплата { get; set; }
        public virtual DbSet<Классы> Классы { get; set; }
        public virtual DbSet<Помещение> Помещение { get; set; }
        public virtual DbSet<Предмет> Предмет { get; set; }
        public virtual DbSet<Преподаваемые_предметы> Преподаваемые_предметы { get; set; }
        public virtual DbSet<Преподоваемые_часы> Преподоваемые_часы { get; set; }
        public virtual DbSet<Сотрудник> Сотрудник { get; set; }
    }
}

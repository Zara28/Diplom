﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OfficeTime.DBModels;

public partial class Post
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Rate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OfficeTime.DBModels;

public partial class Dismissal
{
    public int Empid { get; set; }

    public DateTime? Datecreate { get; set; }

    public DateTime? Date { get; set; }

    public bool Isapp { get; set; }

    public virtual Employee Emp { get; set; }
}
﻿using System;
using System.Collections.Generic;
using System.Text;
using AspStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace AspStudio.Data
{
    public class ApplicationDbContext : DbContext
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Device> Devices {get; set;}
        public DbSet<Person> Persons {get; set;}
        public DbSet<Employee> Empleados { get; set; }
        public DbSet<DeviceSite> DeviceSites { get; set; }
        public DbSet<DeviceEmployee> DeviceEmployees { get; set; }
        public DbSet<FhirData> FhirDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}

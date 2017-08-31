﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.HumanResources
{
    public class Nationality : EntityBase
    {
        public string Name { get; set; }

        //public ICollection<JobPost> Posts { get; set; }
    }

    public class NationalityConfiguration : EntityBaseConfiguration<Nationality>
    {
        public NationalityConfiguration()
        {
            ToTable("hr.Nationality");

            //Property(x => x.Name).IsRequired().HasMaxLength(20);
            Property(x => x.Name).IsRequired().HasMaxLength(20).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class GroupInfo : EntityBase
    {
        public string GroupNo { get; set; }
        public string GroupName { get; set; }
        public string Remark { get; set; }

        public ICollection<GroupLightInfo> GroupLightInfoGroupInfos { get; set; }
        //public ICollection<TempTimeSchDetail> TempTimeSchDetailGroupInfos { get; set; }
    }

    public class GroupInfoConfiguraton : EntityBaseConfiguration<GroupInfo>
    {
        public GroupInfoConfiguraton()
        {
            ToTable("li.GroupInfo");

            //Property(x => x.GroupNo).IsRequired().HasMaxLength(10);
            //Property(x => x.GroupNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
            //    IndexAnnotation.AnnotationName,
            //    new IndexAnnotation(new IndexAttribute { IsUnique = true }));
            Property(x => x.GroupName).HasMaxLength(10);
            Property(x => x.Remark).HasMaxLength(50);



        }
    }
}

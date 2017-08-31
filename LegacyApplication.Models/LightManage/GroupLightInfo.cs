using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using LegacyApplication.Models.HumanResources;
using LegacyApplication.Shared.Features.Base;
using LegacyApplication.Shared.Features.Tree;

namespace LegacyApplication.Models.LightManage
{
    public class GroupLightInfo : TreeEntityBase<GroupLightInfo>
    {
        public string No { get; set; }
        public string Describe { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string ReMark { get; set; }
        public string Back1 { get; set; }
        public string Back2 { get; set; }

        public int? GroupInfoId { get; set; }
        public GroupInfo GroupInfo { get; set; }

    }

    public class GroupLightInfoConfiguration : TreeEntityBaseConfiguration<GroupLightInfo>
    {
        public GroupLightInfoConfiguration()
        {
            ToTable("li.GroupLightInfo");

            //联合唯一键
            //Property(x => x.No).IsRequired().HasMaxLength(10).HasColumnAnnotation(
            //    IndexAnnotation.AnnotationName,
            //    new IndexAnnotation(new IndexAttribute("IX_Internal_LightInfo_ControlNoAndLightNo", 0) { IsUnique = true }));
            
            Property(x => x.No).HasMaxLength(10);
            Property(x => x.Describe).HasMaxLength(50);
            Property(x => x.Address).HasMaxLength(50);
            Property(x => x.Longitude).HasMaxLength(20);
            Property(x => x.Latitude).HasMaxLength(20);
            Property(x => x.ReMark).HasMaxLength(100);
            Property(x => x.Back1).HasMaxLength(100);
            Property(x => x.Back2).HasMaxLength(100);

            HasOptional(x => x.GroupInfo).WithMany(x => x.GroupLightInfoGroupInfos).HasForeignKey(x => x.GroupInfoId);

        }
    }
}

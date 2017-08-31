using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class LightInfo : EntityBase
    {
        public string ControlNo { get; set; }
        public string LightNo { get; set; }
        public string LightSonNo { get; set; }
        public string Describe { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string ReMark { get; set; }
        public string Back1 { get; set; }
        public string Back2 { get; set; }

        public string GroupNo { get; set; }
    }

    public class LightInfoConfiguration : EntityBaseConfiguration<LightInfo>
    {
        public LightInfoConfiguration()
        {
            ToTable("li.LightInfo");

            //联合唯一键
            Property(x => x.ControlNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Internal_LightInfo_ControlNoAndLightNo", 0) { IsUnique = true }));
            Property(x => x.LightNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Internal_LightInfo_ControlNoAndLightNo", 1) { IsUnique = true }));
            Property(x => x.LightSonNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Internal_LightInfo_ControlNoAndLightNo", 2) { IsUnique = true }));

            //Property(x => x.UserName).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Internal_ProjectTeamMembers_ProjectIdAndUserName", 1) { IsUnique = true }));

            //Property(x => x.LightSonNo).HasMaxLength(10);
            Property(x => x.Describe).HasMaxLength(50);
            Property(x => x.Address).HasMaxLength(50);
            Property(x => x.Longitude).HasMaxLength(20);
            Property(x => x.Latitude).HasMaxLength(20);
            Property(x => x.ReMark).HasMaxLength(100);
            Property(x => x.Back1).HasMaxLength(100);
            Property(x => x.Back2).HasMaxLength(100);

            //HasRequired(x => x.GroupInfo).WithMany(x => x.LightInfos).HasForeignKey(x => x.GroupInfoId);

        }
    }
}

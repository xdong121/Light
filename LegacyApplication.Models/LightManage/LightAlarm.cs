using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class LightAlarm : EntityBase
    {
        public string ControlNo { get; set; }
        public string LightNo { get; set; }
        public string LightSonNo { get; set; }
        public LightAlarmState State { get; set; }
        public LightAlarmIsRepair IsRepair { get; set; }
        public string ReMark { get; set; }
        public string Back1 { get; set; }
        public string Back2 { get; set; }
    }

    public class LightAlarmConfiguration : EntityBaseConfiguration<LightAlarm>
    {
        public LightAlarmConfiguration()
        {
            ToTable("li.LightAlarm");

            //联合唯一键
            Property(x => x.ControlNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Internal_LightAlarm_ControlNoAndLightNo", 0) { IsUnique = true }));
            Property(x => x.LightNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Internal_LightAlarm_ControlNoAndLightNo", 1) { IsUnique = true }));
            Property(x => x.LightSonNo).IsRequired().HasMaxLength(10).HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Internal_LightAlarm_ControlNoAndLightNo", 2) { IsUnique = true }));

            //Property(x => x.UserName).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Internal_ProjectTeamMembers_ProjectIdAndUserName", 1) { IsUnique = true }));

            Property(x => x.ReMark).HasMaxLength(100);
            Property(x => x.Back1).HasMaxLength(100);
            Property(x => x.Back2).HasMaxLength(100);

            //HasRequired(x => x.GroupInfo).WithMany(x => x.LightAlarms).HasForeignKey(x => x.GroupInfoId);

        }
    }
}

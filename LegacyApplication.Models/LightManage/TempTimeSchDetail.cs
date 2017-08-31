
using System;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class TempTimeSchDetail : EntityBase
    {
        public DateTime StartTime { get; set; }
        public TempTimeSchDetailIsUse IsUse { get; set; }
        public int Light { get; set; }
        public DateTime? MakeTime { get; set; }
        public TempTimeSchDetailResult Result { get; set; }
        public string Remark { get; set; }

        public int? GroupInfoId { get; set; }
        public GroupInfo Group { get; set; }

        public int? TempTimeSchMainId { get; set; }
        public TempTimeSchMain TempTimeSchMain { get; set; }

    }

    public class TempTimeSchDetailConfiguraton : EntityBaseConfiguration<TempTimeSchDetail>
    {
        public TempTimeSchDetailConfiguraton()
        {
            ToTable("li.TempTimeSchDetail");

            Property(x => x.Remark).HasMaxLength(50);

            HasOptional(x => x.Group).WithMany().HasForeignKey(x => x.GroupInfoId);
            HasOptional(x => x.TempTimeSchMain).WithMany(x => x.TempTimeSchDetailTempTimeSchMains).HasForeignKey(x => x.TempTimeSchMainId);

        }
    }
}

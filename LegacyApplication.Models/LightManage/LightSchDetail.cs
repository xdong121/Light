
using System;
using System.Collections.Generic;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class LightSchDetail : EntityBase
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Power { get; set; }
        public string Remark { get; set; }

        public int? LightSchMainId { get; set; }
        public LightSchMain LightSchMain { get; set; }
    }

    public class LightSchDetailConfiguraton : EntityBaseConfiguration<LightSchDetail>
    {
        public LightSchDetailConfiguraton()
        {
            ToTable("li.LightSchDetail");

          
            Property(x => x.Remark).HasMaxLength(50);

            HasOptional(x => x.LightSchMain).WithMany(x => x.LightSchDetailLightSchMains).HasForeignKey(x => x.LightSchMainId);

        }
    }
}


using System;
using System.Collections.Generic;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class LightSchMain : EntityBase
    {
        public string ControlNo { get; set; }
        public DateTime? MakeTime { get; set; }
        public LightSchMainResult Result { get; set; }
        public string Remark { get; set; }

        public int? GroupLightInfoId { get; set; }
        public GroupLightInfo GroupLightInfo { get; set; }

        public ICollection<LightSchDetail> LightSchDetailLightSchMains { get; set; }

    }

    public class LightSchMainConfiguraton : EntityBaseConfiguration<LightSchMain>
    {
        public LightSchMainConfiguraton()
        {
            ToTable("li.LightSchMain");

            Property(x => x.ControlNo).HasMaxLength(10);
            Property(x => x.Remark).HasMaxLength(50);

            HasOptional(x => x.GroupLightInfo).WithMany().HasForeignKey(x => x.GroupLightInfoId);

        }
    }
}

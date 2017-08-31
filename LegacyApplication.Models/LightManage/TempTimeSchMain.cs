
using System;
using System.Collections.Generic;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class TempTimeSchMain : EntityBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? MakeTime { get; set; }
        public TempTimeSchMainResult Result { get; set; }
        public string Remark { get; set; }

        public ICollection<TempTimeSchDetail> TempTimeSchDetailTempTimeSchMains { get; set; }

    }

    public class TempTimeSchMainConfiguraton : EntityBaseConfiguration<TempTimeSchMain>
    {
        public TempTimeSchMainConfiguraton()
        {
            ToTable("li.TempTimeSchMain");

            Property(x => x.Remark).HasMaxLength(50);

        }
    }
}

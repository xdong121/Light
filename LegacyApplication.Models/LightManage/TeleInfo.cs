
using System;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.Models.LightManage
{
    public class TeleInfo : EntityBase
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }
        public DateTime? ReadTime { get; set; }

    }

    public class TeleInfoConfiguraton : EntityBaseConfiguration<TeleInfo>
    {
        public TeleInfoConfiguraton()
        {
            ToTable("li.TeleInfo");

            Property(x => x.Type).IsRequired().HasMaxLength(10);
            Property(x => x.Content).IsRequired().HasMaxLength(1000);

        }
    }
}

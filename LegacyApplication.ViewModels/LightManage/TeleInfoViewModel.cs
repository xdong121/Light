using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class TeleInfoViewModel : EntityBase
    {
        //[Display(Name = "电文类型")]
        //public TeleInfoType Type { get; set; }
        //[Display(Name = "电文类型")]
        //public string TypeDisplay => Type.ToString();

        [Display(Name = "电文类型")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Type { get; set; }


        [Display(Name = "电文内容")]
        [StringLength(1000, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Content { get; set; }

        //[Display(Name = "结果电文")]
        //public TeleInfoResult Result { get; set; }
        //[Display(Name = "结果电文")]
        //public string ResultDisplay => Result.ToString();

        [Display(Name = "结果电文")]
        [StringLength(1000, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Result { get; set; }

        [Display(Name = "结果读入/生成时间")]
        public DateTime? ReadTime { get; set; }

    }
}

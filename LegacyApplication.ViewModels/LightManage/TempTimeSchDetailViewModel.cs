using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class TempTimeSchDetailViewModel : EntityBase
    {
        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "{0}是必填项")]
        public DateTime StartTime { get; set; }

        [Display(Name = "是否启用")]
        public TempTimeSchDetailIsUse IsUse { get; set; }
        [Display(Name = "是否启用")]
        public string IsUseDisplay => IsUse.ToString();

        [Display(Name = "亮度")]
        [Required(ErrorMessage = "{0}是必填项")]
        public int Light { get; set; }

        [Display(Name = "下达时间")]
        public DateTime? MakeTime { get; set; }

        [Display(Name = "返回结果")]
        public TempTimeSchDetailResult Result { get; set; }
        [Display(Name = "返回结果")]
        public string ResultDisplay => Result.ToString();

        [Display(Name = "备注")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Remark { get; set; }

        [Display(Name = "组ID")]
        public int? GroupInfoId { get; set; }

        [Display(Name = "方案ID")]
        public int? TempTimeSchMainId { get; set; }

        public GroupInfoViewModel Group { get; set; }
        public TempTimeSchMainViewModel TempTimeSchMain { get; set; }
    }
}

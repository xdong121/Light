using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class TempTimeSchMainViewModel : EntityBase
    {
        [Display(Name = "开始日期")]
        [Required(ErrorMessage = "{0}是必填项")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束日期")]
        [Required(ErrorMessage = "{0}是必填项")]
        public DateTime EndDate { get; set; }

        [Display(Name = "下达时间")]
        public DateTime? MakeTime { get; set; }

        [Display(Name = "返回结果")]
        public TempTimeSchMainResult Result { get; set; }
        [Display(Name = "返回结果")]
        public string ResultDisplay => Result.ToString();

        [Display(Name = "备注")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Remark { get; set; }


    }
}

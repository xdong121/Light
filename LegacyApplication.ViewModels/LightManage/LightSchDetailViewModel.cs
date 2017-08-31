using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class LightSchDetailViewModel : EntityBase
    {

        [Display(Name = "照度等级")]
        [Required(ErrorMessage = "{0}是必填项")]
        public int Start { get; set; }

        [Display(Name = "光照终点")]
        public int End { get; set; }

        [Display(Name = "亮度")]
        [Required(ErrorMessage = "{0}是必填项")]
        public int Power { get; set; }


        [Display(Name = "备注")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Remark { get; set; }

        [Display(Name = "主表ID")]
        public int? LightSchMainId { get; set; }
        public LightSchMainViewModel LightSchMain { get; set; }
    }
}

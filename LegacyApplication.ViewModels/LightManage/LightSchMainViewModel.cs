using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class LightSchMainViewModel : EntityBase
    {
        [Display(Name = "控制器编号")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string ControlNo { get; set; }

        [Display(Name = "下达时间")]
        public DateTime? MakeTime { get; set; }

        [Display(Name = "返回结果")]
        public LightSchMainResult Result { get; set; }
        [Display(Name = "返回结果")]
        public string ResultDisplay => Result.ToString();

        [Display(Name = "备注")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Remark { get; set; }

        [Display(Name = "灯节点ID")]
        public int? GroupLightInfoId { get; set; }
        public GroupLightInfoViewModel GroupLightInfo { get; set; }


    }
}

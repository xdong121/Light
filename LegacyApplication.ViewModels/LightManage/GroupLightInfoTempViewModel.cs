using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;
using LegacyApplication.Shared.Features.Tree;
using LegacyApplication.ViewModels.HumanResources;

namespace LegacyApplication.ViewModels.LightManage
{
    public class GroupLightInfoTempViewModel  : GroupLightInfoViewModel
    {

        [Display(Name = "控制器编号")]
        [StringLength(20, ErrorMessage = "{0}的长度不可超过{1}")]
        public string ControlNo { get; set; }

        [Display(Name = "灯线编号")]
        [StringLength(20, ErrorMessage = "{0}的长度不可超过{1}")]
        public string LightNo { get; set; }

        [Display(Name = "状态")]
        [StringLength(20, ErrorMessage = "{0}的长度不可超过{1}")]
        public string LightState { get; set; }

        [Display(Name = "亮度")]
        [StringLength(20, ErrorMessage = "{0}的长度不可超过{1}")]
        public string LightPower { get; set; }

        

    }
}

using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class LightAlarmViewModel : EntityBase
    {
        [Display(Name = "控制器编号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string ControlNo { get; set; }

        [Display(Name = "灯线编号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string LightNo { get; set; }

        [Display(Name = "灯子编号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string LightSonNo { get; set; }

        [Display(Name = "状态")]
        public LightAlarmState State { get; set; }
        [Display(Name = "状态")]
        public string StateDisplay => State.ToString();

        [Display(Name = "是否修复")]
        public LightAlarmIsRepair IsRepair { get; set; }
        [Display(Name = "是否修复")]
        public string IsRepairDisplay => IsRepair.ToString();

        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string ReMark { get; set; }

        [Display(Name = "备注1")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Back1 { get; set; }

        [Display(Name = "备注2")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Back2 { get; set; }

    }
}

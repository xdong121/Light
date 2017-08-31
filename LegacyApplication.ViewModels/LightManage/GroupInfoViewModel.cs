using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;

namespace LegacyApplication.ViewModels.LightManage
{
    public class GroupInfoViewModel : EntityBase
    {

        [Display(Name = "组编号")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string GroupNo { get; set; }


        [Display(Name = "组名称")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string GroupName { get; set; }


        [Display(Name = "备注")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Remark { get; set; }

        

    }
}

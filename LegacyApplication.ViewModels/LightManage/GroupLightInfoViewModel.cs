using System;
using System.ComponentModel.DataAnnotations;
using LegacyApplication.Shared.ByModule.LightManage.Enums;
using LegacyApplication.Shared.Features.Base;
using LegacyApplication.Shared.Features.Tree;
using LegacyApplication.ViewModels.HumanResources;

namespace LegacyApplication.ViewModels.LightManage
{
    public class GroupLightInfoViewModel : TreeEntityBase<GroupLightInfoViewModel>
    {

        [Display(Name = "编号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(10, ErrorMessage = "{0}的长度不可超过{1}")]
        public string No { get; set; }

        public string Text => No;
        
        [Display(Name = "描述")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Describe { get; set; }

        [Display(Name = "地址")]
        [StringLength(50, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Address { get; set; }

        [Display(Name = "经度")]
        [StringLength(20, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Longitude { get; set; }

        [Display(Name = "纬度")]
        [StringLength(20, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Latitude { get; set; }

        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string ReMark { get; set; }

        [Display(Name = "备注1")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Back1 { get; set; }

        [Display(Name = "备注2")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Back2 { get; set; }

        [Display(Name = "组ID")]
        public int? GroupInfoId { get; set; }
        //public GroupInfoViewModel GroupInfo { get; set; }

    }
}

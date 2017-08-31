﻿using AutoMapper;
using LegacyApplication.Models.Core;
using LegacyApplication.Models.HumanResources;
using LegacyApplication.Models.LightManage;
using LegacyApplication.Models.Scrum;
using LegacyApplication.Models.Work;
using LegacyApplication.ViewModels.Core;
using LegacyApplication.ViewModels.HumanResources;
using LegacyApplication.ViewModels.LightManage;
using LegacyApplication.ViewModels.Scrum;
using LegacyApplication.ViewModels.Work;
using LegacyStandalone.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LegacyStandalone.Web.MyConfigurations.Mapping
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName => "ViewModelToDomainMappings";

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<RoleViewModel, IdentityRole>();
            CreateMap<RoleViewModel, IdentityUserRole>();

            CreateMap<UploadedFileViewModel, UploadedFile>();

            CreateMap<InternalMail, InternalMailViewModel>();
            CreateMap<InternalMailTo, InternalMailToViewModel>();
            CreateMap<InternalMailAttachment, InternalMailAttachmentViewModel>();

            CreateMap<DepartmentViewModel, Department>()
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore());
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<JobPostLevelViewModel, JobPostLevel>();
            CreateMap<JobPostViewModel, JobPost>();
            CreateMap<AdministrativeLevelViewModel, AdministrativeLevel>();
            CreateMap<AdministrativePostViewModel, AdministrativePost>();
            CreateMap<TitleLevelViewModel, TitleLevel>();
            CreateMap<TitlePostViewModel, TitlePost>();
            CreateMap<NationalityViewModel, Nationality>();

            CreateMap<ProjectViewModel, Project>();
            CreateMap<FeatureViewModel, Feature>();
            CreateMap<SprintViewModel, Sprint>();
            CreateMap<ProductBacklogItemViewModel, ProductBacklogItem>();
            CreateMap<BugViewModel, Bug>();
            CreateMap<ProductBacklogItemTaskViewModel, ProductBacklogItemTask>();
            CreateMap<BugTaskViewModel, BugTask>();
            CreateMap<ProjectTeamMemberViewModel, ProjectTeamMember>();

            CreateMap<LightInfoViewModel, LightInfo>();
            CreateMap<TeleInfoViewModel, TeleInfo>();
            CreateMap<GroupInfoViewModel, GroupInfo>();
            CreateMap<GroupLightInfoViewModel, GroupLightInfo>()
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore());
            CreateMap<GroupLightInfoTempViewModel, GroupLightInfo>()
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore());
            CreateMap<TempTimeSchMainViewModel, TempTimeSchMain>();
            CreateMap<TempTimeSchDetailViewModel, TempTimeSchDetail>();
            CreateMap<LightSchMainViewModel, LightSchMain>();
            CreateMap<LightSchDetailViewModel, LightSchDetail>();
            CreateMap<LightAlarmViewModel, LightAlarm>();
        }
    }
}
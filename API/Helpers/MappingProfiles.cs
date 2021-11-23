using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Applicant
            CreateMap<Applicant, ApplicantToReturnDto>();
            CreateMap<ApplicantDto, Applicant>();
            #endregion
          
        }
    }
}
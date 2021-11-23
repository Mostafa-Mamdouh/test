using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RESTCountries.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{


    public class ApplicantController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicantController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ApplicantToReturnDto>>> GetApplicants(
          [FromQuery] ApplicantSpecParams applicantParams)
        {
            var spec = new ApplicantListSpecification(applicantParams);
            var countSpec = new ApplicantListCountSpecification(applicantParams);
            var totalItems = await _unitOfWork.Repository<Applicant>().CountAsync(countSpec);
            var Applicants = await _unitOfWork.Repository<Applicant>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ApplicantToReturnDto>>(Applicants);
            return Ok(new Pagination<ApplicantToReturnDto>(applicantParams.PageIndex,
                applicantParams.PageSize, totalItems, data));
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportApplicants(
        [FromQuery] ApplicantSpecParams applicantParams)
        {

            var spec = new ApplicantListSpecification(applicantParams);
            var Applicants = await _unitOfWork.Repository<Applicant>().ListAsync(spec);

            var data = Applicants.Select(x => new
            {
                ID = x.Id,
                FullName = x.Name + " " + x.FamilyName,
                Address = x.Address,
                Country = x.CountryOfOrigin,
                Email = x.EMailAdress,
                Age = x.Age + " Years Old",
                Hired = x.Hired ? "Hired" : "Not Hired",
                CreationDate = x.CreateDate
            });
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("ApplicantList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
                workSheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
                workSheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Left.Color.SetColor(Color.Black);
                workSheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Right.Color.SetColor(Color.Black);
                workSheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Top.Color.SetColor(Color.Black);
                workSheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Bottom.Color.SetColor(Color.Black);
                workSheet.Cells["A1:H1"].Style.Font.Bold = true;
                workSheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:H1"].Style.Font.Color.SetColor(Color.White);
                var Count = data.Count() + 2;
                workSheet.Cells["H2:H" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(data, true);

                await package.SaveAsync();
            }
            stream.Position = 0;
            string excelName = $"ApplicantList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicantToReturnDto>> GetApplicantById(int id)
        {
            var spec = new ApplicantListSpecification(id);
            var applicant = await _unitOfWork.Repository<Applicant>().GetEntityWithSpec(spec);
            if (applicant == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<ApplicantToReturnDto>(applicant);
        }


        [HttpGet("applicantexists")]
        public async Task<ActionResult<bool>> CheckApplicantExistsAsync([FromQuery] int id, string name, string familyName, string email)
        {
            var spec = new ApplicantListSpecification(id, name, familyName, email);
            return await _unitOfWork.Repository<Applicant>().GetEntityWithSpec(spec) != null;
        }
        [HttpPost("create")]
        public async Task<ActionResult<ApplicantToReturnDto>> CreateApplicant(ApplicantDto applicantDto)
        {
            var applicant = _mapper.Map<Applicant>(applicantDto);
            _unitOfWork.Repository<Applicant>().Add(applicant);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating Applicant"));
            return Ok(_mapper.Map<ApplicantToReturnDto>(applicant));
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApplicantToReturnDto>> UpdateApplicant(ApplicantDto applicantDto)
        {
            var applicant = await _unitOfWork.Repository<Applicant>().GetByIdAsync(applicantDto.Id);
            var editableApplicant = _mapper.Map<ApplicantDto, Applicant>(applicantDto, applicant);
            _unitOfWork.Repository<Applicant>().Update(editableApplicant);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating Applicant"));
            return Ok(_mapper.Map<ApplicantToReturnDto>(editableApplicant));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicantToReturnDto>> DeleteApplicant(int id)
        {
            var applicant = await _unitOfWork.Repository<Applicant>().GetByIdAsync(id);
            _unitOfWork.Repository<Applicant>().Delete(applicant);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting Applicant"));
            return Ok(_mapper.Map<ApplicantToReturnDto>(applicant));
        }
    }
}

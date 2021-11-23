using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ApplicantListCountSpecification : BaseSpecifcation<Applicant>
    {
        public ApplicantListCountSpecification(ApplicantSpecParams applicantParams) : base(x =>
          (string.IsNullOrEmpty(applicantParams.Search) ||
        x.Name.ToLower().Contains(applicantParams.Search) ||
        x.Id.ToString().ToLower().Contains(applicantParams.Search) ||
        x.FamilyName.ToLower().Contains(applicantParams.Search) ||
        x.Address.ToLower().Contains(applicantParams.Search) ||
        x.EMailAdress.ToLower().Contains(applicantParams.Search) ||
        x.CountryOfOrigin.ToLower().Contains(applicantParams.Search))
        )
        {
            

        }
    }
}

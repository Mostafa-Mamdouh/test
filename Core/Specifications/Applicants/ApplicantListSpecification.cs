using Core.Entities;
using System.ComponentModel;
using System.Linq;

namespace Core.Specifications
{
    public class ApplicantListSpecification : BaseSpecifcation<Applicant>
    {
        public ApplicantListSpecification(ApplicantSpecParams applicantParams) : base(x =>
                   (string.IsNullOrEmpty(applicantParams.Search) ||
        x.Name.ToLower().Contains(applicantParams.Search) ||
        x.Id.ToString().ToLower().Contains(applicantParams.Search) ||
        x.FamilyName.ToLower().Contains(applicantParams.Search) ||
        x.Address.ToLower().Contains(applicantParams.Search) ||
        x.EMailAdress.ToLower().Contains(applicantParams.Search) ||
        x.CountryOfOrigin.ToLower().Contains(applicantParams.Search)))
        {
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(applicantParams.PageSize * (applicantParams.PageIndex - 1), applicantParams.PageSize);

            if (!string.IsNullOrEmpty(applicantParams.Sort) && !string.IsNullOrEmpty(applicantParams.SortDirection))
            {
                System.Reflection.PropertyInfo prop = typeof(Applicant).GetProperty("" + applicantParams.Sort + "");
                switch (applicantParams.SortDirection)
                {
                    case "asc":
                        AddOrderBy(p => prop.GetValue(p, null));
                        break;
                    case "desc":
                        AddOrderByDescending(p => prop.GetValue(p, null));
                        break;
                    default:
                        AddOrderByDescending(n => n.CreateDate);
                        break;
                }
            }

        }
        public ApplicantListSpecification(int ApplicantId) : base(x => x.Id == ApplicantId)
        {

        }
        public ApplicantListSpecification(int id, string name, string familyName, string email) : base(x => x.Name == name && x.FamilyName == familyName && x.EMailAdress == email && x.Id != id)
        {
        }

    }
}

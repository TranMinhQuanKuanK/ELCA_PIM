using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resource.AddEditProject;
namespace ContractLayer
{
    public class AddEditProjectModel
    {
        public AddEditProjectModel()
        {
            Id = null;
            GroupId = null;
            ProjectNumber = null;
            Name = "";
            Customer = "";
            Status = "NEW";
            StartDate = DateTime.Now;
            EndDate = null;
            MemberString = "";
            MembersList = new List<string>();
            Version = 1;
        }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.ID_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.IDRequired_ModelError))]
        [Range(0, 9999999999999999999, ErrorMessageResourceType = typeof(AddEditProjectRe), ErrorMessageResourceName = nameof(AddEditProjectRe.IDRange_ModelError))]
        public long? Id { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.GroupID_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.GroupIPRequired_ModelError))]
        [Range(0, 9999999999999999999, ErrorMessageResourceType = typeof(AddEditProjectRe), ErrorMessageResourceName = nameof(AddEditProjectRe.GroupIDRange_ModelError))]
        public long? GroupId { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.ProjectNumber_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.ProjectNumberRequired_ModelError))]
        [Range(0, 9999, ErrorMessageResourceType = typeof(AddEditProjectRe), ErrorMessageResourceName = nameof(AddEditProjectRe.ProjectNumberRange_ModelError))]
        public short? ProjectNumber { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.Name_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.NameRequired_ModelError))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.Customer_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.CustomerRequired_ModelError))]
        public string Customer { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.Status_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.StatusRequired_ModelError))]
        public string Status { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.StartDate_DisplayName))]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = nameof(AddEditProjectRe.StartDateRequired_ModelError))]
        public DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.EndDate_DisplayName))]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage ="Version required!")]
        [Range(0,999999999999,ErrorMessage ="Invalid range of version detected")]
        public int Version { get; set; }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = nameof(AddEditProjectRe.Members_DisplayName))]
        public string MemberString { get; set; }
        public List<string> MembersList { get; set; }
    }
}

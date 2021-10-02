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
            ID = null;
            GroupID = null;
            ProjectNumber = null;
            Name = "";
            Customer = "";
            Status = "";
            StartDate = DateTime.Now;
            EndDate = null;
            Members = "";
            MembersList = new List<string>();
        }

        [Display(ResourceType = typeof(AddEditProjectRe), Name = "ID_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "IDRequired_ModelError")]
        [Range(0, 9999999999999999999, ErrorMessageResourceType = typeof(AddEditProjectRe), ErrorMessageResourceName = "IDRange_ModelError")]
        public long? ID { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "GroupID_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "GroupIPRequired_ModelError")]
        [Range(0, 9999999999999999999, ErrorMessageResourceType = typeof(AddEditProjectRe), ErrorMessageResourceName = "GroupIDRange_ModelError")]
        public long? GroupID { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "ProjectNumber_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "ProjectNumberRequired_ModelError")]
        [Range(0, 9999, ErrorMessageResourceType = typeof(AddEditProjectRe), ErrorMessageResourceName = "ProjectNumberRange_ModelError")]
        public short? ProjectNumber { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "Name_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "NameRequired_ModelError")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "Customer_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "CustomerRequired_ModelError")]
        public string Customer { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "Status_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "StatusRequired_ModelError")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "StartDate_DisplayName")]
        [Required(ErrorMessageResourceType = typeof(AddEditProjectRe),
            ErrorMessageResourceName = "StartDateRequired_ModelError")]
        public DateTime StartDate { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "EndDate_DisplayName")]
        public DateTime? EndDate { get; set; }
        [Display(ResourceType = typeof(AddEditProjectRe), Name = "Members_DisplayName")]
        public string Members { get; set; }
        public List<string> MembersList { get; set; }
    }
}

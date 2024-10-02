using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ExperienceAdminView
    {
        public ResumeExperience ResumeExperience { get; set; }
        public List<ResumeDescription> ResumeDescriptions { get; set; } = new List<ResumeDescription>();
    }
}
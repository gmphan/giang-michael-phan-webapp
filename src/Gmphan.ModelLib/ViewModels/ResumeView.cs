using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ResumeView
    {
        public ResumeHeader? ResumeHeader { get; set; }
        public ResumeSummary? ResumeSummary { get; set; }
        public List<ResumeExperience>? ResumeExperiences { get; set; }

        public ResumeView()
        {
            ResumeHeader = new ResumeHeader();
            ResumeSummary = new ResumeSummary();
            ResumeExperiences = new List<ResumeExperience>();
        }

        public void SortExperiencesByDate()
        {
            if (ResumeExperiences.Count > 0)
            {
                ResumeExperiences = ResumeExperiences.OrderByDescending(x => x.ToYear).ToList();
            }
        }
    }
}
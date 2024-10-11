using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ProjectTaskActivityView
    {
        public int Id { get; set; }
        public string? ActivityNote { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
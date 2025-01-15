using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourses.Application.DTOs.Courses
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

}

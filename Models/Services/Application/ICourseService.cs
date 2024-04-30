using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.ViewModels;
using System.Threading.Tasks;

namespace MyCourse.Models.Services.Application
{
    //questa interfaccia include i metodi che un servizio,
    //per poter essere compatibile con il controller;
    //deve implementare affinchè funzioni
    public interface ICourseService
    {
      Task<List<CourseViewModel>> GetCoursesAsync();
       Task<CourseDetailViewModel> GetCourseAsync(int id);
        

    }
}
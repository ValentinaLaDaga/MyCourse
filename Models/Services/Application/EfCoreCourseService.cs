using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.Entities;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MyCourse.Models.Services.Application
{
    public class EfCoreCourseService : ICourseService
    { 
        private readonly MyCourseDbContext dbContext;

        public EfCoreCourseService(MyCourseDbContext dbContext){
            this.dbContext = dbContext;
        }

        //recupera tutte le info dei corsi da database
        //SELECT * FROM Courses
        public async Task<List<CourseViewModel>> GetCoursesAsync()
        {
            //crea un oggetto di tipo List<CourseViewModel> che verrà passato alla View
            //stavolta questo oggetto lo popola utilizzando il metodo Select del Framework Entity Core
            //il quale richiede una espressione lambda tramite cui popoliamo gli oggetti di tipo Entity Course
            List<CourseViewModel> courses = await dbContext.Courses.Select(course => new CourseViewModel 
            {
                Id = course.Id,
                Titolo = course.Title,
                ImgPath = course.ImagePath,
                Autore = course.Author,
                Rating = course.Rating,
                PrezzoFull = course.FullPrice,
                PrezzoScontato = course.CurrentPrice
            }).ToListAsync();//Viene aperta una connessione con il db e inviata la query al database 

            return courses; 
        }

        //metodo che recupera il corso che ha id come parametro
        //SELECT * FROM Courses Where Id=id
        public async Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
                CourseDetailViewModel viewModel = await dbContext.Courses
                .Where(course => course.Id == id)
                .Select(course => new CourseDetailViewModel 
                {
                    Id = course.Id,
                    Titolo = course.Title,
                    Descrizione = course.Description,
                    ImgPath = course.ImagePath,
                    Autore = course.Author,
                    Rating = course.Rating,
                    PrezzoFull = course.FullPrice,
                    PrezzoScontato = course.CurrentPrice,

                    //recupero tutte le lezioni del corso che ho già recuperato con la query precedente       
                    Lezioni = course.Lessons.Select(lesson => new LessonViewModel
                    {
                        Id= lesson.Id,
                        Titolo = lesson.Title,
                        Durata = lesson.Duration
                    }).ToList()//si connette al database e recupera la lista delle lezioni associate al corso
                }).SingleAsync();//Restituisce il primo elemento dell'elenco, ma se l'elenco ne contiene 0 o più di 1, solleva un'eccezione

                return viewModel;
        }
    }
}
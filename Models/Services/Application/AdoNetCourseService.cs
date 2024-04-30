using System;
using System.Collections.Generic;
using System.Linq;
using MyCourse.Models.Services.Infrastractures;
using System.Data;
using System.Threading.Tasks;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService //servizio applicativo che ha il compito di recuperare dati dal database e lo fa tramite l utilizzo del servizio infrastrutturale IDatabaseAccessor
    {
        private readonly IDatabaseAccessor db; // proprieta che deve essere iniettata nel servizio applicativo

        public AdoNetCourseService(IDatabaseAccessor db) // dipendenza debole
        {
            this.db=db;
        }
        public async Task<List<CourseViewModel>> GetCoursesAsync()
         {
            FormattableString query= $"SELECT Id, Title,ImagePath, Author,Rating,FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency  FROM Courses";
            DataSet dataSet=await db.QueryAsync(query);
            var dataTable= dataSet.Tables[0]; //recupera la prima tabella del dataset
            var courseList = new List<CourseViewModel>(); //crea la lista di corsi che deve eseere passata all view
            // per ogni riga presente nalla datatable deve creare un oggetto di tipo CourseViewModel 
            foreach(DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
         }
       public async Task<CourseDetailViewModel> GetCourseAsync(int id)
       {
           FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id}
            ; SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";
            DataSet dataSet=await db.QueryAsync(query);
            var courseTable = dataSet.Tables[0];
            
            //dato che la prima query mi ritorna esattamente una riga, controllo se effettivamente 
            //la tabella contiene una sola riga tramite Rows.Count
            if (courseTable.Rows.Count != 1) {
                throw new InvalidOperationException($"Did not return exactly 1 row for Course {id}");
            }
            //recupero i dati della prima riga della tabella -> quindi i dati del corso con id recuperato
            var courseRow = courseTable.Rows[0];
            //creo l'oggetto ViewModel popolandolo con i dati recuperati da db
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            //recupero i dati della tabella risultato della seconda query
            var lessonTable = dataSet.Tables[1];

            //per ogni riga (lezione) presente nella tabella crea una lezione (oggetto LessonViewModel)
            //popolandolo con i dati letti da database
            foreach(DataRow lessonRow in lessonTable.Rows){
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                //aggiungi la lezione alla lista di lezioni del corso
                courseDetailViewModel.Lezioni.Add(lessonViewModel);
            }

            return courseDetailViewModel;


        }
    }
}
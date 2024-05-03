using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueTypes;

namespace MyCourse.Models.ViewModels
{
    //classe che estende l'entità Corso, aggiungendo descrizione e lista di lezioni
    //che mi serviranno nella pagina di dettaglio
    public class CourseDetailViewModel : CourseViewModel
    {
        public string Descrizione {get; set;}
        public List<LessonViewModel> Lezioni {get; set;}
        public TimeSpan TotalCourseDuration
        {
            get => TimeSpan.FromSeconds(Lezioni?.Sum(l => l.Durata.TotalSeconds) ?? 0);
            /*l'operatore null-coalescing ??
            significa che se quello che c'è a sinistra è Null, allora restituisce 0
            altrimenti restituisce quello che c'è a sinistra (cioè la somma)
            */
        }

        public CourseDetailViewModel()
        {
            Lezioni = new List<LessonViewModel>();
        }

        public static new CourseDetailViewModel FromDataRow(DataRow courseRow){
            var courseDetailViewModel = new CourseDetailViewModel{
                Titolo= Convert.ToString(courseRow["Title"]), //recupero il titolo dal db leggendo la colonna title
                Descrizione = Convert.ToString(courseRow["Description"]),
                ImgPath = Convert.ToString(courseRow["ImagePath"]),
                Autore = Convert.ToString(courseRow["Author"]),
                Rating = Convert.ToDouble(courseRow["Rating"]),
                PrezzoFull = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                    Convert.ToDecimal(courseRow["FullPrice_Amount"])
                ),
                PrezzoScontato = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                    Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                ),
                Id = Convert.ToInt32(courseRow["Id"]),
                Lezioni = new List<LessonViewModel>()
            };
            return courseDetailViewModel;
                
            } 
        }
    }

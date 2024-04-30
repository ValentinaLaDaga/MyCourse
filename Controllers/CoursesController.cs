using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;
using System.Threading.Tasks;



namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService CourseService; //oggetto che deve essere automaticamente iniettato da asp.netcore 
        //quando viene invocato questo controller
        //è una interfaccia per realizzare il disaccoppiamento debole (dependency injection)
        public CoursesController(ICourseService courseService)
        {
            //verrà iniettato automaticamente un oggetto di una classe che implementa l'interfaccia ICourseService
            this.CourseService = courseService;
        }


        //metodo per recuperare la lista di tutti i corsi
        public async Task<IActionResult> Index() 
        {
            //var courseService = new CourseService(); //invocazione del servizio
            //la creazione dell'oggetto (servizio) non serve più perchè tramite l'injection, asp.net core lo fa in automatico
            
            List<CourseViewModel> courses =await CourseService.GetCoursesAsync();
            
            ViewData["Title"] = "Elenco dei corsi";
            return View(courses); //ritorna la lista di tutti i corsi
        }

        //metodo che deve recuperare le info dello specifico corso avente un certo id
        public async Task<IActionResult> Detail(int id)
        {
            //var courseService = new CourseService(); //invocazione del servizio
            //la creazione non serve più perchè tramite l'injection, asp.net core lo fa in automatico
            
           CourseDetailViewModel viewModel =await CourseService.GetCourseAsync(id);
            
            ViewData["Title"] = viewModel.Titolo;
            return View(viewModel);
          
            //return Content($"Sono Detail, ho ricevuto l'id {id}");
            //return Redirect("https://www.amazon.it/");
        }

    public IActionResult Ricerca(string cerca)
{
    // Controlla se la stringa di ricerca è vuota
    if (cerca=="tutti")
    {
        // Se la stringa di ricerca è vuota, reindirizza alla pagina principale o mostra un messaggio di errore
        return RedirectToAction("Index");
        // Oppure, se vuoi restituire un messaggio di errore:
        // return Content("Inserisci un termine di ricerca valido.");
    }
    else {
        return RedirectToAction("detail" , new {id = cerca});
    }

   
} 



      
 
    }
}
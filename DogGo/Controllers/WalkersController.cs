using DogGo.Models;
using DogGo.Models.DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepo;
        private readonly IDogRepository _dogRepo;
 

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IDogRepository dogRepository, IWalksRepository walksRepository)

        {
            _walkerRepo = walkerRepository;
            _walksRepo = walksRepository;
            _dogRepo = dogRepository;
        }

        // GET: WalkersController
      
            // GET: Walkers
            public ActionResult Index()
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();

                return View(walkers);
            }

       

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walks> walks = _walksRepo.GetAllWalksByWalkerId(id);
            int totalTimeMinutes = _walksRepo.GetWalkerTime(id);

            TotalTime totalWalks = new TotalTime()
            {
                Hours = totalTimeMinutes / 60,
                Minutes = totalTimeMinutes % 3600
            };
            WalkerViewModel vm = new WalkerViewModel()
            {
                Walker = walker,
                Walks = walks,
                TotalTime = totalWalks
            };

            if (walker == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

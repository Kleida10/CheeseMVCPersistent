using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;
        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();

            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name,
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {

            List<CheeseMenu> items = context
           .CheeseMenus
           .Include(item => item.Cheese)
           .Where(cm => cm.MenuID == id)
           .ToList();

            Menu menu = context.Menus.Single(c => c.ID == id);

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel()
            {
                Menu = menu,
                Items = items
            };

            return View(viewMenuViewModel);
        
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();
            
            return View(new AddMenuItemViewModel(menu,cheeses));
        }
        
        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
        .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
        .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID).ToList();
               
                if (existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu
                    {
                        Menu = context.Menus.Single(c => c.ID == addMenuItemViewModel.MenuID),
                        Cheese = context.Cheeses.Single(c => c.ID == addMenuItemViewModel.CheeseID)
                    };
                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();
                   return Redirect("/Menu/ViewMenu/" + newCheeseMenu.MenuID);
                 }
                return Redirect("/Menu");
            }

            return View(addMenuItemViewModel);
        }
    }
}
using CheeseMVC.Models;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class ViewMenuViewModel
    {
       
        public Menu Menu { get; set; }
        public IList<CheeseMenu> Items { get; set; }
       
    }
}

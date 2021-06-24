using DogGo.Models.DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerViewModel
    {
        public Walker Walker { get; set; }
        public List<Walks> Walks { get; set; }
        public Walks Walk { get; set; } = new Walks();
        public TotalTime TotalTime { get; set; }
    }
}

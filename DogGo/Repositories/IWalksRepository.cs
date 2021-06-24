using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        List<Walks> GetAllWalksByWalkerId(int id);
        public int GetWalkerTime(int id);
    }
}

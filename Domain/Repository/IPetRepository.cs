using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Domain.Repository
{
    public interface IPetRepository : IRepository<Pet>
    {
        List<Pet> GetOwnerPetsList(int ownerId);
    }
}

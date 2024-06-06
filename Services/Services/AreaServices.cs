using BookingBad.DAL;
using BookingBad.BLL.DTO;
using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.Services
{

    public interface IAreaServices
    {
        public List<Area> GetComplexe();
        public Area GetComplexeById(int id);
        public void CreateComplexe(Area area);
        public void UpdateComplexe(int id, Area area);
        public void DeleteComplexe(int id);
    }
    public class AreaServices : IAreaServices
    {
        public void CreateComplexe(Area area)
        {
            throw new NotImplementedException();
        }

        public void DeleteComplexe(int id)
        {
            throw new NotImplementedException();
        }

        public List<Area> GetComplexe()
        {
            throw new NotImplementedException();
        }

        public Area GetComplexeById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateComplexe(int id, Area area)
        {
            throw new NotImplementedException();
        }
    }
}

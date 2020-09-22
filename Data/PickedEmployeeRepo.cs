using System;
using System.Linq;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public class PickedEmployeeRepo : IPickedEmployeeRepo
    {
        private readonly BaseContext _context;

        public PickedEmployeeRepo(BaseContext context)
        {
            _context = context;
        }

        public PickedEmployee GetPickedEmployee(Guid pickedEmployeeId)
        {
            PickedEmployee pickedEmployee = _context.PickedEmployees.FirstOrDefault(pe => pe.PickedEmployeeId == pickedEmployeeId);
            return pickedEmployee;
        }

        public void PickEmployee(PickedEmployee pickedEmployee)
        {
            _context.PickedEmployees.Add(pickedEmployee);
        }

        public void UnpickEmployee(PickedEmployee pickedEmployee)
        {
            _context.PickedEmployees.Remove(pickedEmployee);
        }

        public PickedEmployee GetPickedEmployeeByAdvertAndEmployee(Guid advertId, Guid employeeId)
        {
            var pickedEmployee = _context.PickedEmployees.FirstOrDefault(pe => pe.AdvertId.Equals(advertId) && pe.EmployeeId.Equals(employeeId));
            return pickedEmployee;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

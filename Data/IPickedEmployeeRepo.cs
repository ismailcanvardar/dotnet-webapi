using System;
using Commander.Models;

namespace Commander.Data
{
    public interface IPickedEmployeeRepo
    {
        bool SaveChanges();
        void PickEmployee(PickedEmployee pickedEmployee);
        void UnpickEmployee(PickedEmployee pickedEmployee);
        PickedEmployee GetPickedEmployee(Guid pickedEmployeeId);
        PickedEmployee GetPickedEmployeeByAdvertAndEmployee(Guid advertId, Guid employeeId);
    }
}

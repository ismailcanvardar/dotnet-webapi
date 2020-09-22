using System;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
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

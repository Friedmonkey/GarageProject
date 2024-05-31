using Bogus.DataSets;
using DatabaseLibrary.Database.Apointments;
using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Database.Materials;
using DatabaseLibrary.Database.ServiceActions;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DatabaseLibrary.Database.AppointmentCouples;

public class AppointmentCoupleRepository : IAppointmentCoupleRepository
{
    //private readonly DatabaseContext _database;
    private readonly IDbContextFactory<DatabaseContext> _databaseFactory;
    private readonly IServiceActionRepository _serviceActionRepository;

    public AppointmentCoupleRepository(IDbContextFactory<DatabaseContext> dbFactory, IServiceActionRepository serviceActionRepository)
    {
        _databaseFactory = dbFactory;
        _serviceActionRepository = serviceActionRepository;
    }

    #region Helpers
    private async Task<AppointmentServiceAction> ResolveAppointmentServiceAction(AppointmentServiceActionDTO entity)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var serviceAction = (await _serviceActionRepository.GetServiceActionsByFilter(id: entity.ServiceActionId)).FirstOrDefault();
            if (serviceAction == null)
            {
                return null;
            }

            return new AppointmentServiceAction()
            {
                ID = entity.ID,
                AppointmentId = entity.AppointmentId,
                ServiceAction = serviceAction,
            };
        }
    }
    #endregion

    #region Create
    public async Task CreateAppointmentServiceActionCouple(int appointmentId, int serviceActionId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            await _database.AppointmentServiceActionCouples.AddAsync(new AppointmentServiceActionDTO()
            {
                AppointmentId = appointmentId,
                ServiceActionId = serviceActionId,
            });
            await _database.SaveChangesAsync();
        }
    }
    #endregion

    #region Read

    public async Task<AppointmentServiceAction?> GetAppointmentServiceActionCouple(int appointmentId, int serviceActionId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var result = await _database.AppointmentServiceActionCouples.FirstOrDefaultAsync(im => im.AppointmentId == appointmentId && im.ServiceActionId == serviceActionId);

            if (result == null) return null;
            return await ResolveAppointmentServiceAction(result);
        }
    }

    public async Task<List<AppointmentServiceAction>?> GetAllAppointmentServiceActionCouple(int appointmentId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            List<AppointmentServiceAction> appointmentServiceActions = new List<AppointmentServiceAction>();

            var results = await _database.AppointmentServiceActionCouples.Where(im => im.AppointmentId == appointmentId).ToListAsync();

            if (results == null) return null;

            foreach (var appointmentServiceAction in results)
            {
                var a = await ResolveAppointmentServiceAction(appointmentServiceAction);
                if (a != null)
                    appointmentServiceActions.Add(a);
            }
            return appointmentServiceActions;
        }
    }
    #endregion

    #region Delete

    public async Task DeleteAppointmentServiceActionCouple(int appointmentServiceActionId)
    {
        using (var _database = _databaseFactory.CreateDbContext())
        {
            var couple = await _database.AppointmentServiceActionCouples.FirstOrDefaultAsync(im => im.ID == appointmentServiceActionId);
            if (couple != null)
            {
                _database.AppointmentServiceActionCouples.Remove(couple);
                await _database.SaveChangesAsync();
            }
        }
    }



    #endregion
}

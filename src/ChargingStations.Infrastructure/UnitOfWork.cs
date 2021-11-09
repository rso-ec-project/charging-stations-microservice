using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using ChargingStations.Domain.TenantAggregate;
using ChargingStations.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        private readonly List<ApplicationDbContext> _applicationDbContexts = new();

        private IChargerRepository _chargerRepository;
        private IChargerModelRepository _chargerModelRepository;
        private IChargingStationRepository _chargingStationRepository;
        private ITenantRepository _tenantRepository;

        private bool _createContext;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = applicationDbContext;
        }

        public IChargerRepository ChargerRepository
        {
            get
            {
                if (_createContext)
                {
                    _createContext = false;

                    return new ChargerRepository(CreateDbContext());
                }

                return _chargerRepository ??= new ChargerRepository(_context);
            }
        }

        public IChargerModelRepository ChargerModelRepository
        {
            get
            {
                if (_createContext)
                {
                    _createContext = false;

                    return new ChargerModelRepository(CreateDbContext());
                }

                return _chargerModelRepository ??= new ChargerModelRepository(_context);
            }
        }

        public IChargingStationRepository ChargingStationRepository
        {
            get
            {
                if (_createContext)
                {
                    _createContext = false;

                    return new ChargingStationRepository(CreateDbContext());
                }

                return _chargingStationRepository ??= new ChargingStationRepository(_context);
            }
        }

        public ITenantRepository TenantRepository
        {
            get
            {
                if (_createContext)
                {
                    _createContext = false;

                    return new TenantRepository(CreateDbContext());
                }

                return _tenantRepository ??= new TenantRepository(_context);
            }
        }

        public IUnitOfWork CreateContext()
        {
            _createContext = true;
            return this;
        }

        private ApplicationDbContext CreateDbContext()
        {
            var options = _serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            var dbContext = new ApplicationDbContext(options);
            _applicationDbContexts.Add(dbContext);
            return dbContext;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            for (var i = 0; i < _applicationDbContexts.Count; i++)
            {
                if (_applicationDbContexts[i] != null)
                {
                    _applicationDbContexts[i].Dispose();
                    _applicationDbContexts[i] = null;
                }
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }

        private async ValueTask DisposeAsyncCore()
        {
            for (var i = 0; i < _applicationDbContexts.Count; i++)
            {
                if (_applicationDbContexts[i] != null)
                {
                    await _applicationDbContexts[i].DisposeAsync().ConfigureAwait(false);

                    _applicationDbContexts[i] = null;
                }
            }
        }
    }
}

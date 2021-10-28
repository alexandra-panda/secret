using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Common.Utilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SyncNode.Settings;

namespace SyncNode.Services
{
    public class SyncWorkJobService: IHostedService
    {
        private readonly IWeaponApiSettings _weaponApiSettings;
        private readonly ConcurrentDictionary<Guid, SyncEntity> _entities;
        private readonly ILogger<SyncWorkJobService> _logger;
        private Timer _timer;

        public SyncWorkJobService(IWeaponApiSettings weaponApiSettings,
            ILogger<SyncWorkJobService> logger)
        {
            _weaponApiSettings = weaponApiSettings;
            _entities = new ConcurrentDictionary<Guid, SyncEntity>();
            _logger = logger;
        }

        public void AddItem(SyncEntity entity)
        {
            SyncEntity cachedEntity = null;

            var exist = _entities.TryGetValue(entity.Id, out cachedEntity);

            if (!exist || cachedEntity.UpdatedTimestamp < entity.UpdatedTimestamp)
            {
                _entities[entity.Id] = entity;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoSendWork(object state)
        {
            foreach (var entity in _entities.Values)
            {
                SyncEntity cachedEntity = null;

                var exist = _entities.TryRemove(entity.Id, out cachedEntity);

                if (exist)
                {
                    foreach (var receiver in _weaponApiSettings.Hosts.Where(h => !h.Contains(entity.Origin)))
                    {
                        var url = $"{receiver}/{entity.ObjectType}/sync";

                        try
                        {
                            var result = SyncApiClient.SendJson(entity.JsonData, url, entity.SyncType);

                            if (!result.IsSuccessStatusCode)
                            {
                                _logger.LogError(result.StatusCode.ToString());

                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                }
            }
        }
    }
}
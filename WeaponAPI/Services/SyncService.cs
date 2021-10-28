using System.Net.Http;
using System.Text.Json;
using Common.Models;
using Common.Utilities;
using Microsoft.AspNetCore.Http;
using WeaponAPI.Services.Abstracts;
using WeaponAPI.Settings.Abstracts;

namespace WeaponAPI.Services
{
    public class SyncService<T>: ISyncService<T> where T : BaseEntityModel
    {
        private readonly ISyncServiceSettings _syncServiceSettings;
        private readonly string _hostOrigin;
        
        public SyncService(ISyncServiceSettings syncServiceSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _syncServiceSettings = syncServiceSettings;
            var protocol = httpContextAccessor?.HttpContext?.Request.IsHttps ?? false ? "https" : "http";
            _hostOrigin = $"{protocol}://{httpContextAccessor?.HttpContext?.Request.Host}";
        }
        public HttpResponseMessage Upsert(T record)
        {
            var json = ToSyncEntityJson(record, _syncServiceSettings.UpsertHttpMethod);
            var response = SyncApiClient.SendJson(json, _syncServiceSettings.Host, "POST");
            return response;
        }

        public HttpResponseMessage Delete(T record)
        {
            var json = ToSyncEntityJson(record, _syncServiceSettings.DeleteHttpMethod);
            var response = SyncApiClient.SendJson(json, _syncServiceSettings.Host, "POST");
            return response;
        }

        private string ToSyncEntityJson(T record, string syncType)
        {
            var objectType = typeof(T);
            var syncEntity = new SyncEntity()
            {
                JsonData = JsonSerializer.Serialize(record),
                SyncType = syncType,
                ObjectType = objectType.Name,
                Id = record.Id,
                UpdatedTimestamp = record.UpdatedTimestamp,
                Origin = _hostOrigin
            };

            return JsonSerializer.Serialize(syncEntity);
        }
    }
}
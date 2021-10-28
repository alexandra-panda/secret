using System.Net.Http;
using Common.Models;

namespace WeaponAPI.Services.Abstracts
{
    public interface ISyncService<T> where T: BaseEntityModel
    {
        HttpResponseMessage Upsert(T record);
        HttpResponseMessage Delete(T record);
    }
}
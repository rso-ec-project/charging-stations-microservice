using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.News
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetAsync();
    }
}

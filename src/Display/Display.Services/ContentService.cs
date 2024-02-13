using Display.Models;
using Display.Models.App;
using Display.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Services
{
    public class ContentService: IContentService
    {
        private readonly IRepository<DetailedScreenModel> _repository;
        private  readonly IDeviceRegistrationRepository _deviceRegistrationRepository;

        public ContentService(IRepository<DetailedScreenModel> repository, IDeviceRegistrationRepository deviceRegistrationRepository)
        {
            _repository = repository;
            _deviceRegistrationRepository = deviceRegistrationRepository;
        }

        public async Task<DetailedScreenModel?> GetDetailsAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);

            if(screen?.PlaylistData?.Items != null)
            {
                var dic = ToKeyValuePair(screen?.PlaylistData?.Items);
                if (screen?.PlaylistData != null)
                {
                    screen.PlaylistData.ItemsSerialized = dic;
                    screen.PlaylistData.Items = null;
                }
            }

            return screen;
        }

        private static List<KeyValuePair<string, string>> ToKeyValuePair(IList<object>? items)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var keyValuePairs = new List<KeyValuePair<string, string>>();
            if(items == null) return keyValuePairs;

            foreach (var item in items!)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(item.GetType().Name, JsonConvert.SerializeObject(item, serializerSettings)));
            }

            return keyValuePairs;
        }

        public Task<DeviceCodeRegistrationModel?> GetDeviceAsync(string id) =>
            _deviceRegistrationRepository.GetAsyncById(id);
    }
}

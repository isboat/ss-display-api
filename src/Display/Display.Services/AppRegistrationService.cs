using Display.Models.App;
using Display.Repositories;

namespace Display.Services
{
    public class AppRegistrationService : IAppRegistrationService
    {
        private readonly IAppRegisterRepository _repository;

        public AppRegistrationService(IAppRegisterRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegisterModel?> GetStatus(int? regNumber)
        {
            var regModel = await _repository.GetAsync(regNumber);
            if (string.IsNullOrEmpty(regModel?.TenantId) || !IsApproved(regModel))
            {
                return null;
            }

            return regModel;
        }

        private bool IsApproved(RegisterModel model)
        {
            return model?.ApprovedDatetime != null && model.ApprovedDatetime > DateTime.UnixEpoch;

        }

        public async Task<RegisterResponseModel> Register(RegisterRequestModel model)
        {
            var registerModel = new RegisterModel
            {
                Id = Guid.NewGuid().ToString("N"),
                RegisteredDatetime = DateTime.UtcNow,
                DeviceExtraInfo = model.DeviceExtraInfo,
                DeviceName = model.DeviceName,
                RegNumber = model.RegNumber,
            };
            var response = await _repository.InsertAsync(registerModel);
            return new RegisterResponseModel { Success = response };
        }
    }
}

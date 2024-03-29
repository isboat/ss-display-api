﻿using Display.Models;
using Display.Models.App;
using Display.Repositories;

namespace Display.Services
{
    public interface IContentService
    {
        Task<DetailedScreenModel?> GetDetailsAsync(string tenantId, string id);

        Task<DeviceCodeRegistrationModel?> GetDeviceAsync(string id);
    }
}
using System;
using Lusa.AddinEngine.Extension;

namespace Lusa.UI.Msic.ConfigrationService
{
    public static class IConfigrationServiceExtension
    {
        public static bool IsProductionConfig(this IConfigurationService configService)
        {
            if(configService.IsNull())
            {
                return false;
            }

            return "prod".Equals(configService.CurrentConfigName,StringComparison.OrdinalIgnoreCase);
        }
    }
}

using System;
using CommonExtension;

namespace CommonLibrary
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

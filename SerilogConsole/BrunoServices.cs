using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SerilogConsole
{
    public class BrunoServices : IBrunoServices
    {
        private readonly ILogger<BrunoServices> logger;
        private readonly IConfiguration config;

        public BrunoServices(ILogger<BrunoServices> logger, IConfiguration config) // DI
        {
            this.logger = logger;
            this.config = config;
        }
        public void Run()
        {
            try
            {
                for (int i = 0; i < config.GetValue<int>("LoopTimes"); i++)
                {
                    if (i == 5)
                    {
                        throw new System.Exception("Esco dal programma");
                    }
                    logger.LogInformation("Run number {runNumber}", i);
                }
            }
            catch (System.Exception ex)
            {

                logger.LogError(ex.Message);

            }
        }
    }
}

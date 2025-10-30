using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace SpaceTradersCLI
{
    internal class SpaceTradersAPIHelper
    {
        private AgentsApi agentsApi;
        private FleetApi fleetApi;
        private GlobalApi globalApi;
        private FactionsApi factionsApi;
        private AppSettings appSettings;

        public SpaceTradersAPIHelper(AppSettings appSettings)
        {
            this.appSettings = appSettings;
            agentsApi = new AgentsApi();
            fleetApi = new FleetApi();
            globalApi = new GlobalApi();
            factionsApi = new FactionsApi();
            LoadTokens();
        }

        public void LoadTokens()
        {
            Configuration cliConfig = new()
            {
                AccessToken = appSettings.GetCliAccessToken()
            };
            globalApi.Configuration = cliConfig;
            factionsApi.Configuration = cliConfig;

            Configuration agentConfig = new()
            {
                AccessToken = appSettings.GetAgentAccessToken()
            };
            agentsApi.Configuration = agentConfig;
            fleetApi.Configuration = agentConfig;
        }

        public bool CheckAccount()
        {
            return !(string.IsNullOrEmpty(appSettings.GetCliAccessToken()));
        }

        public bool CheckAgent()
        {
            return !(string.IsNullOrEmpty(appSettings.GetAgentAccessToken()));
        }

        public Agent GetAgentData()
        {
            InlineResponse20011 agentResponse = agentsApi.GetMyAgent();
            return agentResponse.Data;
        }
        
        public Task<InlineResponse20011> GetAgentDataAsync()
        {
            return agentsApi.GetMyAgentAsync();
        }

        public InlineResponse201 CreateAgent(RegisterBody registerBody)
        {
            InlineResponse201 response = globalApi.Register(registerBody);
            appSettings.SaveAgentAuth(response.Data.Token);
            LoadTokens();
            return response;
        }

        public List<Faction> getFactions()
        {
            InlineResponse2009 response = factionsApi.GetFactions();
            return response.Data;
        }

        public List<Ship> getFleetData()
        {
            InlineResponse20018 response = fleetApi.GetMyShips();
            return response.Data;
        }

        public AgentsApi GetAgentsApi()
        {
            return agentsApi;
        }

        public GlobalApi GetGlobalApi()
        {
            return globalApi;
        }
    }
}

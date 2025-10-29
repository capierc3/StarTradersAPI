using IO.Swagger.Api;
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
            globalApi.Configuration.AccessToken = appSettings.GetCliAccessToken();
            factionsApi.Configuration.AccessToken = appSettings.GetCliAccessToken();
            agentsApi.Configuration.AccessToken = appSettings.GetAgentAccessToken();
            fleetApi.Configuration.AccessToken = appSettings.GetAgentAccessToken();
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

using IO.Swagger.Api;
using IO.Swagger.Model;
using IniParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;

namespace SpaceTradersCLI
{
    internal class SpaceTradersAPIHelper
    {
        private string accessToken = "";
        private AgentsApi agentsApi;
        private FleetApi fleetApi;
        private string AgentSymbol = "";

        public SpaceTradersAPIHelper()
        {
            agentsApi = new AgentsApi();
            fleetApi = new FleetApi();
            //get access token from config.ini
            var parser = new IniParser.FileIniDataParser();
            IniData data = parser.ReadFile("../config.ini");
            accessToken = data["AUTH"]["AccessToken"];
            //set access token for apis
            agentsApi.Configuration.AccessToken = accessToken;
            fleetApi.Configuration.AccessToken = accessToken;
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
        
        public void SetAgentSymbol(string symbol)
        {
            AgentSymbol = symbol;
        }
        public string GetAgentSymbol()
        {
            return AgentSymbol;
        }
    }
}

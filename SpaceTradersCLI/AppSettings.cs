using IniParser.Model;

namespace SpaceTradersCLI
{
    internal class AppSettings
    {
        private string cliAccessToken = "";
        private string agentAccessToken = "";
        private string agentCreationDate = "";

        public AppSettings()
        {
            //get access token from config.ini
            var parser = new IniParser.FileIniDataParser();
            if (!File.Exists("../config.ini"))
            {
                using (StreamWriter sw = File.CreateText("../config.ini"))
                {
                    sw.WriteLine("[APP AUTH]");
                    sw.WriteLine("AccessToken=");
                    sw.WriteLine();
                    sw.WriteLine("[AGENT AUTH]");
                    sw.WriteLine("AccessToken=");
                    sw.WriteLine("CreationDate=");
                }
            }
            IniData data = parser.ReadFile("../config.ini");
            cliAccessToken = data["APP AUTH"]["AccessToken"];
            agentAccessToken = data["AGENT AUTH"]["AccessToken"];
            agentCreationDate = data["AGENT AUTH"]["CreationDate"];
        }

        public void SaveAppAuth(string cliAccessToken)
        {
            var parser = new IniParser.FileIniDataParser();
            IniData data = parser.ReadFile("../config.ini");
            data["APP AUTH"]["AccessToken"] = cliAccessToken;
            parser.WriteFile("../config.ini", data);
        }

        public void SaveAgentAuth(string cliAccessToken)
        {
            var parser = new IniParser.FileIniDataParser();
            IniData data = parser.ReadFile("../config.ini");
            data["AGENT AUTH"]["AccessToken"] = cliAccessToken;
            //data["AGENT AUTH"]["CreationDate"] = ;
            parser.WriteFile("../config.ini", data);
        }

        public string GetCliAccessToken()
        {
            return cliAccessToken;
        }

        public string GetAgentAccessToken()
        {
            return agentAccessToken;
        }

        public string GetAgentCreationDate()
        {
            return agentCreationDate;
        }
    }
}

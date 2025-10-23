using IO.Swagger.Api;
using IO.Swagger.Model;


Console.WriteLine("Logging in... Please Wait");
AgentsApi agentsApi = new AgentsApi();
agentsApi.Configuration.AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZGVudGlmaWVyIjoiV0lMTF9OT1JUT04iLCJ2ZXJzaW9uIjoidjIuMy4wIiwicmVzZXRfZGF0ZSI6IjIwMjUtMTAtMTkiLCJpYXQiOjE3NjExNzYwNTgsInN1YiI6ImFnZW50LXRva2VuIn0.D_11yzfvv3ciDMfirW1cJwo--gZsYLPpDKmyOTiplNoRo3Xxt3mXW-cdG1qbxliuh1Fqxlo6cy50LwpTYPI9Gt29SYZfSaGhKPoJOP0zuA4H9lrmtj8jkibDZZSQ5dNNXGGN81g8asr_e2s38QaQ72G-bJx3eZYHvh1Lcka53uedBjLM942qGqs-0kQMI_hYMR2NMN6GpPXYTDAE-jeQ0pfzf3EAQ1Dcm25JoPnNQZ334MtEcflPNMJ8ZZf8FolLE16Ev5krjVGPu45F0gdfHfIlV83zxHrS0uwsDvkPfNVH1bfzdBRqQ-SAT1CtxyAxr-H1PfMwAyX2vAgaVT5u8U7y4PzzZgfC5a8DJMAub9_P2A7MHB7SBEeBQgdcbFRk5Z5aIrtehKBgOaQQsj9EFS5G8FNZVLqk4Fkrr_CGTTQ3YRser9HvV3Ymuon2EsWqqXNP9qxWPtAITWWjbORqoI1YHDsusHLfHX6_1JiSBbUC0Ous2GuOarnc6aXhgzxNNEU9EFBzqLkKoMTvjlMxhGPjno7nVSwNjzSc7L5WyzemDjRyNyxEcR5RN15ffLGMhJOgF1QCAeab7tphCzVWEbr8TjfPZT4KlffvObW-QgfUAmUXEJQkf4O_jDFwyFe3IR9H9Li7eFNtUGDJ1kY8UFS5KO4D-ngtCEdG1dEWlgg";
InlineResponse20011 response = agentsApi.GetMyAgent();
Agent agent = response.Data;
Console.Clear();
Console.WriteLine("Hello, " + agent.Symbol);
Console.WriteLine("--------------------------");
Console.WriteLine("What would you like to do?");
string input = Console.ReadLine();

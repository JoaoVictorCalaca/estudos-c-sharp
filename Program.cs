using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // Biblioteca para manipular JSON

class Program
{
  static async Task Main(string[] args)
  {
    Console.WriteLine("digite 'sair' para encerrar");

    while (true)
    {
      Console.Write("\n Sua mensagem: ");
      string? userMessage = Console.ReadLine() ?? string.Empty;

      if (userMessage.ToLower() == "sair")
        break;

      string respostaIA = await MakeRequest(userMessage);
      Console.Write("\nResposta da IA: ");
      Console.WriteLine(respostaIA);
    }
  }

  static async Task<string> MakeRequest(string userPrompt)
  {
    string url = "http://localhost:11434/api/generate";

    using (HttpClient client = new HttpClient())
    {
      var requestData = new
      {
        model = "llama3.2",
        //instrução de papel para a IA
        prompt = $"{userPrompt}",
        stream = false,
      }
    ;

    string json = JsonConvert.SerializeObject(requestData);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    try
    {
      HttpResponseMessage response = await client.PostAsync(url, content);
      string responseData = await response.Content.ReadAsStringAsync();

      // Convertendo o JSON para um objeto
      var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseData);

      // Pegando apenas o campo "response"
      return jsonResponse?.response ?? "Erro ao processar a resposta.";
    }
    catch (Exception ex)
    {
      return $"Erro na requisição: {ex.Message}";
    }
  }
}
}

using System.Text;

HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://localhost:7201")
};

var tasks = Enumerable.Range(0, 10000).Select(x => Task.Run(async () =>
{
    var response = await httpClient.PostAsync("createTinyUrl", new StringContent("{\r\n  \"originalUrl\": \"string\"\r\n}", Encoding.UTF8, "application/json"));
    response.EnsureSuccessStatusCode();
    var a = await response.Content.ReadAsStringAsync();
    Console.WriteLine(a);
})).ToArray();

Task.WaitAll(tasks);

Console.WriteLine();
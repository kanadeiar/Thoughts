
using Microsoft.Extensions.Logging;

using Thoughts.WebAPI.Clients.Identity;

var http = new HttpClient
{
    BaseAddress = new("https://localhost:5011")
};

//ILogger<AccountClient> client_logger = LoggerFactory.Create(builder => builder.AddConsole());


var account_client = new AccountClient(http);

Console.WriteLine("Ожидание сервера");
Console.ReadLine();

// Реализация авторизации, получение JWT от сервиса, установка заголовка в http
//http.DefaultRequestHeaders.Add("???", ???);

var roles = await account_client.GetAllRolessAsync();
var users = await account_client.GetAllUsersAsync();

Console.WriteLine("End.");

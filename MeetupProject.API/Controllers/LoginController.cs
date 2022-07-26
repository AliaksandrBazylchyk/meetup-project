using MeetupProject.API.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MeetupProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            HttpClient client = new HttpClient();

            var url = "http://identity/connect/token";
            var options = new Dictionary<string, string>();
            options.Add("grant_type", "password");
            options.Add("username", request.Username);
            options.Add("password", request.Password);
            options.Add("client_id", "Default");
            options.Add("scope", "APIs");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(options)).Result;
            var token = response.Content.ReadAsStringAsync().Result;

            client.Dispose();

            return Ok(token);
        }
    }
}

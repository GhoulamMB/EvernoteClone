using EvernoteClone.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
#pragma warning disable CS8618
namespace EvernoteClone.ViewModel.Helpers
{
    public class FirebaseHelper
    {
        private const string API_KEY = "AIzaSyAvq7-mvs_DuUDYvd4NB7d6cPW7Qd25WvQ";

        public async static Task<bool> Register(User user)
        {
            using(HttpClient client = new())
            {
                var body = new
                {
                    email = user.UserName,
                    password = user.Password,
                    returnSecureToken = true
                };
                var bodyJSON = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJSON,System.Text.Encoding.UTF8,"application/json");

                var request = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={API_KEY}",data);

                if (request.IsSuccessStatusCode)
                {
                    string resultJSON = await request.Content.ReadAsStringAsync();
                    FirebaseResult result = JsonConvert.DeserializeObject<FirebaseResult>(resultJSON)!;
                    App.UserID = result.localId;
                    return true;
                }
                else
                {
                    string errorJSON = await request.Content.ReadAsStringAsync();
                    Error error = JsonConvert.DeserializeObject<Error>(errorJSON)!;
                    MessageBox.Show(error.error.message);
                }
            }
            return false;
        }

        public async static Task<bool> Login(User user)
        {
            using (HttpClient client = new())
            {
                var body = new
                {
                    email = user.UserName,
                    password = user.Password,
                    returnSecureToken = true
                };
                var bodyJSON = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJSON, System.Text.Encoding.UTF8, "application/json");

                var request = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={API_KEY}", data);

                if (request.IsSuccessStatusCode)
                {
                    string resultJSON = await request.Content.ReadAsStringAsync();
                    FirebaseResult result = JsonConvert.DeserializeObject<FirebaseResult>(resultJSON)!;
                    App.UserID = result.localId;
                    return true;
                }
                else
                {
                    string errorJSON = await request.Content.ReadAsStringAsync();
                    Error error = JsonConvert.DeserializeObject<Error>(errorJSON)!;
                    MessageBox.Show(error.error.message);
                }
            }
            return false;
        }

    }

    internal class FirebaseResult
    {
        public string localId { get; set; }
        public string email { get; set; }
        public string idToken { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
        public string kind { get; set; }
    }

    internal class Error
    {
        public ErrorDetails error { get; set; }
    }

    internal class ErrorDetails
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}

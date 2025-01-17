namespace uninside.Auth
{
    internal class App
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string Fcm { get; set; }

        public App(string token, string id, string fcm)
        {
            Token = token;
            Id = id;
            Fcm = fcm;
        }
    }
}
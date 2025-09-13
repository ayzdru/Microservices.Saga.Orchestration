namespace BlazorWebAppOidc
{
    public class IdentityServerSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        public string SignedOutCallbackPath { get; set; }
        public string RemoteSignOutPath { get; set; }
        public string SignedOutRedirectUri { get; set; }
        public List<string> Scope { get; set; }
    }
}

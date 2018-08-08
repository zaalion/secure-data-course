using Microsoft.Azure.KeyVault;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyAddressBookPlus
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(KeyVaultService.GetToken));
            var sec = kv.GetSecretAsync(WebConfigurationManager.AppSettings["CacheConnectionSecretUri"]).Result;
            KeyVaultService.CacheConnection = sec.Value;
        }
    }
}
using AmityFlightSystem;
using AmityFlightSystem.Facade;
using AmityFlightSystem.Login;
using AmityFlightSystem.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi.Controllers.Authentications
{
    public class AdministatorAuthentication : AuthorizationFilterAttribute
    { 
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Checks whether user information has been entered
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                    "You must give a username and a password in basic authentication");
                return;
            }

            //Encoding the user name and password from base64
            string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
            string decodedAuthenticationToken = Encoding.UTF8.GetString(
                Convert.FromBase64String(authenticationToken));

            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
            string username = usernamePasswordArray[0];
            string password = usernamePasswordArray[1];

            if (username == "admin" && password == "9999")
            {
                LoginToken<Administrator> adminToken = (LoginToken<Administrator>)FlightSystemCenter.GetInstance().Login(username, password);
                actionContext.Request.Properties["adminToken"] = adminToken;
                FlightSystemCenter f = FlightSystemCenter.GetInstance();
                LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)f.GetFacade(adminToken);
                actionContext.Request.Properties["adminFacade"] = adminFacade;
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "You are not authorized");
            }
        }
    }
}
using AmityFlightSystem;
using AmityFlightSystem.Exceptions;
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
    public class AirlineAuthentication : AuthorizationFilterAttribute
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

            ILoginToken token;
            try
            {
                 token = FlightSystemCenter.GetInstance().Login(username, password);
            }
            catch(UserNotFoundException e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
                return;
            }
            catch(WrongPasswordException e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
                return;
            }

            if (token != null && token.GetType() == typeof(LoginToken<AirlineCompany>))
            {
                LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)token;
                actionContext.Request.Properties["airlineToken"] = token;
                FlightSystemCenter f = FlightSystemCenter.GetInstance();
                LoggedInAirlineFacade airlineFacade= (LoggedInAirlineFacade)f.GetFacade(airlineToken);
                actionContext.Request.Properties["airlineFacade"] = airlineFacade;
            }
        
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "You are not authorized");
            }
        }
    }
}
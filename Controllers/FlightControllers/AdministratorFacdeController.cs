using AmityFlightSystem;
using AmityFlightSystem.Facade;
using AmityFlightSystem.Login;
using AmityFlightSystem.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers.Authentications;

namespace WebApi.Controllers
{
    [AdministatorAuthentication]
    public class AdministratorFacdeController : ApiController
    {
        [Route("admin/createairline")]
        [HttpPost]
        public IHttpActionResult CreateNewAirline([FromBody]AirlineCompany airline)
        {
            LoginToken<Administrator> admintoken = (LoginToken<Administrator>)Request.Properties["adminToken"];
            LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)Request.Properties["adminFacade"];

            if (airline == null)
                    return Content(HttpStatusCode.NotAcceptable, "you didn't send an airline to post");

            try
            {
                adminFacade.CreateNewAirline(admintoken, airline);
                return Ok($"airline {airline.AIRLINE_NAME} created");
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [Route("admin/updateairline/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateAirlineDetails([FromBody]AirlineCompany airline, [FromUri]long id)
        {
            LoginToken<Administrator> admintoken = (LoginToken<Administrator>)Request.Properties["adminToken"];
            LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)Request.Properties["adminFacade"];


            if (airline == null || id<= 0)
                return Content(HttpStatusCode.NotAcceptable, "airline details haven't been filled out correctly");

            try
            {
                airline.ID = id; //does this matter
                adminFacade.UpdateAirlineDetails(admintoken, airline);
                return Ok($"airline {airline.AIRLINE_NAME} details updated");
            }
            catch(Exception)
            {
                return Content(HttpStatusCode.NotFound, $"airline id {id} wasn't found");
            }
        }

        [Route("admin/removeairline/{id}")]
        [HttpDelete]
        public IHttpActionResult RemoveAirline([FromBody]AirlineCompany airline, [FromUri]long id)
        {
            LoginToken<Administrator> admintoken = (LoginToken<Administrator>)Request.Properties["adminToken"];
            LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)Request.Properties["adminFacade"];

            airline.ID = id;
            if (id <= 0)
                return Content(HttpStatusCode.NotAcceptable, $"id {id} is not valid");

            try
            {
                adminFacade.RemoveAirline(admintoken, airline);
                return Ok($"airline {airline.USER_NAME} removed");
            }
            catch(Exception)
            {
                return Content(HttpStatusCode.NotFound, $"airline id {id} wasn't found");
            }
        }


        [Route("admin/createcustomer")]
        [HttpPost]
        public IHttpActionResult CreateNewCustomer([FromBody]Customer customer)
        {
            LoginToken<Administrator> admintoken = (LoginToken<Administrator>)Request.Properties["adminToken"];
            LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)Request.Properties["adminFacade"];

            if (customer == null)
                return Content(HttpStatusCode.NotAcceptable, $"you didn't fill out the information correctly");

            try
            {
                adminFacade.CreateNewCustomer(admintoken, customer);
                return Ok($"customer {customer.USER_NAME} was created");
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
        }

        [Route("admin/updatecustomer/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateCustomerDetails([FromBody]Customer customer, [FromUri]long id)
        {
            LoginToken<Administrator> admintoken = (LoginToken<Administrator>)Request.Properties["adminToken"];
            LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)Request.Properties["adminFacade"];

            customer.ID = id;
            if (customer == null || id <= 0)
                return Content(HttpStatusCode.NotAcceptable, "you didn't fill out the information correctly");

            try
            {
                adminFacade.UpdateCustomerDetails(admintoken, customer);
                return Ok($"customer {customer.USER_NAME} details updated");
            }
            catch(Exception)
            {
                return Content(HttpStatusCode.NotFound, $"customer id {id} wasn't found");
            }
        }

        [Route("admin/removecustomer/{id}")]
        [HttpDelete]
        public IHttpActionResult RemoveCustomer([FromBody]Customer customer, [FromUri]long id)
        {
            LoginToken<Administrator> admintoken = (LoginToken<Administrator>)Request.Properties["adminToken"];
            LoggedInAdministratorFacade adminFacade = (LoggedInAdministratorFacade)Request.Properties["adminFacade"];

            customer.ID = id;
            if (id <= 0)
                return Content(HttpStatusCode.NotAcceptable, $"id {id} is not valid");

            try
            {
                adminFacade.RemoveCustomer(admintoken, customer);
                return Ok($"customer {customer.USER_NAME} was removed");
            }
            catch(Exception)
            {
                return Content(HttpStatusCode.NotFound, $"customer id {id} wasn't found");
            }
        }
    }
}

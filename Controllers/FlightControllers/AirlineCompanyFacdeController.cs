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
    [AirlineAuthentication]
    public class AirlineCompanyFacdeController : ApiController
    {
        [Route("airline/getalltickets")]
        [HttpGet]
        public IHttpActionResult GetAllTickets()
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            IList<Ticket> tickets = airlineFacade.GetAllTickets(airlineToken);
            if (tickets == null || tickets.Count == 0)
                return NotFound();
            return Ok(tickets);
        }

        [Route("airline/getallflights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            IList<Flight> flights = airlineFacade.GetAllFlights(airlineToken);
            if (flights == null || flights.Count == 0)
                return NotFound();
            return Ok(flights);
        }

        [Route("airline/cancelflight/{id}")]
        [HttpDelete]
        public IHttpActionResult CancelFlight([FromUri]int id)
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            Flight flight = airlineFacade.GetFlightById(id);
            if (flight == null || flight.ID <= 0)
                return NotFound();

            airlineFacade.CancelFlight(airlineToken, flight);
            return Ok(flight);
        }

        [Route("airline/createflight")]
        [HttpPost]
        public IHttpActionResult CreateFlight([FromBody]Flight flight)
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            if (flight == null)
                return Content(HttpStatusCode.NotAcceptable, "you didn't send a flight to post");
            try
            {
                airlineFacade.CreateFlight(airlineToken, flight);
                return Ok($"flight {flight.ID} was created");
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [Route("airline/updateflight/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateFlight(Flight flight, [FromUri]int id)
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            if (flight == null || id <= 0)
                return Content(HttpStatusCode.NotAcceptable, "flight details haven't been filled out correctly");

            try
            {
                flight.ID = id;
                airlineFacade.UpdateFlight(airlineToken, flight);
                return Ok($"flight {flight.ID} was created");
            }
            catch(Exception)
            {
                return Content(HttpStatusCode.NotFound, $"flight id {id} wasn't found");
            }
        }

        [Route("airline/changepassword/{oldpasswprd}/{newpassword}")]
        [HttpPut]
        public IHttpActionResult ChangeMyPassword([FromUri]string oldPassword, [FromUri]string newPassword)
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            if (newPassword == oldPassword)
                return BadRequest();
            airlineFacade.ChangeMyPassword(airlineToken, oldPassword, newPassword);
            return Ok();
        }

        [Route("airline/modifyairline/{id}")]
        [HttpPut]
        public IHttpActionResult MofidyAirlineDetails(AirlineCompany airline, [FromUri]int id)
        {
            LoginToken<AirlineCompany> airlineToken = (LoginToken<AirlineCompany>)Request.Properties["airlineToken"];
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)Request.Properties["airlineFacade"];

            if (airline == null || id <= 0)
                return Content(HttpStatusCode.NotFound, $"airline details haven't been filled out correctly");

            try
            {
                airlineFacade.ModifyAirlineDetails(airlineToken, airline);
                return Ok($"airline {airline.USER_NAME} details updated");
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.NotFound, $"airline {airline.ID} wasn't found");
            }
        }
    }
}

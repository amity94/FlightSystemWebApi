using AmityFlightSystem;
using AmityFlightSystem.DAO;
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
    [CustomerAuthentication]
    public class CustomerFacdeController : ApiController
    {

        [Route("customer/getallmyflights")]
        [HttpGet]
        public IHttpActionResult GetAllMyFlights()
        {
            LoginToken<Customer> customerToken = (LoginToken<Customer>)Request.Properties["customerToken"];
            LoggedInCustomerFacade custFacade = (LoggedInCustomerFacade)Request.Properties["customerFacade"];

            IList<Flight> flights = custFacade.GetAllMyFlights(customerToken);
            if (flights == null || flights.Count == 0)
                return NotFound();
            return Ok(flights);
        }

        [Route("customer/purchaseticket/{flightid}")]
        [HttpPut]
        public IHttpActionResult PurchaseTicket([FromUri]int flightid)
        {
            LoginToken<Customer> customerToken = (LoginToken<Customer>)Request.Properties["customerToken"];
            LoggedInCustomerFacade custFacade = (LoggedInCustomerFacade)Request.Properties["customerFacade"];

            Flight flight = custFacade.GetFlightById(flightid);
            if (flight == null || flight.ID <= 0)
                return NotFound();
            try
            {
                Ticket ticket = custFacade.PurchaseTicket(customerToken, flight);
                return Ok(ticket);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }

        [Route("customer/cancelticket/{ticketid}")]
        [HttpDelete]
        public IHttpActionResult CancelTicket([FromUri]int ticketid)
        {
            LoginToken<Customer> customerToken = (LoginToken<Customer>)Request.Properties["customerToken"];
            LoggedInCustomerFacade custFacade = (LoggedInCustomerFacade)Request.Properties["customerFacade"];

            TicketDAOMSSQL ticketDAO = new TicketDAOMSSQL();
            Ticket ticket = ticketDAO.Get(ticketid);
            if (ticket == null || ticket.ID <= 0)
                return NotFound();
            try
            {
                custFacade.CancelTicket(customerToken, ticket);
                return Ok();
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, e.Message);
            }
        }
    }
}

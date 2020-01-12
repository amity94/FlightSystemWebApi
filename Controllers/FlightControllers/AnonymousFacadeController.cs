using AmityFlightSystem;
using AmityFlightSystem.Facade;
using AmityFlightSystem.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApi.Controllers
{
    public class AnonymousFacadeController : ApiController
    {
        public FlightSystemCenter f = new FlightSystemCenter();

        [Route("anonymous/getallflights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            IList<Flight> flights = anonymousFacade.GetAllFlights();
            if (flights == null || flights.Count == 0)
                return NotFound();
            return Ok(flights);
        }

        [Route("anonymous/getallairlinecompanies")]
        [HttpGet]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            IList<AirlineCompany> airlineCompanies = anonymousFacade.GetAllAirlineCompanies();
            if (airlineCompanies == null || airlineCompanies.Count == 0)
                return NotFound();
            return Ok(airlineCompanies);
        }

        [Route("anonymous/getallflightsvacancy")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            Dictionary<Flight, int> vacancy = anonymousFacade.GetAllFlightsVacancy();
            if (vacancy == null || vacancy.Count == 0)
                return NotFound();
            return Ok(vacancy);
        }

        [Route("anonymous/getflightsbyid/{id}")]
        [HttpGet]
        public IHttpActionResult GetFlightById([FromUri]int id)
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            Flight flight = anonymousFacade.GetFlightById(id);
            if (flight == null || id <= 0)
                return NotFound();
            return Ok(flight);
        }

        [Route("anonymous/getflightsbyorigincountry/{countrycode}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByOriginCountry([FromUri]int countryCode)
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            IList<Flight> flights = anonymousFacade.GetFlightsByOriginCountry(countryCode);
            if (flights == null || flights.Count == 0 || countryCode <= 0)
                return NotFound();
            return Ok(flights);
        }

        [Route("anonymous/getflightsbydestinationcountry/{countrycode}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDestinationCountry([FromUri]int countryCode)
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            IList<Flight> flights = anonymousFacade.GetFlightsByDestinationCountry(countryCode);
            if (flights == null || flights.Count == 0 || countryCode <= 0)
                return NotFound();
            return Ok(flights);
        }

        [Route("anonymous/getflightsbydeparturedate/{departureDate}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDepartureDate([FromUri]DateTime departureDate)
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

           IList<Flight> flights = anonymousFacade.GetFlightsByDepatrureDate(departureDate);
            if (flights == null || flights.Count == 0 || departureDate <= DateTime.Now)
                return NotFound();
            return Ok(flights);
        }

        [Route("anonymous/getflightsbylandingtime/{landingtime}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByLandingDate([FromUri]DateTime landingDate)
        {
            AnonymousUserFacade anonymousFacade = (AnonymousUserFacade)f.GetFacade(null);

            IList<Flight> flights = anonymousFacade.GetFlightsByLandingDate(landingDate);
            if (flights == null || flights.Count == 0 || landingDate <= DateTime.Now)
                return NotFound();
            return Ok(flights);
        }
    }
}

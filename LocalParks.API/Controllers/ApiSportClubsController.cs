using LocalParks.API.Controllers.Base;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LocalParks.API.Controllers
{
    public class SportsClubsController : AuthorizedControllerBase
    {
        private readonly ISportsClubsService _service;

        public SportsClubsController(ISportsClubsService service)
        {
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<SportsClubModel[]>> GetAllSportsClubs()
        {
            var results = await _service.GetAllSportsClubModelsAsync();

            return Ok(results);
        }

        [HttpGet("api/parks/{parkId:int}/[controller]")]
        public async Task<ActionResult<SportsClubModel[]>> GetParkSportsClubs(int parkId)
        {
            var results = await _service.GetSportsClubModelsByParkAsync(parkId);

            return Ok(results);
        }
        [HttpGet("api/[controller]/sports")]
        public ActionResult<SportsClubModel[]> GetAllSports()
        {
            var results = _service.GetAllSports();

            return Ok(results);
        }

        [HttpGet("api/[controller]/{clubId:int}")]
        public async Task<ActionResult<SportsClubModel>> GetSportsClub(int clubId)
        {
            var result = await _service.GetSportsClubModelAsync(clubId);

            return Ok(result);
        }

        [HttpGet("api/parks/{parkId:int}/[controller]/{sport}")]
        public async Task<ActionResult<SportsClubModel[]>> GetSportsClub(int parkId, string sport)
        {
            var result = await _service.GetSportsClubModelsBySportAsync(parkId, sport);

            return Ok(result);
        }
    }
}

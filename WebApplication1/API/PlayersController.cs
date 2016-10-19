using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.API
{
    [AllowAnonymous]
    public class PlayersController : ApiController
    {
        [HttpGet, Route("api/players/{id}")]
        public Player SearchById(string id)
        {
            return Player.GetById(id);
        }

        [HttpGet, Route("api/players")]
        public IEnumerable<Player> SearchForPlayers([FromUri]SearchFilter filter)
        {
            return Player.SearchPlayers(filter);
        }
    }
}

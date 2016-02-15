﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using NJsonApi.HelloWorld.Models;

namespace NJsonApi.HelloWorld.Controllers
{
    [Route("api/[controller]")]
    public class ContinentsController : Controller
    {
        [HttpGet]
        public IEnumerable<Continent> Get()
        {
            return StaticPersistentStore.Continents;
        }

        [HttpGet("{id}")]
        public Continent Get(int id)
        {
            return StaticPersistentStore.Continents.Single(w => w.Id == id);
        }
    }
}

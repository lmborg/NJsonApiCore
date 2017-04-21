﻿using Microsoft.AspNetCore.Mvc;
using NJsonApi.Infrastructure;
using NJsonApi.Serialization.Representations;
using NJsonApi.Web.MVCCore.HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NJsonApi.Web.MVCCore.HelloWorld.Controllers
{
    [Route("[controller]")]
    public class ArticlesController : Controller
    {

        [HttpGet]
        public IActionResult Get()
        {
            var a = StaticPersistentStore.Articles;
            var md = new TopLevelDocument<List<Article>>(a);
            md.GetMetaData().Add("response created", DateTime.Now);
            md.GetMetaData().Add("response created by", this.GetType());
            md.Links.Add("link1", new SimpleLink(new Uri("http://localhost")));
            return Ok(md);
        }

        // but you could simply:

        //[HttpGet]
        //public IEnumerable<Article> Get()
        //{
        //    return StaticPersistentStore.Articles;
        //}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var a = StaticPersistentStore.Articles.Single(w => w.Id == id);
            var md = new TopLevelDocument<Article>(a);
            md.GetMetaData().Add("response created", DateTime.Now);
            md.GetMetaData().Add("response created by", this.GetType());
            md.Links.Add("link1", new SimpleLink(new Uri("http://localhost")));
            return new ObjectResult(md);
        }

        // but you could simply:

        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    return new ObjectResult(StaticPersistentStore.Articles.Single(w => w.Id == id));
        //}

        [HttpPost]
        public IActionResult Post([FromBody]Delta<Article> article)
        {
            var newArticle = article.ToObject();
            newArticle.Id = StaticPersistentStore.GetNextId();
            StaticPersistentStore.Articles.Add(newArticle);
            return CreatedAtAction("Get", new { id = newArticle.Id }, newArticle);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch([FromBody]Delta<Article> update, int id)
        {
            var article = StaticPersistentStore.Articles.Single(w => w.Id == id);
            update.ApplySimpleProperties(article);
            return new ObjectResult(article);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            StaticPersistentStore.Articles.RemoveAll(x => x.Id == id);
            return new NoContentResult();
        }
    }
}
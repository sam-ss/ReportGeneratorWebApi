using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportGeneratorWebApi.Helpers;
using ReportGeneratorWebApi.Models; 

namespace ReportGeneratorWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportGeneratorController : ControllerBase
    { 
        // GET: api/ReportGenerator
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ReportGenerator/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ReportGenerator
        [HttpPost]
        public string AddReportData(Dictionary<string, AxeResultModel> pagesDetails)
        {
           // VoilationHtmlGenerator.GenerateHtml(pagesDetails);
            //return HttpResponseMessage.
           
                return VoilationHtmlGenerator.GenerateHtml(pagesDetails);

        }

        // PUT: api/ReportGenerator/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

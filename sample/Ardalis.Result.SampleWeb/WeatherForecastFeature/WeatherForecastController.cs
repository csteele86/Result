﻿using System.Collections.Generic;
using Ardalis.Sample.Core;
using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ardalis.Result.SampleWeb.WeatherForecastFeature
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            WeatherService weatherService,
            ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<IEnumerable<WeatherForecast>> GetForecast([FromBody]ForecastRequestDto model)
        {
            var result = _weatherService.GetForecast(model);
            if (result.Status == ResultStatus.NotFound) return NotFound();
            if (result.Status == ResultStatus.Invalid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("model", error);
                }
                return BadRequest(ModelState);
            }

            return Ok(result.Value);
        }
    }
}
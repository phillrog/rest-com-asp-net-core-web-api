﻿using System;
using DevIO.Api.Controllers;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {

        public TesteController(INotificador notificador, IUser appUser) : base(notificador, appUser)
        {

        }

        [HttpGet]
        public string Valor()
        {
            return "Sou a V2";
        }
    }
}
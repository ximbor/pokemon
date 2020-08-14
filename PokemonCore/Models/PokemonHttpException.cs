using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PokemonCore.Models
{
    public class PokemonHttpException: Exception
    {

        public HttpStatusCode StatusCode { get; set; }

        public PokemonHttpException() : base() { }
        
        public PokemonHttpException(string message) : base(message) { }
        
        public PokemonHttpException(string message, System.Exception innerException) : base(message, innerException) { }

        public PokemonHttpException(string message, HttpStatusCode statusCode) : base(message) {
            StatusCode = statusCode;
        }

    }
}

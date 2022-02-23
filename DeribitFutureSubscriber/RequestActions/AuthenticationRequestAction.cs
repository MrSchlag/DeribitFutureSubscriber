﻿using System;
using System.Threading.Tasks;
using DeribitFutureSubscriber.Models;
using Models.DeribitFutureSubscriber;
using Newtonsoft.Json.Linq;

namespace DeribitFutureSubscriber.RequestActions
{
    public class AuthenticationRequestAction : AbstractRequestAction
    {
        private DateTime _authExpiration = DateTime.MinValue;

        public AuthenticationRequestAction(IClientWebSocket clientWebSocket) : base(clientWebSocket)
        {
        }

        protected override async Task<int> RequestAction(int requestId)
        {
            if (DateTime.Now > _authExpiration)
            {
                var auth = new JsonRfcRequest<AuthRequestParams>
                {
                    JsonRpc = "2.0",
                    Id = requestId,
                    Method = "public/auth",
                    Params = new AuthRequestParams
                    {
                        GrantType = "client_credentials",
                        ClientId = "vrIlT3qo",
                        ClientSecret = "rmhiqP72b2LuLULwzcQ0EnoXRs024zRq6pz1fBIkM8w"
                    }
                };
                _requestIdsWaited.Add(requestId++);

                await _clientWebSocket.Send(auth);
            }
            return requestId;
        }

        protected override Task<bool> HandlerAction(JObject jObject)
        {
            var token = jObject["result"];
            var expires_in = (int)token["expires_in"];
            _authExpiration = DateTime.Now.AddSeconds(expires_in);
            Console.WriteLine("auth OK");

            return Task.FromResult(true);
        }
    }
}
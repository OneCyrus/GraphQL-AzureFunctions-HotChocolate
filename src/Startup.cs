﻿using HotChocolate;
using HotChocolate.Language;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars;
using StarWars.Data;
using StarWars.Types;
using HotChocolate.AspNetCore;

[assembly: FunctionsStartup(typeof(GraphQLAzureFunctions.Startup))]

namespace GraphQLAzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<CharacterRepository>();
            builder.Services.AddSingleton<ReviewRepository>();

            builder.Services.AddSingleton<Query>();

            builder.Services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType<QueryType>()
                .AddType<HumanType>()
                .AddType<DroidType>()
                .AddType<EpisodeType>()
                .AddType<UnitType>()
                .Create());

            builder.Services.AddAzureFunctionsGraphQL(new AzureFunctionsMiddlewareOptions());

            builder.Services.AddSingleton<IDocumentHashProvider>(new MD5DocumentHashProvider());
        }
    }
}

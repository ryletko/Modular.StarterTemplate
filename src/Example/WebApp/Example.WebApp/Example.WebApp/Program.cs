using Example.WebApp;

WebApplication.CreateBuilder(args)
              .RegisterServices()
              .Build()
              .Configure()
              .Run();
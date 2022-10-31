using Wallet.Application.OutputPorts;
using Wallet.Application.UseCases.CreateUser;
using Wallet.Application.UseCases.WalletDeposit;
using Wallet.Repositories;
using Wallet.Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

ConfigureMongo(builder.Services, builder.Configuration);
ConfigureUseCases(builder.Services);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureMongo(IServiceCollection services, IConfiguration configuration)
{
    var connectionStringConfig = configuration.GetConnectionString("MongoDbConnectionString");
    services.AddSingleton<MongoContext, MongoContext>(c => new MongoContext(connectionStringConfig, "WalletDb"));

    services.AddScoped<IUsersRespository, UsersRepository>();
    services.AddScoped<IWalletsRepository, WalletsRepository>();
}

static void ConfigureUseCases(IServiceCollection services)
{
    services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
    services.AddScoped<IWalletDepositUseCase, WalletDepositUseCase>();
}

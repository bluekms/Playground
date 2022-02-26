using System;
using System.Threading.Tasks;
using AuthServer.Handlers.Maintenance;
using AuthServer.Models;
using CommonLibrary.Handlers;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthServer.Controllers
{
    [ApiController]
    public class AddMaintenanceController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly IRuleChecker<AddMaintenanceRule> rule;
        private readonly ICommandHandler<InsertMaintenanceCommand, MaintenanceData> insertMaintenance;

        public AddMaintenanceController(
            ILogger<AddMaintenanceController> logger,
            IMapper mapper,
            IRuleChecker<AddMaintenanceRule> rule,
            ICommandHandler<InsertMaintenanceCommand, MaintenanceData> insertMaintenance)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.rule = rule;
            this.insertMaintenance = insertMaintenance;
        }

        [HttpPost, Route("Auth/Maintenance/Add")]
        public async Task<ActionResult<ReturnData>> AddMaintenance([FromBody] ArgumentData args)
        {
            await rule.CheckAsync(new(args.Start, args.End, args.Reason));
            var newData = await insertMaintenance.ExecuteAsync(new(args.Start, args.End, args.Reason));
            return mapper.Map<ReturnData>(newData);
        }
    }

    public sealed record ArgumentData(DateTime Start, DateTime End, string Reason);

    public sealed record ReturnData(long Id, DateTime Start, DateTime End, string Reason);
}
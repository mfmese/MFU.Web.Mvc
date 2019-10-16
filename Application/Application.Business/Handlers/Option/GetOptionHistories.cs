using Application.Data;
using Framework.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class GetOptionHistoriesRequest : BaseRequest<ResponseOf<List<Models.OptionHistory>>>
    {
        public int OptionId { get; set; }
    }

    class GetOptionHistoriesHandler : BaseHandler<GetOptionHistoriesRequest, ResponseOf<List<Models.OptionHistory>>>
    {
        MFUContext dbContext;

        public GetOptionHistoriesHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {
            var option = dbContext.OptionHistory.Where(x => x.OptionId == Request.OptionId).ToList();

            Response.Data = Mapper.Map<List<Models.OptionHistory>>(option);

            Response.AddMessage("Your operation completed Successfully.", "", MessageTypes.Success, isEndUserMessage: true);

            return Response.SetSuccess().AsTask();
        }
    }
}

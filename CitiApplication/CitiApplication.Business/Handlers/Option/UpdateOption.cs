using Application.Data;
using Application.Data.Entities;
using FluentValidation;
using Framework.MakerChecker;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Business.Handlers
{
    public class UpdateOptionRequest : Framework.Messaging.BaseRequest<VoidResponse>
    {
        public int OptionId { get; set; }
        public string OptionKey { get; set; }
        public string OptionValue { get; set; }
    }

    internal class UpdateOptionRequestValidator : AbstractValidator<UpdateOptionRequest>
    {
        public UpdateOptionRequestValidator()
        {

        }
    }

    class UpdateOptionHandler : MakerCheckerHandler<UpdateOptionRequest, VoidResponse, Data.Entities.Option>
    {
        MFUContext dbContext;

        public UpdateOptionHandler(MFUContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override OperationType OperationType => OperationType.UPDATE;

        protected override Data.Entities.Option GetEntity()
        {
            return dbContext.Option.FirstOrDefault(o => o.OptionId == Request.OptionId);
        }

        protected override Task Handle(CancellationToken cancellationToken)
        {

            var option = Mapper.Map(Request, Entity);

            var optionResult = dbContext.Option.Update(option);

            dbContext.SaveChanges();

            InsertOptionHistory(optionResult.Entity);

            return Response.SetSuccessWithEndUserMessage("Updated Successfully.", "OK").AsTask();
        }
        private void InsertOptionHistory(Option option)
        {
            var optionHistory = Mapper.Map<OptionHistory>(option);

            dbContext.OptionHistory.Add(optionHistory);
            dbContext.SaveChanges();
        }

    }
}

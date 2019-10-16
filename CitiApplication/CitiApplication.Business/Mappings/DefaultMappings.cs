using AutoMapper;
using Application.Business.Handlers;
using Application.Data.Entities;

namespace Application.Business.Mappers
{
	internal class DefaultMappings : Profile
	{
		public DefaultMappings()
		{       
            CreateMap<Models.ExcelFile, UpdateExcelFileRequest>(MemberList.Source).ReverseMap();
            CreateMap<ExcelFile, Models.ExcelFile>(MemberList.Source).ReverseMap();
            CreateMap<ExcelFile, CreateExcelFileRequest > (MemberList.Source).ReverseMap();
            CreateMap<ExcelFile, UpdateExcelFileRequest>(MemberList.Source).ReverseMap();
            CreateMap<ExcelFileHistory, Models.ExcelFileHistory>(MemberList.Source).ReverseMap();
            CreateMap<Mfufile, Models.Mfufile>(MemberList.Source).ReverseMap();

            CreateMap<Option, OptionHistory>(MemberList.Source).ReverseMap();
            CreateMap<Option, UpdateOptionRequest>(MemberList.Source).ReverseMap();
            CreateMap<Option, Models.Option>(MemberList.Source).ReverseMap();
            CreateMap<OptionHistory, Models.OptionHistory>(MemberList.Source).ReverseMap();

            CreateMap<Models.MfuswiftInfo, MfuswiftInfo>(MemberList.Source).ReverseMap();

        }
	}
}

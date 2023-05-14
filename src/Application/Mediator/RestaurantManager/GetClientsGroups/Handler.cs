using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Mediator.RestaurantManager.GetClientsGroups
{
    public class Handler : IRequestHandler<GetClientsGroupsRequest, List<ClientsGroupDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IClientsGroupRepository _clientsGroupRepository;

        public Handler(IMapper mapper, IClientsGroupRepository clientsGroupRepository)
        {
            _mapper = mapper;
            _clientsGroupRepository = clientsGroupRepository;
        }

        public async Task<List<ClientsGroupDTO>> Handle(GetClientsGroupsRequest request, CancellationToken cancellationToken)
        {
            var clientsGroups = await _clientsGroupRepository.GetCollection();

            return _mapper.Map<List<ClientsGroupDTO>>(clientsGroups);
        }
    }
}

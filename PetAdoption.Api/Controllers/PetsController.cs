using Microsoft.AspNetCore.Mvc;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;
        public PetsController(IPetService petService)
        {
            _petService = petService;
        }
        [HttpGet("{id}")]
        public async Task<ApiResponse<PetDetailDto>> GetPet(int id) =>
            await _petService.GetPet(id);

        [HttpGet("")]
        public async Task<ApiResponse<List<PetListDto>>> GetAllPetList() =>
            await _petService.GetAllPetList();

        [HttpGet("new/{count:int}")]
        public async Task<ApiResponse<List<PetListDto>>> GetNewPetList(int count) =>
            await _petService.GetNewPetList(count);



        [HttpGet("popular/{count:int}")]
        public async Task<ApiResponse<List<PetListDto>>> GetPopularPetList(int count) =>
            await _petService.GetPopularPetList(count);

        [HttpGet("random/{count:int}")]
        public async Task<ApiResponse<List<PetListDto>>> GetRandomPetList(int count) =>
            await _petService.GetRandomPetList(count);
    }
}

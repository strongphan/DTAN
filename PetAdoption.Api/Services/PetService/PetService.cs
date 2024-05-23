using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Api.Data;
using PetAdoption.Shared.Dtos;
namespace PetAdoption.Api.Services
{
    public class PetService : IPetService
    {
        private readonly PetContext _context;
        private readonly IMapper _mapper;

        public PetService(PetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<PetListDto>>> GetNewPetList(int count)
        {
            var pets = await _context.Pets.OrderByDescending(p => p.Id).Take(count).ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
        public async Task<ApiResponse<List<PetListDto>>> GetPopularPetList(int count)
        {
            var pets = await _context.Pets.OrderByDescending(p => p.Views).Take(count).ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
        public async Task<ApiResponse<List<PetListDto>>> GetRandomPetList(int count)
        {
            var pets = await _context.Pets.OrderByDescending(_ => Guid.NewGuid()).Take(count).ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
        public async Task<ApiResponse<List<PetListDto>>> GetAllPetList()
        {
            var pets = await _context.Pets.OrderByDescending(p => p.Id).ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
        public async Task<ApiResponse<PetDetailDto>> GetPet(int id)
        {
            var pet = await _context.OwnedPets.AsTracking().Where(p => p.PetId == id).Select(p => p.Pet).FirstOrDefaultAsync();
            var user = await _context.OwnedPets.AsTracking().Where(p => p.PetId == id).Select(p => p.User).FirstOrDefaultAsync();
            var userDto = _mapper.Map<UserDto>(user);
            if (pet == null)
            {
                return ApiResponse<PetDetailDto>.Fail("Không tồn tại thú cưng này");
            }
            else
            {
                pet.Views++;
                await _context.SaveChangesAsync();
            }
            var petDto = _mapper.Map<PetDetailDto>(pet);
            petDto.Owner = userDto;


            return ApiResponse<PetDetailDto>.Success(petDto);
        }

        public async Task<ApiResponse<List<PetListDto>>> GetBySearch(string? input)
        {
            List<Pet> pets;
            if (string.IsNullOrWhiteSpace(input))
            {
                pets = await _context.Pets.OrderByDescending(p => p.Id).ToListAsync();

            }
            else
            {
                pets = await _context.Pets
                .Where(p => p.Name.Contains(input) || p.Type.Contains(input) || p.Breed.Contains(input))
                .OrderByDescending(p => p.Id).ToListAsync();
            }
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
    }
}

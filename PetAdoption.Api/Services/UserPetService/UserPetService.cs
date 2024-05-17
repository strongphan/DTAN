using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Api.Data;
using PetAdoption.Api.helper;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public class UserPetService : IUserPetService
    {
        private static readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly PetContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        // Constructor for UserPetService
        public UserPetService(PetContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        // Toggles a pet as a favorite for a user
        public async Task<ApiResponse> ToggleFavoriteAsync(int userId, int petId)
        {
            // Check if the user has already favorited the pet
            var uf = await _context.UserFavorites
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.PetId == petId);
            if (uf != null)
            {
                // If the user has favorited the pet, remove it from favorites
                _context.UserFavorites.Remove(uf);
            }
            else
            {
                // If the user has not favorited the pet, add it to favorites
                uf = new UserFavorite
                {
                    UserId = userId,
                    PetId = petId
                };
                _context.UserFavorites.Add(uf);
            }
            // Save the changes to the database
            _context.SaveChanges();
            // Return a success response
            return ApiResponse.Success();
        }

        // Gets a list of favorite pets for a user
        public async Task<ApiResponse<List<PetListDto>>> GetFavoritePetList(int userId)
        {
            // Get the favorite pets for the user from the database
            var pets = await _context.UserFavorites
                .Where(p => p.UserId == userId)
                .Select(p => p.Pet)
                .ToListAsync();
            // Map the pets to PetListDto and return the list
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }

        // Gets a list of pets that a user has adopted
        public async Task<ApiResponse<List<PetListDto>>> GetAdoptionPetList(int userId)
        {
            // Get the adopted pets for the user from the database
            var pets = await _context.UserAdoptions
                .Where(p => p.UserId == userId)
                .Select(p => p.Pet)
                .ToListAsync();
            // Map the pets to PetListDto and return the list
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }

        // Allows a user to adopt a pet
        public async Task<ApiResponse> AdopPetAsync(int userId, int petId)
        {
            try
            {
                // Use a semaphore to ensure only one adoption can happen at a time
                await _semaphore.WaitAsync();

                // Get the pet from the database
                var pet = await _context.Pets.AsTracking().FirstOrDefaultAsync(p => p.Id == petId);
                if (pet == null)
                {
                    // If the pet does not exist, return a failure response
                    return ApiResponse.Fail("Không tìm thấy thú cưng");
                }
                if (pet.AdoptionStatus == (int)AdoptionStatus.Adopted)
                {
                    // If the pet has already been adopted, return a failure response
                    return ApiResponse.Fail($"{pet.Name} đã được nhận nuôi");
                }
                // Update the pet's adoption status to adopted
                pet.AdoptionStatus = (int)AdoptionStatus.Adopted;
                // Create a new UserAdoption record
                var user = new UserAdoption
                {
                    UserId = userId,
                    PetId = petId
                };
                // Add the UserAdoption record to the database
                await _context.UserAdoptions.AddAsync(user);
                // Save the changes to the database
                await _context.SaveChangesAsync();
                // Return a success response
                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failure response with the exception message
                return ApiResponse.Fail($"Có lỗi xảy ra: {ex.Message}");
            }
            finally
            {
                // Release the semaphore
                _semaphore.Release();
            }
        }
        public async Task<ApiResponse<List<PetListDto>>> GetOwnerPetList(int userId)
        {
            // Get the adopted pets for the user from the database
            var pets = await _context.OwnedPets
                .Where(p => p.UserId == userId)
                .Select(p => p.Pet)
                .ToListAsync();
            // Map the pets to PetListDto and return the list
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
        // Create a new pet
        public async Task<ApiResponse> CreatePet(int userId, PetCreateDto dto)
        {
            try
            {
                var pet = _mapper.Map<Pet>(dto);
                if (dto.ImageData != null && dto.ImageData.Length > 0)
                {
                    var imagePath = Path.Combine(_env.WebRootPath, "images/pets", $"image_{pet.Id}.jpg");

                    await File.WriteAllBytesAsync(imagePath, dto.ImageData);
                }
                pet.Image = $"image_{pet.Id}.jpg";

                await _context.Pets.AddAsync(pet);
                await _context.SaveChangesAsync();

                var ownerPet = new OwnedPet
                {
                    UserId = userId,
                    PetId = pet.Id, // Use the ID from the pet object
                };
                await _context.OwnedPets.AddAsync(ownerPet);
                await _context.SaveChangesAsync();

                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(ex.Message);
            }
        }

        // Update a pet
        public async Task<ApiResponse> UpdatePet(int petId, PetUpdateDto dto)
        {
            try
            {
                var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
                if (pet == null)
                {
                    return ApiResponse.Fail("Không tìm thấy thú cưng");
                }
                if (!string.IsNullOrEmpty(pet.Image))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, "images/pets", pet.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                if (dto.ImageData != null && dto.ImageData.Length > 0)
                {
                    var imagePath = Path.Combine(_env.WebRootPath, "images/pets", $"image_{pet.Id}.jpg");

                    await File.WriteAllBytesAsync(imagePath, dto.ImageData);
                }
                _mapper.Map(dto, pet);

                pet.Image = $"image_{pet.Id}.jpg";

                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();

                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(ex.Message);
            }
        }

        // Delete a pet
        public async Task<ApiResponse> DeletePet(int petId)
        {
            try
            {
                var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
                if (pet == null)
                {
                    return ApiResponse.Fail("Không tìm thấy thú cưng");
                }
                // Delete the old image from the file system
                if (!string.IsNullOrEmpty(pet.Image))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, "images/pets", pet.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();

                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<PetDetailDto>> GetPet(int petId, int userId, string address)
        {
            var pet = await _context.OwnedPets.AsTracking().Where(p => p.PetId == petId).Select(p => p.Pet).FirstOrDefaultAsync();
            var owner = await _context.OwnedPets.AsTracking().Where(p => p.PetId == petId).Select(p => p.User).FirstOrDefaultAsync();
            var ownerDto = _mapper.Map<UserDto>(owner);
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
            petDto.Owner = ownerDto;
            {
                if (await _context.UserFavorites.AnyAsync(uf => uf.UserId == userId && uf.PetId == petId))
                {
                    petDto.isFavorite = true;
                }
                var mapApi = new HandleMapsApi();
                petDto.Distance = await mapApi.GetDistance(address, ownerDto.Address);

                return ApiResponse<PetDetailDto>.Success(petDto);
            }
        }
        public async Task<ApiResponse<List<PetListDto>>> GetPopularPetList(int count, string address)
        {
            var pets = await _context.Pets
                .Include(p => p.OwnedPets)
                    .ThenInclude(op => op.User)
                .OrderByDescending(p => p.Views)
                .Take(count)
                .ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            for (int i = 0; i < pets.Count; i++)
            {
                petListDto[i].Address = pets[i].OwnedPets.FirstOrDefault().User.Address;
                var mapApi = new HandleMapsApi();
                petListDto[i].Distance = await mapApi.GetDistance(address, petListDto[i].Address);

            }
            petListDto = petListDto.OrderBy(p => p.Distance).ToList();
            return ApiResponse<List<PetListDto>>.Success(petListDto);

        }

        public async Task<ApiResponse<List<PetListDto>>> GetRandomPetList(int count, string address)
        {
            var pets = await _context.Pets
                .Include(p => p.OwnedPets)
                    .ThenInclude(op => op.User)
                .OrderByDescending(_ => Guid.NewGuid())
                .Take(count)
                .ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            for (int i = 0; i < pets.Count; i++)
            {
                petListDto[i].Address = pets[i].OwnedPets.FirstOrDefault().User.Address;
                var mapApi = new HandleMapsApi();
                petListDto[i].Distance = await mapApi.GetDistance(address, petListDto[i].Address);

            }
            petListDto = petListDto.OrderBy(p => p.Distance).ToList();
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
        public async Task<ApiResponse<List<PetListDto>>> GetAllPetList(string address)
        {
            var pets = await _context.Pets.OrderByDescending(p => p.Id).ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }

        public async Task<ApiResponse<List<PetListDto>>> GetNewPetList(int count, string address)
        {

            var pets = await _context.Pets
                .Include(p => p.OwnedPets)
                    .ThenInclude(op => op.User)
                .OrderByDescending(p => p.Id)
                .Take(count)
                .ToListAsync();
            var petListDto = _mapper.Map<List<PetListDto>>(pets);
            for (int i = 0; i < pets.Count; i++)
            {
                petListDto[i].Address = pets[i].OwnedPets.FirstOrDefault().User.Address;
                var mapApi = new HandleMapsApi();
                petListDto[i].Distance = await mapApi.GetDistance(address, petListDto[i].Address);

            }
            petListDto = petListDto.OrderBy(p => p.Distance).ToList();
            return ApiResponse<List<PetListDto>>.Success(petListDto);
        }
    }
}

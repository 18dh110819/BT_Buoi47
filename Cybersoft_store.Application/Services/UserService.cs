using System.Net;
using Cybersoft_store.Helper;
using Cybersoft_store.Infrastructure.Models;

public interface IUserService
{
    Task<ResponseType<string>> Register(UserRegisterDto registerDto);
    Task<ResponseType<string>> Login(LoginDto loginDto);
    Task<ResponseType<ProfileDto>> GetProfile(string token);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtService;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, JwtAuthService jwtService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<ResponseType<string>> Register(UserRegisterDto registerDto)
    {
        try
        {
            // Tạo một đối tượng User mới từ registerDto
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                PasswordHash = HelperFunction.HashPassword(registerDto.Password), // Lưu ý: bạn nên mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                FullName = registerDto.Fullname,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                Address = registerDto.Address,
                CreatedAt = DateTime.Now,
                Alias = HelperFunction.StringToSlug(registerDto.Fullname),
                Avatar = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(registerDto.Fullname)}",
                Deleted = false,
                // Gán vai trò mặc định cho người dùng mới (Cách 1: Thêm thẳng vào List)
                UserRoles =
                {
                    new() {
                        RoleId = (int)UserRoles.User // Giả sử RoleId 2 là vai trò mặc định cho người dùng mới
                    }
                }
            };

            //Cách 2: Tạo một đối tượng UserRole mới và thêm vào danh sách UserRoles của người dùng mới
            // var userRole = new UserRole
            // {
            //     UserId = newUser.Id,
            //     RoleId = (int)UserRoles.User // Giả sử RoleId 2 là vai trò mặc định cho người dùng mới
            // };
            // newUser.UserRoles.Add(userRole);


            var existUsername = await _userRepository.GetFirstOrDefaultAsync(u => u.Username == registerDto.Username);
            var existEmail = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == registerDto.Email);
            var existPhone = await _userRepository.GetFirstOrDefaultAsync(u => u.Phone == registerDto.Phone);

            if (existUsername != null)
            {
                return new ResponseType<string>
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Message = UserResponseMessage.UsernameAlreadyExists,
                    DataResponse = UserResponseMessage.UsernameAlreadyExists,
                    Timestamp = DateTime.Now
                };
            }

            if (existEmail != null)
            {
                return new ResponseType<string>
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Message = UserResponseMessage.EmailAlreadyExists,
                    DataResponse = UserResponseMessage.EmailAlreadyExists,
                    Timestamp = DateTime.Now
                };
            }

            if (existPhone != null)
            {
                return new ResponseType<string>
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Message = UserResponseMessage.PhoneAlreadyExists,
                    DataResponse = UserResponseMessage.PhoneAlreadyExists,
                    Timestamp = DateTime.Now
                };
            }

            await _userRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseType<string>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = UserResponseMessage.RegisterSuccess,
                DataResponse = UserResponseMessage.RegisterSuccess,
                Timestamp = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            // Xử lý lỗi nếu cần thiết
            return new ResponseType<string>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = UserResponseMessage.RegisterFailed,
                DataResponse = UserResponseMessage.RegisterFailed,
                Timestamp = DateTime.Now
            };
        }
    }

    public async Task<ResponseType<string>> Login(LoginDto loginDto)
    {
        // Kiểm tra thông tin đăng nhập
        var token = _jwtService.GenerateToken(loginDto);
        if (string.IsNullOrEmpty(token)) // Lưu ý: bạn nên so sánh mật khẩu đã mã hóa
        {
            return new ResponseType<string>
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                DataResponse = string.Empty,
                Message = UserResponseMessage.UserNotValid,
                Timestamp = DateTime.UtcNow
            };
        }

        return new ResponseType<string>
        {
            StatusCode = (int)HttpStatusCode.Accepted,
            DataResponse = token,
            Message = UserResponseMessage.UserLoginSuccess,
            Timestamp = DateTime.UtcNow
        };

        // Thực hiện các bước xác thực và tạo token JWT nếu cần thiết
    }

    public async Task<ResponseType<ProfileDto>> GetProfile(string token)
    {
        var userId = _jwtService.DecodePayloadToken(token);
        var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id.ToString() == userId);

        return new ResponseType<ProfileDto>
        {
            StatusCode = 200,
            DataResponse = new ProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Alias = user.Alias,
                Email = user.Email,
                Phone = user.Phone,
                Avatar = user.Avatar
            },
            Message = "",
            Timestamp = DateTime.UtcNow
        };

    }
}
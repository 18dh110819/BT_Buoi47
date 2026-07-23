using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cybersoft_store.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtAuthService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly IUserRepository _context;
    public JwtAuthService(IConfiguration Configuration, IUserRepository db)
    {
        _key = Configuration["Jwt:Key"];
        _issuer = Configuration["Jwt:Issuer"];
        _audience = Configuration["Jwt:Audience"];
        _context = db;
    }

    public string GenerateToken(LoginDto userLogin)
    {

        var userModel = _context.GetFirstOrDefaultAsync(x => x.Username == userLogin.UserNameOrEmailOrPhone ||
        x.Phone == userLogin.UserNameOrEmailOrPhone || x.Email == userLogin.UserNameOrEmailOrPhone).Result;

        if (userModel == null) return string.Empty;
        if (!HelperFunction.VerifyPassword(userLogin.Password, userModel.PasswordHash)) return string.Empty;


        // Khóa bí mật để ký token
        var key = Encoding.ASCII.GetBytes(_key);
        // Tạo danh sách các claims cho token
        var claims = new List<Claim>
        {
            new Claim("UserName", userModel.Username),               // Claim mặc định cho username
            // new Claim(ClaimTypes.Role, userLogin.Role),                   // Claim mặc định cho Role
            new Claim(JwtRegisteredClaimNames.Sub, userModel.Id.ToString()),   // Subject của token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) // Thời gian tạo token
        };

        //Thêm role
        var lstRoles = userModel.UserRoles.Where(x => x.UserId == userModel.Id)
            .Select(x => x.Role.RoleName.ToString())
            .ToList();
        foreach (var roles in lstRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roles));
        }

        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(2), // Token hết hạn sau 1 giờ
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience,              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa
        return tokenHandler.WriteToken(token);
    }

    public string DecodePayloadToken(string token)
    {
        try
        {
            // Kiểm tra token có null hoặc rỗng không
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token không được để trống", nameof(token));
            }

            // Tạo handler và đọc token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy username từ claims (thường nằm trong claim "sub" hoặc "name")
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub); // Common in some identity providers

            if (userIdClaim == null)
            {
                throw new InvalidOperationException("Không tìm thấy userId trong payload");
            }

            return userIdClaim.Value;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (có thể log lỗi ở đây)
            throw new InvalidOperationException($"Lỗi khi decode token: {ex.Message}", ex);
        }
    }

}
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Cybersoft_store.Infrastructure.Models;
// using Microsoft.IdentityModel.Tokens;

// public class JwtAuthService
// {
//     private readonly string? _key;
//     private readonly string? _issuer;
//     private readonly string? _audience;
//     private readonly CybersoftMarketPlaceContext _context;
//     public JwtAuthService(IConfiguration Configuration, CybersoftMarketPlaceContext db)
//     {
//         _key = Configuration["jwt:Key"];
//         _issuer = Configuration["jwt:Issuer"];
//         _audience = Configuration["jwt:Audience"];
//         _context = db;
//     }

//     public string GenerateToken(LoginDto userLogin)
//     {
//         // Khóa bí mật để ký token
//         var key = Encoding.ASCII.GetBytes(_key);

//         var user = _context.Users.SingleOrDefault(x =>
//             x.Email == userLogin.UserNameOrEmailOrPhone ||
//             x.Username == userLogin.UserNameOrEmailOrPhone ||
//             x.Phone == userLogin.UserNameOrEmailOrPhone
//         );

//         // Tạo danh sách các claims cho token
//         var claims = new List<Claim>
//         {
//             new("UserName", user.Username),               // Claim mặc định cho username
//             // new Claim(ClaimTypes.Role, userLogin.Role),                   // Claim mặc định cho Role
//             new(JwtRegisteredClaimNames.Sub, user.Email),   // Subject của token
//             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
//             new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) // Thời gian tạo token
//         };

//         var lstRoles = _context.Roles.Where(x => x.Id == user.Id).Select(x => x.IdRoleNavigation.Rolename).ToList();
//         foreach (var roles in lstRoles)
//         {
//             claims.Add(new Claim(ClaimTypes.Role, roles));
//         }

//         // Tạo khóa bí mật để ký token
//         var credentials = new SigningCredentials(
//             new SymmetricSecurityKey(key),
//             SecurityAlgorithms.HmacSha256Signature
//         );
//         // Thiết lập thông tin cho token
//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(claims),
//             Expires = DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
//             SigningCredentials = credentials,
//             Issuer = _issuer,                 // Thêm Issuer vào token
//             Audience = _audience,              // Thêm Audience vào token
//         };
//         // Tạo token bằng JwtSecurityTokenHandler
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         // Trả về chuỗi token đã mã hóa
//         return tokenHandler.WriteToken(token);
//     }

//     public string DecodePayloadToken(string token)
//     {
//         try
//         {
//             // Kiểm tra token có null hoặc rỗng không
//             if (string.IsNullOrEmpty(token))
//             {
//                 throw new ArgumentException("Token không được để trống", nameof(token));
//             }

//             // Tạo handler và đọc token
//             var handler = new JwtSecurityTokenHandler();
//             var jwtToken = handler.ReadJwtToken(token);

//             // Lấy username từ claims (thường nằm trong claim "sub" hoặc "name")
//             var usernameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserName"); // Common in some identity providers

//             if (usernameClaim == null)
//             {
//                 throw new InvalidOperationException("Không tìm thấy username trong payload");
//             }

//             return usernameClaim.Value;
//         }
//         catch (Exception ex)
//         {
//             // Xử lý lỗi (có thể log lỗi ở đây)
//             throw new InvalidOperationException($"Lỗi khi decode token: {ex.Message}", ex);
//         }
//     }

// }
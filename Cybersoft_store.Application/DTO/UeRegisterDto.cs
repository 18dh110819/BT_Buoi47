using System.ComponentModel.DataAnnotations;

public class UeRegisterDto
{
    [Required(ErrorMessage = "Username là bắt buộc.")]
    [StringLength(32, MinimumLength = 6, ErrorMessage = "Username phải có độ dài từ 6 đến 32 ký tự.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password là bắt buộc.")]
    [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Fullname là bắt buộc.")]
    [RegularExpression("^[^0-9]*$", ErrorMessage = "Fullname không được chứa số.")]
    public string Fullname { get; set; }

    [Required(ErrorMessage = "Phone là bắt buộc.")]
    [Phone(ErrorMessage = "Phone không hợp lệ.")]
    public string Phone { get; set; }

    public string? Address { get; set; }
}
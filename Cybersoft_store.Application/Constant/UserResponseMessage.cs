public class UserResponseMessage
{
    public const string RegisterSuccess = "Đăng ký thành công.";
    public const string RegisterFailed = "Đăng ký thất bại.";
    public const string UsernameAlreadyExists = "Username thoại đã tồn tại.";
    public const string EmailAlreadyExists = "Email đã tồn tại.";
    public const string PhoneAlreadyExists = "Số điện thoại đã tồn tại.";
}

public class CategoryResponseMessage
{
    public const string GetAllCategorySuccess = "Categories retrieved successfully"; 
    public const string CreateCategorySuccess = "Đã thêm loại sản phẩm trong cửa hàng.";
    public const string CreateCategoryFailed = "Đã xảy ra lỗi khi đang thêm loại sản phẩm vào cửa hàng.";
    public const string CreateCategoryAlreadyExists = "Đã tồn tại loại sản phẩm trong cửa hàng.";
}
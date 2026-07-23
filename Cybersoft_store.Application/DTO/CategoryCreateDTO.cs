using System.ComponentModel.DataAnnotations;

public class CategoryCreateDTO
{
    /// <summary>
    /// Tên loại sản phẩm(bắt buộc) có độ dài từ 2 đến 100 ký tự
    /// </summary>
    /// <value>sản phẩm thịt tươi</value>
	[Required]
	[StringLength(100, MinimumLength = 2)]
	public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Mã cửa hàng khi chọn cửa hàng trong dropdown
    /// </summary>
    /// <value>207 - Shop Tổng Hợp ....</value>
	[Required]
	public int ShopId { get; set; }
}
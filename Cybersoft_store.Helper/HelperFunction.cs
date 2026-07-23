using System.Text.RegularExpressions;


namespace Cybersoft_store.Helper;

public static class HelperFunction
{
    private static readonly Regex WhitespaceRegex = new(@"\s+");
    private static readonly Regex NonWordRegex = new(@"[^\w-]");
    private static readonly Regex MultipleDashRegex = new("-+");

    public static string StringToSlug(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return string.Empty;

        string slug = name.ToLower();

        var map = new (string[] From, string To)[]
        {
            (new[] { "à","á","ả","ã","ạ","ă","ặ","ắ","ằ","ẳ","ẵ","â","ầ","ấ","ẩ","ẫ","ậ" }, "a"),
            (new[] { "è","é","ẻ","ẽ","ẹ","ê","ề","ế","ể","ễ","ệ" }, "e"),
            (new[] { "ì","í","ỉ","ĩ","ị" }, "i"),
            (new[] { "ò","ó","ỏ","õ","ọ","ô","ồ","ố","ổ","ỗ","ộ","ơ","ờ","ớ","ở","ỡ","ợ" }, "o"),
            (new[] { "ù","ú","ủ","ũ","ụ","ư","ừ","ứ","ử","ữ","ự" }, "u"),
            (new[] { "ỳ","ý","ỷ","ỹ","ỵ" }, "y"),
            (new[] { "đ" }, "d"),
        };

        foreach (var (froms, to) in map)
        {
            foreach (var from in froms)
            {
                slug = slug.Replace(from, to);
            }
        }

        slug = WhitespaceRegex.Replace(slug, "-");
        slug = NonWordRegex.Replace(slug, "");
        slug = MultipleDashRegex.Replace(slug, "-");
        slug = slug.Trim('-');
        return slug;
    }

    public static string HashPassword(string password, int workFactor = 12)
    {
        if(string.IsNullOrEmpty(password))
            throw new ArgumentException("Mật khẩu không được để trống", nameof(password));
        if(workFactor < 4 || workFactor > 31)
            throw new ArgumentException("Salt chỉ được trong khoảng từ 4 đến 31", nameof(workFactor));
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
    }

    public static bool VerifyPassword(string password, string hashedPass)
    {
        if(string.IsNullOrEmpty(password))
            throw new ArgumentException("Mật khẩu không được để trống", nameof(password));
        if(string.IsNullOrEmpty(hashedPass))
            throw new ArgumentException("Mật khẩu Hash không được để trống", nameof(hashedPass));

        return BCrypt.Net.BCrypt.Verify(password, hashedPass);
    }
}

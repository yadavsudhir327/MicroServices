namespace WebAppMicro.Utility
{
    public class SD
    {
        public static string CouponBaseAPI {  get; set; }
        public static string AuthBaseAPI { get; set; }
        public static string ProductBaseAPI { get; set; }
        public static  string ShoppingCartBaseAPI { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}

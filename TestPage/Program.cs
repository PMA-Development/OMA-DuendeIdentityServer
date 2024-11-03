namespace TestPage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("cookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5000";
                options.ClientId = "testID2";
                options.ClientSecret = "uKKCCAlB8Qys/8QZr/udsR5VfFSpyTKLay/Fa/ngICU=";

                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "query";

                // options.CallbackPath = "/signin-oidc"; // default redirect URI

                // options.Scope.Add("oidc"); // default scope
                // options.Scope.Add("profile"); // default scope
                //options.Scope.Add("api1.read");
                options.SaveTokens = true;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}

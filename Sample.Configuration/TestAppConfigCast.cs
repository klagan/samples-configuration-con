using Microsoft.Extensions.Configuration;
using Sample.Configuration.Models.Settings;

namespace Sample.Configuration
{
    public class TestAppConfigCast
    {
        public void Execute(IConfiguration config)
        {
            var person = new Person();

            config.GetSection("Person").Bind(person);

            var result = config.GetSection("Person").Get<Person>();
        }
    }
}
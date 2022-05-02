using RestSharp;

using System.ComponentModel;
using System.Reflection;

namespace QianShi.Music.Extensions
{
    public static class IRequestExtensions
    {
        public static RestRequest AddQueryParameters(this RestRequest request, object parameter)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (parameter is null) return request;

            var type = parameter.GetType();
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(parameter);
                if (prop.PropertyType.IsEnum)
                {
                    value = Convert.ToInt32(value);
                }

                if (prop.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute description
                    && value != null
                    && !string.IsNullOrWhiteSpace(description.Description)
                    && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    request.AddQueryParameter(description.Description, value.ToString());
                }
            }
            return request;
        }
    }
}
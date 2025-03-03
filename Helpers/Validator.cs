using SimoshStore;

namespace SimoshStoreAPI;

public static class Validator
{
    public static ServiceResult Validate(object dto)
    {
        var validationResults = new List<string>();
        var properties = dto.GetType().GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(dto);
            if (value == null)
            {
                validationResults.Add($"{property.Name} is null");
            }
        }

        if (validationResults.Any())
        {
            return new ServiceResult(false, string.Join(", ", validationResults));
        }

        return new ServiceResult(true, "Validation succeeded");
    }
}

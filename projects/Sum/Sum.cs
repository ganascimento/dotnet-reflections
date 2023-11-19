namespace Sum;

public class Sum
{
    public decimal Invoke(params decimal[] numbers)
    {
        decimal total = 0;

        foreach (var number in numbers)
        {
            total += number;
        }

        return total;
    }
}

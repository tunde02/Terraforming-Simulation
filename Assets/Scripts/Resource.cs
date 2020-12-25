using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Resource
{
    private readonly string[] units = { "", "a", "b", "c", "d", "e", "f" };

    public ResourceID resourceId;
    public string name;
    public long storage;
    public long income;
    public string storageOverview;
    public string incomeOverview;

    public Resource(ResourceID resourceId, string name, long storage, long income)
    {
        this.resourceId = resourceId;
        this.name = name;
        this.storage = storage;
        this.income = income;

        UpdateOverviews();
    }

    public void UpdateOverviews()
    {
        int storageUnit = 0;
        int incomeUnit = 0;
        double storageCompare = 1000;
        double incomeCompare = 1000;

        while (storage >= storageCompare)
        {
            storageCompare *= 1000;
            storageUnit++;
        }

        while (income >= incomeCompare)
        {
            incomeCompare *= 1000;
            incomeUnit++;
        }

        storageOverview = $"{Math.Floor(storage / (storageCompare / 1000) * 10) * 0.1d}{units[storageUnit]}";
        incomeOverview = $"{Math.Floor(income / (incomeCompare / 1000) * 10) * 0.1d}{units[incomeUnit]}";
    }
}

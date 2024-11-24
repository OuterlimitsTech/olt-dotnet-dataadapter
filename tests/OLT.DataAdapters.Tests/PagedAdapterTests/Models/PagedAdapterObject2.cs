﻿using OLT.DataAdapters.Tests.Assets.Models;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Models;

public class PagedAdapterObject2 : PersonTwo
{

    public static PagedAdapterObject2 FakerData()
    {
        var result = new PagedAdapterObject2
        {
            Name = new PersonName
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            }
        };


        return result;
    }

    public static List<PagedAdapterObject2> FakerList(int number)
    {
        var list = new List<PagedAdapterObject2>();
        for (int i = 0; i < number; i++)
        {
            var item = FakerData();
            list.Add(item);
        }
        return list;
    }
}

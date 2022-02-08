using JsonMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonMicroservice.Data
{
    public class Repository
    {
        private IEnumerable<JsonModel> _dataRepository { get; set; }

        public Repository()
        {
            _dataRepository = new[]
            {
                new JsonModel
                {
                    Temperature = 5.6,
                    Pressure = 1001.25
                },
                new JsonModel
                {
                    Temperature = 7.9,
                    Pressure = 1011.13
                },
                new JsonModel
                {
                    Temperature = 0.6,
                    Pressure = 1111.14
                },
                new JsonModel
                {
                    Temperature = -5.6,
                    Pressure = 9999.99
                },
            };
        }

        public JsonModel GetData()
        {
            JsonModel randomValue = _dataRepository.OrderBy(x => new Random().Next()).First();
            return randomValue;
        }
    }
}
